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
    public class ExpenseController : BaseController
    {
       
        public ExpenseController(ApplicationDbContext DB):base(DB)
        {
           
        }
        // عرض كل المصروفات 
        public IActionResult Index(string SearchKey, int page = 1)
        {
            var numberOFExpenses = _DB.Expenses.Count(x => !x.IsDelete && (x.Name.Contains(SearchKey) || string.IsNullOrEmpty(SearchKey)));

            var numberOFPages = Math.Ceiling(numberOFExpenses / 20.0);
            var skipValue = (page - 1) * 20;
            var takeValue = 20;
            var Expenses = _DB.Expenses.Where(x => !x.IsDelete && (x.Name.Contains(SearchKey) || string.IsNullOrEmpty(SearchKey))).Skip(skipValue).Take(takeValue).ToList();

            ViewBag.page = page;
            ViewBag.numberOFPages = numberOFPages;
            ViewBag.SearchKey = SearchKey;
            return View(Expenses);
        }

        // انشاء مصروف 
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Expense model)
        {
            _DB.Expenses.Add(model);
            model.CreatedBy = CurrentUserId;
            _DB.SaveChanges();
            TempData["msg"] = Messages.CreateAction;
            return RedirectToAction("Index");
        }
        // تعديل مصروف
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Expenses = _DB.Expenses.SingleOrDefault(x => !x.IsDelete && x.Id == id);
            if (Expenses == null)
            {
                return NotFound();
            }
            return View(Expenses);
        }
        [HttpPost]
        public IActionResult Edit(Expense model)
        {
            _DB.Expenses.Update(model);
            model.UpdatedBy = CurrentUserId;
            model.UpdatedAt = DateTime.Now;
            _DB.SaveChanges();
            TempData["msg"] = Messages.UpdateAction;
            return RedirectToAction("Index");
        }
        // حذف مصروف
        public IActionResult Delete(int id)
        {
            var Expenses = _DB.Expenses.SingleOrDefault(x => !x.IsDelete && x.Id == id);
            if (Expenses == null)
            {
                return NotFound();
            }
            Expenses.IsDelete = true;
            _DB.Update(Expenses);
            _DB.SaveChanges();
            TempData["msg"] = Messages.DeleteAction;
            return RedirectToAction("Index");
        }
    }
}
