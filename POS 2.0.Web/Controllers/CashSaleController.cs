using Clinic.Web.Services.Emails;
using CMS.Web.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POS_2._0.Web.Data;
using POS_2._0.Web.Enums;
using POS_2._0.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_2._0.Web.Controllers
{
    public class CashSaleController : BaseController
    {
        private readonly IEmailSender _emailSender;

        public CashSaleController(ApplicationDbContext DB, IEmailSender emailSender) :base(DB)
        {
            _emailSender = emailSender;
        }
        // عرض كل مبيعات الكاش 
        public IActionResult Index(string SearchKey, int page = 1)
        {
            var numberOFCashSales = _DB.CashSales.Count(x => !x.IsDelete && (x.Item.Name.Contains(SearchKey) || string.IsNullOrEmpty(SearchKey)));
            
            var numberOFPages = Math.Ceiling(numberOFCashSales / 10.0);
            var skipValue = (page - 1) * 10;
            var takeValue = 10;
            var CashSales = _DB.CashSales.Include(x => x.Item)
                .Where(x => !x.IsDelete && (x.Item.Name.Contains(SearchKey) || string.IsNullOrEmpty(SearchKey)))
                .Skip(skipValue)
                .Take(takeValue)
                .ToList();
            
            ViewBag.page = page;
            ViewBag.numberOFPages = numberOFPages;
            ViewBag.SearchKey = SearchKey;
            return View(CashSales);
        }

        // انشاء بيع كاش 
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Item"] = new SelectList(_DB.Items.Where(x => !x.IsDelete),"Id","Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CashSale model)
        {
            _DB.CashSales.Add(model);
            model.CreatedBy = CurrentUserId;

            await _emailSender.Send("tgsdk13@gmail.com", "Hi", "Cash Sale now");

            _DB.SaveChanges();
            TempData["msg"] = Messages.CreateAction;
            return RedirectToAction("Index");
        }
        // تعديل بيع كاش
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var CashSales = _DB.CashSales.SingleOrDefault(x => !x.IsDelete && x.Id == id);
            if (CashSales == null)
            {
                return NotFound();
            }
            ViewData["Item"] = new SelectList(_DB.Items.Where(x => !x.IsDelete), "Id", "Name");
            return View(CashSales);
        }
        [HttpPost]
        public IActionResult Edit(CashSale model)
        {
            _DB.CashSales.Update(model);
            model.UpdatedBy = CurrentUserId;
            model.UpdatedAt = DateTime.Now;
            _DB.SaveChanges();
            TempData["msg"] = Messages.UpdateAction;
            return RedirectToAction("Index");
        }
        // حذف بيع كاش
        public IActionResult Delete(int id)
        {
            var CashSales = _DB.CashSales.SingleOrDefault(x => !x.IsDelete && x.Id == id);
            if (CashSales == null)
            {
                return NotFound();
            }
            CashSales.IsDelete = true;
            _DB.Update(CashSales);
            _DB.SaveChanges();
            TempData["msg"] = Messages.DeleteAction;
            return RedirectToAction("Index");
        }
    }
}
