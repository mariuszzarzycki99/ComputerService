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
    [Authorize(Policy = Policies.ManagerPolicy)]
    public class WorkerController : Controller
    {
        private readonly dbContext _context;

        public WorkerController(dbContext context)
        {
            _context = context;
        }

        // GET: Worker
        public async Task<IActionResult> Index()
        {
            var dbContext = _context.Worker.Include(w => w.Person);
            return View(await dbContext.ToListAsync());
        }

        // GET: Worker/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Worker
                .Include(w => w.Person)
                .FirstOrDefaultAsync(m => m.PersonID == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // GET: Worker/Create
        public IActionResult Create()
        {
            ViewData["PersonID"] = new SelectList(_context.Person, "ID", "Address");
            return View();
        }

        // POST: Worker/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Earnings,PESEL,AccountNumber,JobPosition,PersonID,Name,Surname,Address,Email,PhoneNumber")] Worker worker, Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                worker.PersonID = person.ID;
                _context.Add(worker);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["PersonID"] = new SelectList(_context.Person, "ID", "Address", worker.PersonID);
            return View(worker);
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewData["EmployeeRole"] = Roles.Employee;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            [Bind("Name,Surname,Address,Email,Role,PhoneNumber")] Person person,
            [Bind("Earnings,PESEL,AccountNumber,JobPosition")] Worker worker,
            [Bind("UserName,Password")] LoginCredentials loginCredentials)
        {
            if (ModelState.IsValid)
            {
                var email = await _context.Person
                .FirstOrDefaultAsync(l => l.Email.Equals(person.Email));

                var login = await _context.LoginCredentials
                .FirstOrDefaultAsync(l => l.UserName.Equals(loginCredentials.UserName));

                if (login != null)
                {
                    TempData["Error"] = "Login is taken!";
                    return View();
                }

                if (email != null)
                {
                    TempData["Error"] = "Email is taken!";
                    return View();
                }

                _context.Add(person);
                
                await _context.SaveChangesAsync();
                worker.PersonID = person.ID;
                loginCredentials.ID = person.ID;
                _context.Add(worker);
                _context.Add(loginCredentials);
                await _context.SaveChangesAsync();

                return Redirect("/");
            }

            TempData["Error"] = "Invalid credentials";
            return View();
        }

        // GET: Worker/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Worker.FindAsync(id);
            if (worker == null)
            {
                return NotFound();
            }
            ViewData["PersonID"] = new SelectList(_context.Person, "ID", "Address", worker.PersonID);
            return View(worker);
        }

        // POST: Worker/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonID,Earnings,PESEL,AccountNumber,JobPosition")] Worker worker)
        {
            if (id != worker.PersonID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(worker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerExists(worker.PersonID))
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
            ViewData["PersonID"] = new SelectList(_context.Person, "ID", "Address", worker.PersonID);
            return View(worker);
        }

        // GET: Worker/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Worker
                .Include(w => w.Person)
                .FirstOrDefaultAsync(m => m.PersonID == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // POST: Worker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var worker = await _context.Worker.FindAsync(id);
            _context.Worker.Remove(worker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkerExists(int id)
        {
            return _context.Worker.Any(e => e.PersonID == id);
        }
    }
}
