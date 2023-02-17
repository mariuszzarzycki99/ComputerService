using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TabApp.Enums;
using TabApp.Models;


namespace TabApp.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly dbContext _context;

        public MessageController(dbContext context)
        {
            _context = context;
        }

        // GET: Message
        public async Task<IActionResult> Index()
        {
            return View(await _context.Message.ToListAsync());
        }

        // GET: Reply
        public async Task<IActionResult> Reply(uint? ID)
        {
            if (ID == null)
                return RedirectToAction(nameof(Mailbox));

            var msg = await _context.Message.Where(msg => msg.ID == ID).Include("Sender").FirstOrDefaultAsync();

            if (msg == null)
                return RedirectToAction(nameof(AddresseeNotFound));

            var login = await _context.LoginCredentials.Where(l => l.Person.ID == msg.Sender.ID).FirstOrDefaultAsync();

            if (login == null)
                return RedirectToAction(nameof(AddresseeNotFound));

            ViewBag.receiver = login.UserName;
            ViewBag.msgTitle = "re: " + msg.Title;

            return View("Create");
        }


        // GET: Message/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .FirstOrDefaultAsync(m => m.ID == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }
        // GET: Message/AddresseeNotFound
        public IActionResult AddresseeNotFound()
        {
            return View();
        }

        // GET: Message/Create
        public IActionResult Create()
        {
            ViewBag.msgTitle = "";
            ViewBag.receiver = "";
            return View();
        }
        // POST: Message/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string recv, [Bind("Content, Title")] Message message)
        {
            if (ModelState.IsValid)
            {
                if (recv == "" || recv == null)
                    return RedirectToAction(nameof(AddresseeNotFound));

                var addr = _context.Person.Where(p => p.LoginCredentials.UserName == recv).FirstOrDefaultAsync();
                if (addr.Result == null)
                    return RedirectToAction(nameof(AddresseeNotFound));

                var sender = _context.Person.FirstOrDefaultAsync(p => p.LoginCredentials.UserName == User.Identity.Name);

                message.Addressee = addr.Result;
                message.Sender = sender.Result;
                message.Date = DateTime.Now;
                _context.Add(message);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Mailbox));
        }

        // GET: Message/Mailbox
        public async Task<IActionResult> Mailbox(string FromFilter, string TitleFilter)
        {
            ViewBag.FromFilter = FromFilter;
            ViewBag.TitleFilter = TitleFilter;

            var name = User.Identity.Name;
            var unfiltered_messages = _context.Message.Include("Sender").Where(recv => recv.Addressee.LoginCredentials.UserName == name);

            if (!String.IsNullOrEmpty(FromFilter))
            {
                unfiltered_messages = unfiltered_messages.Where(msg => msg.Addressee.LoginCredentials.UserName.Contains(FromFilter));
            }
            if (!String.IsNullOrEmpty(TitleFilter))
            {
                unfiltered_messages = unfiltered_messages.Where(msg => msg.Title.Contains(TitleFilter));
            }
            unfiltered_messages = unfiltered_messages.OrderByDescending(m => m.Date);

            var messages = await unfiltered_messages.ToListAsync();

            foreach (var msg in messages)
            {
                msg.Sender = await _context.Person.Include("LoginCredentials").Where(p => p.ID == msg.Sender.ID).FirstOrDefaultAsync();
            }


            return View(messages);
        }

        public async Task<IActionResult> ShowMessage(uint? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Mailbox));

            var currentUserID = await _context.Person.Where(u => u.LoginCredentials.UserName == User.Identity.Name).Select(p => p.ID).FirstAsync();

            var message = await _context.Message.Include("Sender").Include("Addressee").Where(msg => msg.ID == id).FirstAsync();

            message.Sender = await _context.Person.Include("LoginCredentials").Where(p => p.ID == message.Sender.ID).FirstOrDefaultAsync();

            if (currentUserID != message.Addressee.ID)
                return Unauthorized();

            return View(new[] { message });
        }

        // GET: Message/Mailbox
        public async Task<IActionResult> Sendbox(string FromFilter, string TitleFilter)
        {
            ViewBag.FromFilter = FromFilter;
            ViewBag.TitleFilter = TitleFilter;

            var name = User.Identity.Name;
            var unfiltered_messages = _context.Message.Include("Addressee").Where(send => send.Sender.LoginCredentials.UserName == name);

            if (!String.IsNullOrEmpty(FromFilter))
            {
                unfiltered_messages = unfiltered_messages.Where(msg => msg.Addressee.LoginCredentials.UserName.Contains(FromFilter));
            }
            if (!String.IsNullOrEmpty(TitleFilter))
            {
                unfiltered_messages = unfiltered_messages.Where(msg => msg.Title.Contains(TitleFilter));
            }
            unfiltered_messages = unfiltered_messages.OrderByDescending(m => m.Date);

            var messages = await unfiltered_messages.ToListAsync();
            foreach (var msg in messages)
            {
                msg.Addressee = await _context.Person.Include("LoginCredentials").Where(p => p.ID == msg.Addressee.ID).FirstOrDefaultAsync();
            }


            return View(messages);
        }

        public async Task<IActionResult> ShowSentMessage(uint? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Mailbox));

            var currentUserID = await _context.Person.Where(u => u.LoginCredentials.UserName == User.Identity.Name).Select(p => p.ID).FirstAsync();

            var message = await _context.Message.Include("Sender").Include("Addressee").Where(msg => msg.ID == id).FirstAsync();

            message.Addressee = await _context.Person.Include("LoginCredentials").Where(p => p.ID == message.Addressee.ID).FirstOrDefaultAsync();

            if (currentUserID != message.Sender.ID)
                return Unauthorized();

            return View(new[] { message });
        }

        // GET: Message/SendToWorker
        public IActionResult SendToWorker()
        {
            return View();
        }
        // POST: Message/SendToWorker
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToWorker([Bind("Content, Title")] Message message)
        {
            if (ModelState.IsValid)
            {
                var adresee = _context.Person.Where(p => p.Role == "Support").ToList();
                var support = adresee.ElementAt((new Random()).Next(adresee.Count()));

                var sender = _context.Person.FirstOrDefaultAsync(p => p.LoginCredentials.UserName == User.Identity.Name);

                message.Addressee = support;
                message.Sender = sender.Result;
                message.Date = DateTime.Now;
                _context.Add(message);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Mailbox));
        }

        // GET: Message/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }

        // POST: Message/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Content,Date")] Message message)
        {
            if (id != message.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        // GET: Message/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FirstOrDefaultAsync(m => m.ID == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Message/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Mailbox));

            var currentUserID = await _context.Person.Where(u => u.LoginCredentials.UserName == User.Identity.Name).Select(p => p.ID).FirstAsync();

            var message = await _context.Message.Include("Addressee").Where(msg => msg.ID == id).FirstAsync();

            if (currentUserID != message.Addressee.ID)
                return Unauthorized();

            _context.Message.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Mailbox));
        }

        private bool MessageExists(int id)
        {
            return _context.Message.Any(e => e.ID == id);
        }
    }
}
