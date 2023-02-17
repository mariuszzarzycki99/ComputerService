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
    [Authorize(Policy = Policies.EmployeePolicy)]
    public class InvoiceController : Controller
    {
        private readonly dbContext _context;

        public InvoiceController(dbContext context)
        {
            _context = context;
        }

        // GET: Invoice
        public async Task<IActionResult> Index()
        {
            return View(await _context.Invoice.ToListAsync());
        }

        // GET: Invoice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .Include("Repair")
                .FirstOrDefaultAsync(m => m.ID == id);
            if (invoice == null)
            {
                return NotFound();
            }

            ViewData["ID"] = invoice.Repair.ID;
            return View(invoice);
        }

        
        public async Task<IActionResult> Generate(int? invoiceID)
        {
            if (invoiceID == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .Include("Repair")
                .FirstOrDefaultAsync(i => i.ID == invoiceID);

            if (invoice == null)
            {
                return NotFound();
            }

            var repair = await _context.Repair
            .Include("Service")
            .Include("Item.Person")
            .FirstOrDefaultAsync(m => m.ID == invoice.Repair.ID); 

            if (repair == null)
            {
                return NotFound();
            }

            var services  = new List<Service>();
            foreach(var service in repair.Service)
            {
                var tmpService =  await _context.Service.Include(s => s.PriceList).FirstOrDefaultAsync(s => s.ID == service.ID);  
                services.Add(tmpService);
            }

            repair.Service = services;
            ViewData["NIP"] = invoice.NIP;
            ViewData["InvoiceDate"] = invoice.InvoiceDate.ToString("dd/MM/yyyy");
            return View(repair);
        }

        // GET: Invoice/Create
        public async Task<IActionResult> Create(int? repairID)
        {
            if (repairID == null)
            {
                return NotFound();
            }
           
            var repair = await _context.Repair
                .Include("Invoice")
                .FirstOrDefaultAsync(r => r.ID == repairID);
            
            if (repair == null)
            {
                return NotFound();
            }
            if(repair.Invoice != null)
            {
                return RedirectToAction("Generate", "Invoice", new { invoiceID = repair.Invoice.ID});
            }

           ViewBag.RepairID = repairID;

            return View();
        }

        // POST: Invoice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int repairID, [Bind("ID,NIP,InvoiceDate")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                var repair = await _context.Repair.FindAsync(repairID);
                if (repair == null)
                {
                    return NotFound();
                }

                invoice.Repair = repair;
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction("Generate", "Invoice", new { invoiceID = invoice.ID});
            }
            return View(invoice);
        }

        // GET: Invoice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        // POST: Invoice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,NIP,InvoiceDate")] Invoice invoice)
        {
            if (id != invoice.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.ID))
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
            return View();
        }

        // GET: Invoice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .FirstOrDefaultAsync(m => m.ID == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
            _context.Invoice.Remove(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoice.Any(e => e.ID == id);
        }
    }
}
