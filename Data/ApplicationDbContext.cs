using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SparkAuto.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SparkAuto.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<ServiceType> ServiceType { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Car> Car { get; set; }
        public DbSet<ServiceShoppingCart> ServiceShoppingCart { get; set; }
        public DbSet<ServiceHeader> ServiceHeader { get; set; }
        public DbSet<ServiceDetails> ServiceDetails { get; set; }

        public DbSet<CommonValues> CommonValues {get;set;}
    }
}
