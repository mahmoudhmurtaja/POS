using CMS.Web.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POS_2._0.Web.Data;
using POS_2._0.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_2._0.Web.Controllers
{
    public class PremiumController : BaseController
    {
        
        public PremiumController(ApplicationDbContext DB):base(DB)
        {
           
        }
        // عرض كل الدفع 
        public IActionResult Index(string SearchKey, int page = 1)
        {
            var numberOFPremiums = _DB.Premiums.Count(x => !x.IsDelete && (x.PremiumSale.CustomerName.Contains(SearchKey) || string.IsNullOrEmpty(SearchKey)));

            var numberOFPages = Math.Ceiling(numberOFPremiums / 20.0);
            var skipValue = (page - 1) * 20;
            var takeValue = 20;
            var Premiums = _DB.Premiums.Include(x => x.PremiumSale).Where(x => !x.IsDelete && (x.PremiumSale.CustomerName.Contains(SearchKey) || string.IsNullOrEmpty(SearchKey))).Skip(skipValue).Take(takeValue).ToList();

            ViewBag.page = page;
            ViewBag.numberOFPages = numberOFPages;
            ViewBag.SearchKey = SearchKey;
            return View(Premiums);
        }

        // انشاء دفعة 
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["PremiumSale"] = new SelectList( _DB.PremiumSales.Where(x => !x.IsDelete),"Id", "CustomerName");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Premium model)
        {
            _DB.Premiums.Add(model);
            model.CreatedBy = CurrentUserId;
            _DB.SaveChanges();
            TempData["msg"] = Messages.CreateAction;
            return RedirectToAction("Index");
        }
        // تعديل على دفعة
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Premiums = _DB.Premiums.SingleOrDefault(x => !x.IsDelete && x.Id == id);
            if (Premiums == null)
            {
                return NotFound();
            }
            ViewData["PremiumSale"] = new SelectList(_DB.PremiumSales.Where(x => !x.IsDelete), "Id", "CustomerName");
            return View(Premiums);
        }
        [HttpPost]
        public IActionResult Edit(Premium model)
        {
            _DB.Premiums.Update(model);
            model.UpdatedBy = CurrentUserId;
            model.UpdatedAt = DateTime.Now;
            _DB.SaveChanges();
            TempData["msg"] = Messages.UpdateAction;
            return RedirectToAction("Index");
        }
        // حذف دفعة
        public IActionResult Delete(int id)
        {
            var Premiums = _DB.Premiums.SingleOrDefault(x => !x.IsDelete && x.Id == id);
            if (Premiums == null)
            {
                return NotFound();
            }
            Premiums.IsDelete = true;
            _DB.Update(Premiums);
            _DB.SaveChanges();
            TempData["msg"] = Messages.DeleteAction;
            return RedirectToAction("Index");
        }
    }
}
