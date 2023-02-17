using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TabApp.Models;
using TabApp.Enums;
using Microsoft.AspNetCore.Authorization;

namespace TabApp.Controllers
{
    [Authorize]
    public class RepairController : Controller
    {
        private readonly dbContext _context;

        public RepairController(dbContext context)
        {
            _context = context;
        }

        // GET: Repair
        public async Task<IActionResult> Index()
        {
            var dbContext = _context.Repair.Include(r => r.Item).Include(r => r.RepairStatus).Include(r => r.PickupCode);
            return View(await dbContext.ToListAsync());
        }

        // GET: Repair/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repair
                .Include(r => r.Item)
                .Include(r => r.RepairStatus)
                .Include(r => r.PickupCode)
                .FirstOrDefaultAsync(m => m.ID == id);
    
            if (repair == null)
            {
                return NotFound();
            }

            return View(repair);
        }
        public IActionResult Add()
        {
            return View();
        }

        // GET: Repair/Create/itemId
        public async Task<IActionResult> Create(int? itemID)
        {
            if (itemID != null)
            {
                var item = await _context.Item.FindAsync(itemID);
                if (item == null)
                {
                    return NotFound();
                }
                ViewBag.ItemID = itemID;
                ViewBag.ItemSerialNumber = item.SerialNumber;
                ViewBag.ItemDescription = item.Description; 
            }

            return View();
        }



        public string Status { get; set; }

        // POST: Repair/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? itemID, [Bind("AdmissionDate,Cost,Warranty")] Repair repair, 
        [Bind("SerialNumber,Description")] Item item)
        {
            if (ModelState.IsValid)
            {
                if(itemID != null)
                {
                    var existingItem = await _context.Item.FindAsync(itemID);
                    if (existingItem == null)
                    {
                        return NotFound();
                    }
                    repair.Item = existingItem;
                }  
                else
                {
                    _context.Add(item);
                    await _context.SaveChangesAsync();
                    repair.Item = item;
                }

                var repairStatus = await _context.RepairStatus.FindAsync(1);
                repair.RepairStatus = repairStatus;


                _context.Add(repair);
                await _context.SaveChangesAsync();

                var pickupCode = await _context.PickupCodes.FindAsync(repair.ID);
                repair.PickupCode = pickupCode;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(repair);
        }

        // GET: Repair/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repair
                .Include(r => r.RepairStatus)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (repair == null)
            {
                return NotFound();
            }
            ViewData["RepairStatus"] = new SelectList(_context.RepairStatus, "ID", "Status", repair.RepairStatus.ID);
            return View(repair);
        }

        // POST: Repair/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int repairStatusID, [Bind("ID,AdmissionDate,Warranty")] Repair repair)
        {
            if (id != repair.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldRepair = await _context.Repair
                    .Include(r => r.Item)
                    .Include(r => r.PickupCode)
                    .Include(r => r.Service)
                    .FirstOrDefaultAsync(m => m.ID == id);
                    
                    var repairStatus = await _context.RepairStatus.FindAsync(repairStatusID);

                    oldRepair.AdmissionDate = repair.AdmissionDate;
                    oldRepair.Warranty = repair.Warranty;
                    oldRepair.RepairStatus = repairStatus;

                    if(repairStatus.Status == RepairStatuses.Ready)
                    {
                        oldRepair.IssueDate = DateTime.Today;
                        if(oldRepair.Service != null)
                        {
                            oldRepair.Cost = 0;
                            foreach(var service in oldRepair.Service)
                            {
                                var tmpService =  await _context.Service.Include(s => s.PriceList).FirstOrDefaultAsync(s => s.ID == service.ID); 
                                if(tmpService != null) 
                                    oldRepair.Cost += tmpService.PriceList.Price;

                                if(tmpService.PartsCost != null) 
                                   oldRepair.Cost += tmpService.PartsCost;                       
                            }
                        }
                        
                    }

                    _context.Update(oldRepair);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepairExists(repair.ID))
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
            ViewData["RepairStatus"] = new SelectList(_context.RepairStatus, "ID", "Status", repair.RepairStatus.ID);
            //ViewData["ItemID"] = new SelectList(_context.Item, "ID", "Description", repair.ItemID);
            return View(repair);
        }

        // GET: Repair/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repair
                .Include(r => r.Item)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (repair == null)
            {
                return NotFound();
            }

            return View(repair);
        }

        // POST: Repair/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repair = await _context.Repair
            .Include("Invoice")
            .FirstOrDefaultAsync(m => m.ID == id);
            _context.Service.RemoveRange(_context.Service.Where(x => x.Repair == repair));
            _context.Invoice.Remove(repair.Invoice);
            _context.Repair.Remove(repair);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepairExists(int id)
        {
            return _context.Repair.Any(e => e.ID == id);
        }
    }
}
