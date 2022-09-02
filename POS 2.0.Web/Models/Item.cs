using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace POS_2._0.Web.Models
{
    //المشتريات
    public class Item : BaseEntity
    {
        [Required]
        public string Name { get; set; }//اسم الصنف
        public float Price { get; set; }//السعر
        public int Quantity { get; set; }//الكمية
        public string MerchantName { get; set; }//اسم التاجر
        public string MerchantMobile { get; set; }// رقم جوال التاجر
        public int ItemCategoryId { get; set; }
        public ItemCategory ItemCategory { get; set; }

    }
}
