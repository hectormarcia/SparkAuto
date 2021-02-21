using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SparkAuto.Data;
using SparkAuto.Models.ViewModel;
using System.Collections.Generic;
using System.Web;

namespace SparkAuto.Pages.Reports{

    [Authorize]
    public class IndexModel : PageModel{

        public readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public ReportsViewModel ReportsVM {get;set;}
        
        public SearchReportViewModel search {get;set;}
        

        [HttpGet]
        public IActionResult OnGet(SearchReportViewModel search)
        {
            ReportsVM = new ReportsViewModel();
            ViewData["plate"] = search.plate;
            ViewData["model"] = search.model;
            ViewData["username"] = search.username;
            ViewData["initdate"] = search.initdate;
            ViewData["enddate"] = search.enddate;
            ViewData["generalgraph"] = search.generalgraph;
            ViewData["invoicegraph"] = search.invoicegraph;
            
            //REPORT MOST POPULAR SERVICE BUYED
            var result1 = _db.ServiceDetails.GroupBy(x => x.ServiceTypeId).Select(g => new { key = g.Key, count = g.Count()}).OrderBy(x => x.count).Last();
            ReportsVM.mospopularservicename = _db.ServiceDetails.Find(result1.key).ServiceName;
            ReportsVM.mostpopularservicecount = result1.count;

            //REPORT MOST VALUE CLIENT
            var result2 = _db.ServiceHeader.GroupBy(c => c.CarId).Select(a => new { carid = a.Key, suma = a.Sum( x => x.FullPrice) }).OrderBy(x => x.suma).Last();
            ReportsVM.mostvalueclientname = _db.ApplicationUser.Find(_db.Car.Find(result2.carid).UserId).Name;
            ReportsVM.mostvalueclientmoney = result2.suma;

            ReportsVM.totaltoday = _db.ServiceHeader.Where(x => x.DateAdded.Date == DateTime.Now.Date).Sum(x => x.FullPrice);
            ReportsVM.totalmonth = _db.ServiceHeader.Where(x => x.DateAdded.Month == DateTime.Now.Month).Sum(x => x.FullPrice);

            
            switch(search.generalgraph){
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


            ReportsVM.facturas = _db.ServiceHeader.ToList();
            foreach(var f in ReportsVM.facturas){
                f.Car = _db.Car.Find(f.CarId);
                f.Car.ApplicationUser = _db.ApplicationUser.Find(f.Car.UserId);
            }
            
            
            if(search == null){
                int nmax = ReportsVM.facturas.Count > 10 ? 10 : ReportsVM.facturas.Count;
                ReportsVM.facturas = ReportsVM.facturas.OrderByDescending(x => x.Id).ToList().GetRange(0,nmax);
            }
            
            if(search.plate != null){
                ReportsVM.facturas = ReportsVM.facturas.Where(x => x.Car.VIN.Equals(search.plate)).ToList();
            }
            
            if(search.model != null){
                ReportsVM.facturas = ReportsVM.facturas.Where(x => x.Car.Model.ToLower().Equals(search.model.ToLower())).ToList();
            }
            
            if(search.username != null){
                ReportsVM.facturas = ReportsVM.facturas.Where(x => x.Car.ApplicationUser.UserName.ToLower().Contains(search.username.ToLower())).ToList();
            }

            if(search.initdate != null){
                ReportsVM.facturas = ReportsVM.facturas.Where(x => x.DateAdded >= DateTime.Parse(search.initdate)).ToList();
            }

            if(search.enddate != null){
                ReportsVM.facturas = ReportsVM.facturas.Where(x => x.DateAdded <= DateTime.Parse(search.enddate).AddDays(1)).ToList();
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
                ReportsVM.graphvalues += Math.Round(item.suma,2) + ",";
            }
            
            ReportsVM.graphlabels = ReportsVM.graphlabels.Substring(0, ReportsVM.graphlabels.Length - 1);
            ReportsVM.graphvalues = ReportsVM.graphvalues.Substring(0, ReportsVM.graphvalues.Length - 1);

            ReportsVM.type = ReportsViewModel.GrapType.bar;
            ReportsVM.axislabel = "Sales of day";
        }



    }

}