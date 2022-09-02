using POS_2._0.Web.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace POS_2._0.Web.Models
{
    //الموظفين
    public class Employee : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Adress { get; set; }
        public float Salary { get; set; }
        public Gender Gender { get; set; }
    }
}
