using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TabApp.Models;


namespace TabApp.Controllers
{
    public class Statistics : Controller
    {
        private readonly dbContext _context;

        public Statistics(dbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult WorkerRaport()
        {
            var dictionary = new Dictionary<Person,int>();
            var users = _context.Person.ToList();
            foreach(var user in users)
            {
                dictionary.Add(user, 0);
            }
            var services = _context.Service.ToArray();
            foreach(var service in services)
            {
                if(dictionary.ContainsKey(service.Person))
                {
                    dictionary[service.Person]=+1;
                }
            }
            var list = new List<KeyValuePair<Person, int>>();
            foreach(var entity in dictionary)
            {
                if(entity.Value!=0)
                {
                    list.Add(new KeyValuePair<Person, int>(entity.Key,entity.Value));
                }
            }
            return View(list);
        }
        public IActionResult RepairRaport()
        {
            var dictionary = new Dictionary<PriceList,int>();
            var priceList = _context.PriceList.ToList();
            foreach(var PL in priceList)
            {
                dictionary.Add(PL, 0);
            }

            var services = _context.Service.ToArray();
            foreach(var service in services)
            {
                if(dictionary.ContainsKey(service.PriceList))
                {
                    dictionary[service.PriceList]=+1;
                }
            }
            var list = new List<KeyValuePair<PriceList, int>>();
            foreach(var entity in dictionary)
            {
                    list.Add(new KeyValuePair<PriceList, int>(entity.Key,entity.Value));
            }
            return View(list);
        }

        private bool ServiceExists(int id)
        {
            return _context.Service.Any(e => e.ID == id);
        }
    }
}
