using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TabApp.Models;

namespace TabApp.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        private readonly dbContext _context;

        public ServiceController(dbContext context)
        {
            _context = context;
        }

        // GET: Service
        public async Task<IActionResult> Index(int? repairID)
        {
            if (repairID == null)
            {
                return NotFound();
            }
            
            var repair = await _context.Repair
            .Include(r => r.Service)
            .Include(r => r.PickupCode)
            .FirstOrDefaultAsync(m => m.ID == repairID);       

            var services  = new List<Service>();
            foreach(var service in repair.Service)
            {
                var tmpService =  await _context.Service.Include(s => s.PriceList).FirstOrDefaultAsync(s => s.ID == service.ID);  
                services.Add(tmpService);
            }
            ViewBag.PickupCode = repair.PickupCode.Value;
            ViewBag.RepairID = repairID;
            return View(services);
        }

        // GET: Service/Details/5
        public async Task<IActionResult> Details(int? id, int? repairID)
        {
            if (id == null || repairID == null)
            {
                return NotFound();
            }

            var service = await _context.Service
                .Include(s => s.PriceList)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (service == null)
            {
                return NotFound();
            }
            ViewBag.RepairID = repairID;
            return View(service);
        }

         public async Task<IActionResult> Preview()
        {
            return View(await _context.Service.ToListAsync());
        }

        // GET: Service/Create
        public async Task<IActionResult> Create(int? repairID)
        {
            if(repairID == null)
            {
                return NotFound();
            }

            var repair = await _context.Repair.FindAsync(repairID);
            if (repair == null)
            {
                    return NotFound();
            }
            
            ViewBag.RepairID = repairID;
            ViewData["PriceList"] = new SelectList(_context.PriceList, "ID", "Description");
            return View();
        }

        // POST: Service/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int repairID, int priceListID, [Bind("ID,WarrantyDate,PartsCost")] Service service)
        {
            if (ModelState.IsValid)
            {
                var repair = await _context.Repair.FindAsync(repairID);
                service.Repair = repair;

                var priceList = await _context.PriceList.FindAsync(priceListID);
                service.PriceList = priceList;

                service.Person = await _context.Person.Include("LoginCredentials").Where(u => u.LoginCredentials.UserName == User.Identity.Name).FirstAsync();

                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { repairID });
            }
            ViewData["PriceList"] = new SelectList(_context.PriceList, "ID", "Description");
            return View(service);
        }

        // GET: Service/Edit/5
        public async Task<IActionResult> Edit(int? id, int? repairID)
        {
            if (id == null || repairID == null)
            {
                return NotFound();
            }

            var service = await _context.Service.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            
            ViewBag.RepairID = repairID;
            return View(service);
        }

        // POST: Service/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int repairID, [Bind("ID,WarrantyDate")] Service service)
        {
            if (id != service.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { repairID });
            }
            return View(service);
        }

        // GET: Service/Delete/5
        public async Task<IActionResult> Delete(int? id, int? repairID)
        {
            if (id == null || repairID == null)
            {
                return NotFound();
            }

            var service = await _context.Service
                .FirstOrDefaultAsync(m => m.ID == id);
            if (service == null)
            {
                return NotFound();
            }

            ViewBag.RepairID = repairID;
            return View(service);
        }

        // POST: Service/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int repairID)
        {
            var service = await _context.Service.FindAsync(id);
            _context.Service.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { repairID } );
        }

        private bool ServiceExists(int id)
        {
            return _context.Service.Any(e => e.ID == id);
        }
    }
}
