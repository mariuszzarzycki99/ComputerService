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
    public class PriceListController : Controller
    {
        private readonly dbContext _context;

        public PriceListController(dbContext context)
        {
            _context = context;
        }

        // GET: PriceList
        public async Task<IActionResult> Index()
        {
            return View(await _context.PriceList.ToListAsync());
        }

        // GET: PriceList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceList = await _context.PriceList
                .FirstOrDefaultAsync(m => m.ID == id);
            if (priceList == null)
            {
                return NotFound();
            }

            return View(priceList);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Preview()
        {
            return View(await _context.PriceList.ToListAsync());
        }

        // GET: PriceList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PriceList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Price,Description")] PriceList priceList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(priceList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(priceList);
        }

        // GET: PriceList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceList = await _context.PriceList.FindAsync(id);
            if (priceList == null)
            {
                return NotFound();
            }
            return View(priceList);
        }

        // POST: PriceList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Price,Description")] PriceList priceList)
        {
            if (id != priceList.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(priceList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PriceListExists(priceList.ID))
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
            return View(priceList);
        }

        // GET: PriceList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceList = await _context.PriceList
                .FirstOrDefaultAsync(m => m.ID == id);
            if (priceList == null)
            {
                return NotFound();
            }

            return View(priceList);
        }

        // POST: PriceList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var priceList = await _context.PriceList.FindAsync(id);
            _context.PriceList.Remove(priceList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PriceListExists(int id)
        {
            return _context.PriceList.Any(e => e.ID == id);
        }
    }
}
