using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace POS_2._0.Web.Models
{
    // بيع لاقساط
    public class PremiumSale : BaseEntity
    {
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public float Price { get; set; }// السعر
        public float FirstPremium { get; set; }// الدفعة الاولى
        public DateTime PremiumAt { get; set; }// وقت كل قسط
        public float AgreedPremium { get; set; } // القسط المتفق عليه
        public List<Premium> Premiums { get; set; } // الاقساط المدفوعة
        [Required]
        public string CustomerName { get; set; }// اسم الزبون
        [Required]
        public string CustomerIdNo { get; set; }// رقم هوية الزبون
        [Required]
        public string CustomerModile { get; set; }//رقم جوال الزبون
        public string CustomerEmail { get; set; }// ايميل الزبون
        [Required]
        public string CustomerAdress { get; set; }// عنوان سكن الزبون
        public string CustomerIdImageUrl { get; set; }//صورة هوية الزبون
        
        //بيانات الكفيل 
        public string GuarantorName { get; set; }
        public string GuarantorIdNo { get; set; }
        public string GuarantorModile { get; set; }
        public string GuarantorEmail { get; set; }
        public string GuarantorAdress { get; set; }
        public string GuarantorIdImageUrl { get; set; }

        public string AttachmentUrl { get; set; } //رابط مرفق الكمبيالة

        public PremiumSale()
        {
            PremiumAt = DateTime.Now;
        }
    }
}
