using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using POS_2._0.Web.Data;
using POS_2._0.Web.Models;
using POS_2._0.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace POS_2._0.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : BaseController
    {
        
        public HomeController(ApplicationDbContext DB):base(DB)
        {
           
        }
        public IActionResult Index()
        {
            var result = new DashboardViewModel();
            result.UserCount = _DB.Users.Count(x => !x.IsDelete);
            result.EmployeeCount = _DB.Employees.Count(x => !x.IsDelete);
            result.CashSaleCount = _DB.CashSales.Count(x => !x.IsDelete);
            result.PremiumSaleCount = _DB.PremiumSales.Count(x => !x.IsDelete);

            float TotalCapital = _DB.Items.Where(x => !x.IsDelete).Sum(x => x.Price);
            result.TotalCapital = TotalCapital;

            float TotalCashSale = _DB.CashSales.Where(x => !x.IsDelete).Sum(x => x.Price);
            result.TotalCashSale = TotalCashSale;

            float TotalPremiumSale = _DB.PremiumSales.Where(x => !x.IsDelete).Sum(x => x.Price);
            result.TotalPremiumSale = TotalPremiumSale;

            float TotalEmployeesSalary = _DB.Employees.Where(x => !x.IsDelete).Sum(x => x.Salary);
            result.TotalEmployeesSalary = TotalEmployeesSalary;

            float TotalExenses = _DB.PremiumSales.Where(x => !x.IsDelete).Sum(x => x.Price);
            result.TotalExenses = TotalExenses;

            float sumFirstPremiums = _DB.PremiumSales.Where(x => !x.IsDelete).Sum(x => x.FirstPremium);
            float sumPremiums = _DB.Premiums.Where(x => !x.IsDelete).Sum(x => x.PremiumValue);
            float sumPrice = _DB.PremiumSales.Where(x => !x.IsDelete).Sum(x => x.Price);
            float TotalDebts = sumPrice - (sumFirstPremiums + sumPremiums);
            result.TotalDebts = TotalDebts;

            float Profet = TotalCapital - (TotalCashSale + TotalPremiumSale);
            result.Profet = (TotalCashSale + TotalPremiumSale) - (TotalCapital + TotalEmployeesSalary + TotalExenses); 

            return View(result);
        }

        
    }
}
