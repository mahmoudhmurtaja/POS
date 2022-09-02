using CMS.Web.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS_2._0.Web.Data;
using POS_2._0.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_2._0.Web.Controllers
{
   
    public class ItemCategoryController : BaseController
    {
       
        public ItemCategoryController(ApplicationDbContext DB):base(DB)
        {
           
        }
        // عرض كل التصنيفات 
        public IActionResult Index(string SearchKey, int page = 1)
        {
            var numberOFCategorys = _DB.ItemCategorys.Count(x => !x.IsDelete && (x.Name.Contains(SearchKey) || string.IsNullOrEmpty(SearchKey)));

            var numberOFPages = Math.Ceiling(numberOFCategorys / 20.0);
            var skipValue = (page - 1) * 20;
            var takeValue = 20;
            var categorys = _DB.ItemCategorys.Where(x => !x.IsDelete && (x.Name.Contains(SearchKey) || string.IsNullOrEmpty(SearchKey))).Skip(skipValue).Take(takeValue).ToList();

            ViewBag.page = page;
            ViewBag.numberOFPages = numberOFPages;
            ViewBag.SearchKey = SearchKey;
            return View(categorys);
        }
        // انشاء تصنيف 
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ItemCategory model)
        {
            _DB.ItemCategorys.Add(model);
            model.CreatedBy = CurrentUserId;
            _DB.SaveChanges();
            TempData["msg"] = Messages.CreateAction;
            return RedirectToAction("Index");
        }
        // تعديل على تصنيف
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _DB.ItemCategorys.SingleOrDefault(x => !x.IsDelete && x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(ItemCategory model)
        {
            _DB.ItemCategorys.Update(model);
            model.UpdatedBy = CurrentUserId;
            model.UpdatedAt = DateTime.Now;
            _DB.SaveChanges();
            TempData["msg"] = Messages.UpdateAction;
            return RedirectToAction("Index");
        }
        // حذف تصنيف
        public IActionResult Delete(int id)
        {
            var category = _DB.ItemCategorys.SingleOrDefault(x => !x.IsDelete && x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            category.IsDelete = true;
            _DB.Update(category);
            _DB.SaveChanges();
            TempData["msg"] = Messages.DeleteAction;
            return RedirectToAction("Index");
        }
    }
}
