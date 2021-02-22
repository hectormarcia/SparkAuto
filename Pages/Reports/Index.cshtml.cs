using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SparkAuto.Data;
using SparkAuto.Models;
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
        
        public List<Chart> chartitems {get;set;}

        [HttpGet]
        public IActionResult OnGet(SearchReportViewModel search)
        {
            ReportsVM = new ReportsViewModel();
            chartitems = new List<Chart>();
            ViewData["plate"] = search.plate;
            ViewData["model"] = search.model;
            ViewData["username"] = search.username;
            ViewData["initdate"] = search.initdate;
            ViewData["enddate"] = search.enddate;
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


            chartitems.Add(DailyEarnings());
            chartitems.Add(ServicesGraph());
            chartitems.Add(FutureAppointments());


            ReportsVM.facturas = _db.ServiceHeader.ToList();
            foreach(var f in ReportsVM.facturas){
                f.Car = _db.Car.Find(f.CarId);
                f.Car.ApplicationUser = _db.ApplicationUser.Find(f.Car.UserId);
            }
            
            
            if(search.plate != null){
                ReportsVM.facturas = ReportsVM.facturas.Where(x => x.Car.VIN.Contains(search.plate)).ToList();
            }
            
            if(search.model != null){
                ReportsVM.facturas = ReportsVM.facturas.Where(x => x.Car.Model.ToLower().Contains(search.model.ToLower())).ToList();
            }
            
            if(search.username != null){
                ReportsVM.facturas = ReportsVM.facturas.Where(x => x.Car.ApplicationUser.Name.ToLower().Contains(search.username.ToLower())).ToList();
            }

            if(search.initdate != null){
                ReportsVM.facturas = ReportsVM.facturas.Where(x => x.DateAdded >= DateTime.Parse(search.initdate)).ToList();
            }

            if(search.enddate != null){
                ReportsVM.facturas = ReportsVM.facturas.Where(x => x.DateAdded <= DateTime.Parse(search.enddate).AddDays(1)).ToList();
            }

            int nmax = ReportsVM.facturas.Count > 10 ? 10 : ReportsVM.facturas.Count;
            if(search.maxrows != 0){
                nmax = ReportsVM.facturas.Count > search.maxrows ? search.maxrows : ReportsVM.facturas.Count;
            }
            
            ReportsVM.facturas = ReportsVM.facturas.OrderByDescending(x => x.Id).ToList().GetRange(0,nmax);

            
            return Page();
        }


        

        public Chart ServicesGraph(){
            Chart chart = new Chart();
            chart.axislabel = "Number of sales";
            chart.type = ChartType.doughnut;

            List<string> labels = new List<string>();
            List<string> values = new List<string>();

            var result = _db.ServiceDetails.GroupBy(x => x.ServiceTypeId).Select(g => new {key = g.Key, count = g.Count()}).ToList();
            
            foreach (var item in result){
                labels.Add(_db.ServiceType.Find(item.key).Name);
                values.Add(item.count + "");
            }

            chart.labels = labels;
            chart.values = values;
    
            return chart;

        }


        public Chart DailyEarnings(){
            Chart chart = new Chart();
            chart.axislabel = "Sales of day";
            chart.type = ChartType.line;

            List<string> labels = new List<string>();
            List<string> values = new List<string>();
            
            var result = _db.ServiceHeader.GroupBy(x => x.DateAdded.Day).Select(x => new {headerid = x.Key, suma = x.Sum(y => y.FullPrice)}).OrderBy(x => x.headerid);
            
            foreach(var item in result){
                labels.Add("Day " + item.headerid);
                values.Add(Math.Round(item.suma,2).ToString());
            }
            
            chart.labels = labels;
            chart.values = values;

            return chart;
        }

        public Chart FutureAppointments(){
            Chart chart = new Chart();
            chart.axislabel = "Nex Services Date";
            chart.type = ChartType.bar;

            List<string> labels = new List<string>();
            List<string> values = new List<string>();

            
            var result = _db.ServiceHeader.Where(x => x.NextServiceDate.Date > DateTime.Now.Date).GroupBy(x => x.NextServiceDate.Date).Select(x => new {
                n = x.Count(), date = x.Key
            }).ToList();

            if(result.Count > 15){
                result = result.GetRange(0,15);
            }

            foreach(var item in result){
                labels.Add(item.date.ToString("dd/MM/yyyy"));
                values.Add(item.n+"");
            }

            chart.labels = labels;
            chart.values = values;

            return chart;
        }



    }

}