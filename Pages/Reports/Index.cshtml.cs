using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto.Data;
using SparkAuto.Models;
using SparkAuto.Models.ViewModel;
using SparkAuto.Utility;

namespace SparkAuto.Pages.Reports{

    [Authorize]
    public class IndexModel : PageModel{

        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public ReportsViewModel ReportsVM {get;set;}

        public IActionResult OnGet()
        {
            ReportsVM = new ReportsViewModel();
            
            //REPORT MOST POPULAR SERVICE BUYED
            var result1 = _db.ServiceDetails.GroupBy(x => x.ServiceTypeId).Select(g => new { key = g.Key, count = g.Count()}).OrderBy(x => x.count).Last();
            ReportsVM.mospopularservicename = _db.ServiceDetails.Find(result1.key).ServiceName;
            ReportsVM.mostpopularservicecount = result1.count;

            //REPORT MOST VALUE CLIENT
            var result2 = _db.ServiceHeader.GroupBy(c => c.CarId).Select(a => new { carid = a.Key, suma = a.Sum( x => x.FullPrice) }).OrderBy(x => x.suma).Last();
            ReportsVM.mostvalueclientname = _db.Users.Find(_db.Car.Find(result2.carid).UserId).UserName;
            ReportsVM.mostvalueclientmoney = result2.suma;

            ReportsVM.totaltoday = _db.ServiceHeader.Where(x => x.DateAdded.Date == DateTime.Now.Date).Sum(x => x.FullPrice);
            ReportsVM.totalmonth = _db.ServiceHeader.Where(x => x.DateAdded.Month == DateTime.Now.Month).Sum(x => x.FullPrice);

            //REPORT MOST SOLD SERVICES
            var result3 = _db.ServiceDetails.GroupBy(x => x.ServiceTypeId).Select(g => new {key = g.Key, count = g.Count()}).OrderBy(x => x.count).ToList();
            List<KeyValuePair<int,ServiceDetails>> lista = new List<KeyValuePair<int,ServiceDetails>>();
            foreach(var item in result3){
                lista.Add(new KeyValuePair<int,ServiceDetails>(item.count,_db.ServiceDetails.Find(item.key)));
            }

            




            return Page();
        }


    }

}