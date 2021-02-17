using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SparkAuto.Data;
using SparkAuto.Models.ViewModel;

namespace SparkAuto.Pages.Reports{

    [Authorize]
    public class IndexModel : PageModel{

        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public ReportsViewModel ReportsVM {get;set;}

        [HttpGet]
        public IActionResult OnGet(int graphtype)
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
            switch(graphtype){
                case 1:
                    ServicesGraph();
                    break;
                case 2:
                    DailyEarnings();
                    break;
                default:
                    ServicesGraph();
                    break;
            }

            return Page();
        }

        public void ServicesGraph(){
            var result = _db.ServiceDetails.GroupBy(x => x.ServiceTypeId).Select(g => new {key = g.Key, count = g.Count()}).ToList();
            foreach (var item in result){
                ReportsVM.graphlabels += _db.ServiceType.Find(item.key).Name + ",";
                ReportsVM.graphvalues += item.count + ",";
            }

            ReportsVM.graphlabels = ReportsVM.graphlabels.Substring(0, ReportsVM.graphlabels.Length - 1);
            ReportsVM.graphvalues = ReportsVM.graphvalues.Substring(0, ReportsVM.graphvalues.Length - 1);
            ReportsVM.type = ReportsViewModel.GrapType.doughnut;
            ReportsVM.axislabel = "Number of sales";
        }


        public void DailyEarnings(){
            ReportsVM.graphlabels = "";
            ReportsVM.graphvalues = "";
            var result = _db.ServiceHeader.GroupBy(x => x.DateAdded.Day).Select(x => new {headerid = x.Key, suma = x.Sum(y => y.FullPrice)}).OrderBy(x => x.headerid);
            
            foreach(var item in result){
                ReportsVM.graphlabels += "Day " + item.headerid +  ",";
                ReportsVM.graphvalues += item.suma + ",";
            }
            
            ReportsVM.graphlabels = ReportsVM.graphlabels.Substring(0, ReportsVM.graphlabels.Length - 1);
            ReportsVM.graphvalues = ReportsVM.graphvalues.Substring(0, ReportsVM.graphvalues.Length - 1);

            ReportsVM.type = ReportsViewModel.GrapType.bar;
            ReportsVM.axislabel = "Sales of month";
        }



    }

}