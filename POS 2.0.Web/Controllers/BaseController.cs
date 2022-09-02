using Clinic.Web.Services.Emails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using POS_2._0.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_2._0.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext _DB;
        
        public BaseController(ApplicationDbContext DB)
        {
            _DB = DB;
        }
        public string UserName { get; set; }
        protected string CurrentUserId;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            UserName = User.Identity.Name;
            var user = _DB.Users.SingleOrDefault(x => x.UserName == UserName);
            CurrentUserId = user.Id;
            TempData["UserType"] = user.UserType;
        }

    }
}
