using Clinic.Web.Services.Files;
using CMS.Web.Constant;
using Microsoft.AspNetCore.Http;
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
    public class PremiumSaleController : BaseController
    {
       
        private readonly IFileService _fileServise;
        public PremiumSaleController(ApplicationDbContext DB, IFileService fileServise):base(DB)
        {
          
            _fileServise = fileServise;
        }
        // عرض كل مبيعات الاقساط 
        public IActionResult Index(string SearchKey, int page = 1)
        {
            var numberOFPremiumSales = _DB.PremiumSales.Count(x => !x.IsDelete && (x.CustomerName.Contains(SearchKey) || string.IsNullOrEmpty(SearchKey)));

            var numberOFPages = Math.Ceiling(numberOFPremiumSales / 20.0);
            var skipValue = (page - 1) * 20;
            var takeValue = 20;
            var PremiumSales = _DB.PremiumSales.Include(x => x.Item).Where(x => !x.IsDelete && (x.CustomerName.Contains(SearchKey) || string.IsNullOrEmpty(SearchKey))).Skip(skipValue).Take(takeValue).ToList();

            ViewBag.page = page;
            ViewBag.numberOFPages = numberOFPages;
            ViewBag.SearchKey = SearchKey;
            return View(PremiumSales);
        }

        // انشاء بيع اقساط 
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Item"] = new SelectList( _DB.Items.Where(x => !x.IsDelete),"Id","Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PremiumSale model, IFormFile Customerfile, IFormFile Guarantorfile, IFormFile Attachmentfile)
        {
            var imgCustomerAddress = await _fileServise.SaveFile(Customerfile,"Images");
            var imgGuarantorAddress = await _fileServise.SaveFile(Guarantorfile, "Images");
            var AttachmentAddress = await _fileServise.SaveFile(Guarantorfile, "Attachments");
            
            model.CreatedBy = CurrentUserId;
            model.CustomerIdImageUrl = imgCustomerAddress;
            model.GuarantorIdImageUrl = imgGuarantorAddress;
            model.AttachmentUrl = AttachmentAddress;

            _DB.PremiumSales.Add(model);
            _DB.SaveChanges();
            TempData["msg"] = Messages.CreateAction;
            return RedirectToAction("Index");
        }
        // تعديل على بيع اقساط
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var PremiumSales = _DB.PremiumSales.SingleOrDefault(x => !x.IsDelete && x.Id == id);
            if (PremiumSales == null)
            {
                return NotFound();
            }
            ViewData["Item"] = new SelectList(_DB.Items.Where(x => !x.IsDelete), "Id", "Name");
            return View(PremiumSales);
        }
        [HttpPost]
        public IActionResult Edit(PremiumSale model)
        {
            _DB.PremiumSales.Update(model);
            model.UpdatedBy = CurrentUserId;
            model.UpdatedAt = DateTime.Now;
            _DB.SaveChanges();
            TempData["msg"] = Messages.UpdateAction;
            return RedirectToAction("Index");
        }
        // حذف بيع اقساط
        public IActionResult Delete(int id)
        {
            var PremiumSales = _DB.PremiumSales.SingleOrDefault(x => !x.IsDelete && x.Id == id);
            if (PremiumSales == null)
            {
                return NotFound();
            }
            PremiumSales.IsDelete = true;
            _DB.Update(PremiumSales);
            _DB.SaveChanges();
            TempData["msg"] = Messages.DeleteAction;
            return RedirectToAction("Index");
        }
    }
}
