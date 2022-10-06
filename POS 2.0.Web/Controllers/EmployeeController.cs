using CMS.Web.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POS_2._0.Web.Data;
using POS_2._0.Web.Enums;
using POS_2._0.Web.Models;
using POS_2._0.Web.Services.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_2._0.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(ApplicationDbContext DB, IEmployeeService employeeService) :base(DB)
        {
            _employeeService = employeeService;
        }
        // عرض كل الموظفين 
        
        public IActionResult Index(string SearchKey, Gender? GenderType, int page = 1)
        {
            var numberOFEmployees = _DB.Employees.Count(x => !x.IsDelete && (x.Name.Contains(SearchKey) || string.IsNullOrEmpty(SearchKey)) && (x.Gender == GenderType || GenderType == null));
            var numberOFPages = Math.Ceiling(numberOFEmployees / 10.0);
            var skipValue = (page - 1) * 10;
            var takeValue = 10;

            var employees = _DB.Employees .Where(x => !x.IsDelete && (x.Name.Contains(SearchKey) || string.IsNullOrEmpty(SearchKey))
                && (x.Gender == GenderType || GenderType == null))
                .Skip(skipValue)
                .Take(takeValue)
                .ToList();

            ViewBag.page = page;
            ViewBag.numberOFPages = numberOFPages;
            ViewBag.SearchKey = SearchKey;

            return View(employees);
        }

        // انشاء موظف 
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee model)
        {
            _employeeService.Create(model);
            TempData["msg"] = Messages.CreateAction;
            return RedirectToAction("Index");
        }
        // تعديل موظف
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Employees = _employeeService.Get(id);
            if (Employees == null)
            {
                return NotFound();
            }
            return View(Employees);
        }
        [HttpPost]
        public IActionResult Edit(Employee model)
        {
            _employeeService.Update(model);
            TempData["msg"] = Messages.UpdateAction;
            return RedirectToAction("Index");
        }
        // حذف موظف
        public IActionResult Delete(int id)
        {
            _employeeService.Delete(id);
            TempData["msg"] = Messages.DeleteAction;
            return RedirectToAction("Index");
        }
    }
}
