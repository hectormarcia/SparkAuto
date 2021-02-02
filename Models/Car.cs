using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SparkAuto.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Plate Number")]
        public string VIN { get; set; }
       [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }        
        public string Style { get; set; }
        [Required]
        public string Year { get; set; }

        [Display(Name = "Odometer")]
        public double Miles { get; set; }

        public string Color { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
