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

namespace SparkAuto.Pages.Settings{

    [Authorize]
    public class IndexModel : PageModel{
        
        public readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public CommonValues cv {get;set;}

        public IActionResult OnGet(){
            cv = //null;
            _db.CommonValues.Find(1);
            return Page();
        }

        public IActionResult OnPost(string iva, string envCharge){
            CommonValues cv = new CommonValues{
                Id = 1,
                iva = Double.Parse(iva),
                envCharge = Double.Parse(envCharge)
            };
            _db.CommonValues.Update(cv);
            _db.SaveChanges();

            return RedirectToPage("Index");
        }

    }
    
}