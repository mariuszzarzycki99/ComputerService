using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TabApp.Models;


namespace TabApp.Controllers
{
    public class UserRepairsController : Controller
    {
        private readonly dbContext _context;

        public UserRepairsController(dbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {   
            var currentUserID = await _context.Person.Where(u => u.LoginCredentials.UserName == User.Identity.Name).Select(p => p.ID).FirstAsync();
            /////////

            var items = _context.Item.Include(i => i.Repair).Where(item=>item.Person.ID == currentUserID).ToListAsync();

            var repairs  = new List<Repair>();
            foreach(var item in items.Result)
            {
            foreach(var repair in item.Repair)
            {
                var tmpRepair =  await _context.Repair.Include(r => r.PickupCode).Include(r => r.RepairStatus).FirstOrDefaultAsync(r => r.ID == repair.ID);
                repairs.Add(tmpRepair);
            }
            }

            return View(repairs);
        }
        public async Task<IActionResult> UserServices(int? repairID)
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
        private bool ServiceExists(int id)
        {
            return _context.Service.Any(e => e.ID == id);
        }
    }
}
