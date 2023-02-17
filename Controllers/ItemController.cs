using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TabApp.Models;


namespace TabApp.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly dbContext _context;

        public ItemController(dbContext context)
        {
            _context = context;
        }

        // GET: Item
        public async Task<IActionResult> Index()
        {
            return View(await _context.Item.Include("Person").ToListAsync());
        }

        public async Task<IActionResult> SelectOwner(string Description, string SerialNumber, string NameFilter, string SurnameFilter)
        {
            ViewBag.Description = Description;
            ViewBag.SerialNumber = SerialNumber;
            ViewBag.NameFilter = NameFilter;
            ViewBag.SurnameFilter = SurnameFilter;

            IQueryable<Person> unfiltered_users = _context.Person;

            if (!String.IsNullOrEmpty(NameFilter))
            {
                unfiltered_users = unfiltered_users.Where(u => u.Name.Contains(NameFilter));
            }
            if (!String.IsNullOrEmpty(SurnameFilter))
            {
                unfiltered_users = unfiltered_users.Where(u => u.Surname.Contains(SurnameFilter));
            }
            unfiltered_users = unfiltered_users.OrderBy(m => m.Surname);

            var users = await unfiltered_users.ToListAsync();

            return View(users);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItemWithPerson(string Description, string SerialNumber, int PersonID)
        {
            Item item = new Item
            {
                SerialNumber = SerialNumber,
                Description = Description
            };

            var person = _context.Person.Where(p => p.ID == PersonID).FirstAsync();
            
            if (person.Result == null)
                return RedirectToAction(nameof(Index));

            item.Person = person.Result;
            _context.Add(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> RepairList(int? itemID)
        {
            if (itemID == null)
            {
                return NotFound();
            }

            var item = await _context.Item
            .Include(i => i.Repair)
            .FirstOrDefaultAsync(it => it.ID == itemID);

            var repairs  = new List<Repair>();
            foreach(var repair in item.Repair)
            {
                var tmpService =  await _context.Repair.Include(r => r.PickupCode).Include(r => r.RepairStatus).FirstOrDefaultAsync(r => r.ID == repair.ID);
                repairs.Add(tmpService);
            }

            return View(repairs);
        }
        public IActionResult AddNewPerson(string Description, string SerialNumber)
        {
            ViewBag.Description = Description;
            ViewBag.SerialNumber = SerialNumber;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewPerson(string Description,string SerialNumber,[Bind("ID,Role,Name,Surname,Address,Email,PhoneNumber")] Person person)
        {
            Item item = new Item();

            item.SerialNumber=SerialNumber;
            item.Description = Description;

            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
            }

            item.Person = person;
            _context.Add(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Item/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Item/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SerialNumber,Description")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Item/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SerialNumber,Description")] Item item)
        {
            if (id != item.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ID))
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
            return View(item);
        }

        // GET: Item/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Item.FindAsync(id);
            _context.Repair.RemoveRange(_context.Repair.Where(x => x.Item == item));
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ID == id);
        }
    }
}
