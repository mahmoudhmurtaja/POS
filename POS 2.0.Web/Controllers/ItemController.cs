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
    public class ItemController : BaseController
    {
       
        public ItemController(ApplicationDbContext DB):base(DB)
        {
           
        }
        // عرض كل الاصناف 
        public IActionResult Index(string SearchKey, int page = 1)
        {
            var numberOFItems = _DB.Items.Count(x => !x.IsDelete && (x.Name.Contains(SearchKey) || string.IsNullOrEmpty(SearchKey)));

            var numberOFPages = Math.Ceiling(numberOFItems / 10.0);
            var skipValue = (page - 1) * 10;
            var takeValue = 10;
            var Items = _DB.Items.Include(x => x.ItemCategory)
                .Where(x => !x.IsDelete && (x.Name.Contains(SearchKey) || string.IsNullOrEmpty(SearchKey)))
                .Skip(skipValue)
                .Take(takeValue)
                .ToList();

            ViewBag.page = page;
            ViewBag.numberOFPages = numberOFPages;
            ViewBag.SearchKey = SearchKey;
            return View(Items);
        }

        // انشاء صنف 
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ItemCategory"] = new SelectList( _DB.ItemCategorys.Where(x => !x.IsDelete),"Id","Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Item model)
        {
            _DB.Items.Add(model);
            model.CreatedBy = CurrentUserId;
            _DB.SaveChanges();
            TempData["msg"] = Messages.CreateAction;
            return RedirectToAction("Index");
        }
        // تعديل على صنف
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Items = _DB.Items.SingleOrDefault(x => !x.IsDelete && x.Id == id);
            if (Items == null)
            {
                return NotFound();
            }
            ViewData["ItemCategory"] = new SelectList(_DB.ItemCategorys.Where(x => !x.IsDelete), "Id", "Name");
            return View(Items);
        }
        [HttpPost]
        public IActionResult Edit(Item model)
        {
            _DB.Items.Update(model);
            model.UpdatedBy = CurrentUserId;
            model.UpdatedAt = DateTime.Now;
            _DB.SaveChanges();
            TempData["msg"] = Messages.UpdateAction;
            return RedirectToAction("Index");
        }
        // حذف صنف
        public IActionResult Delete(int id)
        {
            var Items = _DB.Items.SingleOrDefault(x => !x.IsDelete && x.Id == id);
            if (Items == null)
            {
                return NotFound();
            }
            Items.IsDelete = true;
            _DB.Update(Items);
            _DB.SaveChanges();
            TempData["msg"] = Messages.DeleteAction;
            return RedirectToAction("Index");
        }
    }
}
