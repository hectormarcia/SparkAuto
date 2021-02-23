using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SparkAuto.Models
{
    public class ServiceHeader
    {
        public int Id { get; set; }
        public double Miles { get; set; }
        [Required]
        
        public double TotalPrice { get; set; }

        public string Details { get; set; }

        [Required]
        public double Tax { get; set; }

        public double EnvCharge {get;set;}

        [Required]
        public double FullPrice { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}"), Display(Name ="Service Date")]
        public DateTime DateAdded { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}"), Display(Name ="Next Service Date")]
        public DateTime NextServiceDate { get; set; }

        public int CarId { get; set; }

        [ForeignKey("CarId")]
        public virtual Car Car { get; set; }
    }
}
