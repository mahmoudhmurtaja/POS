using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_2._0.Web.ViewModel
{
    public class DashboardViewModel
    {
        public int UserCount { get; set; }
        public int EmployeeCount { get; set; }
        public int CashSaleCount { get; set; }
        public int PremiumSaleCount { get; set; }
        public float TotalCapital { get; set; }
        public float TotalCashSale { get; set; }
        public float TotalPremiumSale { get; set; }
        public float TotalDebts { get; set; }
        public float TotalEmployeesSalary { get; set; }
        public float TotalExenses { get; set; }
        public float Profet { get; set; }

    }
}
