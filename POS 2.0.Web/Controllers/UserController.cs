using POS_2._0.Web.Data;
using POS_2._0.Web.Models;
using POS_2._0.Web.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Web.Constant;
using POS_2._0.Web.ViewModel;
using POS_2._0.Web.DTO;

namespace POS_2._0.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : BaseController
    {
        private readonly UserManager<User> _UserManager;

        public UserController(ApplicationDbContext DB, UserManager<User> UserManager):base(DB)
        {
            _UserManager = UserManager;
        }
        public IActionResult Index(string SearchKey, Gender? GenderType, int page = 1)
        {

            var numberOFUsers = _DB.Users.Count(x => !x.IsDelete && (x.FirstName.Contains(SearchKey) || x.LastName.Contains(SearchKey) || string.IsNullOrEmpty(SearchKey)) && (x.Gender == GenderType || GenderType == null));
            var numberOFPages = Math.Ceiling(numberOFUsers / 20.0);
            var skipValue = (page - 1) * 20;
            var takeValue = 20;
            var users = _DB.Users.Where(x => !x.IsDelete && (x.FirstName.Contains(SearchKey) || x.LastName.Contains(SearchKey) || string.IsNullOrEmpty(SearchKey)) && (x.Gender == GenderType || GenderType == null)).Skip(skipValue).Take(takeValue).ToList();

            var usersVm = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userVm = new UserViewModel();
                userVm.Id = user.Id;
                userVm.FullName = user.FirstName + " " + user.LastName;
                userVm.Email = user.Email;
                userVm.Mobile = user.PhoneNumber;
                userVm.UserType = user.UserType;
                userVm.Gender = user.Gender;

                usersVm.Add(userVm);
            }

            ViewBag.page = page;
            ViewBag.numberOFPages = numberOFPages;
            ViewBag.SearchKey = SearchKey;

            return View(usersVm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            if (_DB.Users.Any(x => x.Email == dto.Email || x.PhoneNumber == dto.Mobile))
            {
                TempData["msg"] = Messages.DublicatedUserName;
                return View(dto);
            }
            var user = new User();
            user.CreatedBy = CurrentUserId;
            user.CreatedAt = DateTime.Now;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.PhoneNumber = dto.Mobile;
            user.Gender = dto.Gender;
            user.UserType = dto.UserType;
            user.UserName = dto.Email;
            
            var result = await _UserManager.CreateAsync(user, "Anas111$$");

            if (result.Succeeded)
            {
                if (user.UserType == Enums.UserType.Admin)
                {
                    await _UserManager.AddToRoleAsync(user, "Admin");
                }
                else if (user.UserType == Enums.UserType.Employee)
                {
                    await _UserManager.AddToRoleAsync(user, "Employee");
                }
                
            }

            //_DB.Users.Add(user);
            //_DB.SaveChanges();
            TempData["msg"] = Messages.CreateAction;
            return RedirectToAction("Index");
        }

        public IActionResult Delete(string Id)
        {
            var user = _DB.Users.SingleOrDefault(x => !x.IsDelete && x.Id ==Id);
            if (user == null)
            {
                return NotFound();
            }
            user.IsDelete = true;
            _DB.Update(user);
            _DB.SaveChanges();
            TempData["msg"] = Messages.DeleteAction;
            return RedirectToAction("Index");

        }
    }
}
