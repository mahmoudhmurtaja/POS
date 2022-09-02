using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_2._0.Web.Models
{
    //بيع الكاش
    public class CashSale : BaseEntity
    {
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public float Price { get; set; }//سعر البيع
        public int Quantity { get; set; }// الكمية
    }
}
