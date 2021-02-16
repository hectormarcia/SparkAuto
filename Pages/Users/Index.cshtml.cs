using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto.Data;
using SparkAuto.Models;
using SparkAuto.Models.ViewModel;
using SparkAuto.Utility;

namespace SparkAuto.Pages.Users
{
    [Authorize(Roles = Utility.SD.AdminEndUser + "," + Utility.SD.CustomerEndUser)]
   
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public UsersListViewModel UsersListVM { get; set; }

        public async Task<IActionResult> OnGet(int productPage=1, string searchPlate=null, string searchEmail=null, string searchName=null, string searchPhone=null)
        {
            UsersListVM = new UsersListViewModel()
            {
                ApplicationUserList = await _db.ApplicationUser.ToListAsync()
            };

            StringBuilder param = new StringBuilder();
            param.Append("/Users?productPage=:");
            param.Append("&seachPlate=");
            if(searchPlate != null){
                param.Append(searchPlate);
            }
            param.Append("&searchName=");
            if(searchName!=null)
            {
                param.Append(searchName);
            }
            param.Append("&searchEmail=");
            if (searchEmail != null)
            {
                param.Append(searchEmail);
            }
            param.Append("&searchPhone=");
            if (searchPhone != null)
            {
                param.Append(searchPhone);
            }

            

            if(searchPlate != null){
                Car car = await _db.Car.Where(x => x.VIN.Equals(searchPlate)).FirstOrDefaultAsync();
                string userid = car != null ? car.UserId : "0";
                UsersListVM.ApplicationUserList = await _db.ApplicationUser.Where(u => u.Id.Equals(userid)).ToListAsync();
            }else{
                if(searchEmail!=null)
                {
                    UsersListVM.ApplicationUserList = await _db.ApplicationUser.Where(u => u.Email.ToLower().Contains(searchEmail.ToLower())).ToListAsync();
                }
                else
                {
                    if (searchName != null)
                    {
                        UsersListVM.ApplicationUserList = await _db.ApplicationUser.Where(u => u.Name.ToLower().Contains(searchName.ToLower())).ToListAsync();
                    }
                    else 
                    {
                        if (searchPhone != null)
                        {
                            UsersListVM.ApplicationUserList = await _db.ApplicationUser.Where(u => u.PhoneNumber.ToLower().Contains(searchPhone.ToLower())).ToListAsync();
                        }
                    }
                }
            }

            var count = UsersListVM.ApplicationUserList.Count;

            UsersListVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = SD.PaginationUsersPageSize,
                TotalItems = count,
                UrlParam = param.ToString()
            };

            UsersListVM.ApplicationUserList = UsersListVM.ApplicationUserList.OrderBy(p => p.Email)
                .Skip((productPage - 1) * SD.PaginationUsersPageSize)
                .Take(SD.PaginationUsersPageSize).ToList();
            
            return Page();
        }

    }
}
