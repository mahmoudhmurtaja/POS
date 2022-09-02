using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_2._0.Web.Models
{
    //الاقساط
    public class Premium : BaseEntity
    {
        public int PremiumSaleId { get; set; }
        public PremiumSale PremiumSale { get; set; } // اسم الزبون
        public float PremiumValue { get; set; } // قيمة السط
    }
}
