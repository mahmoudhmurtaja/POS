using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace POS_2._0.Web.Models
{
    //المصورفات
    public class Expense : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public float Price { get; set; }
    }
}
