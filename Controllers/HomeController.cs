using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Casestudy.Utils;
using Microsoft.AspNetCore.Authorization;

namespace Casestudy.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(SessionVars.LoginStatus) == null)
            {
                HttpContext.Session.SetString(SessionVars.LoginStatus, "Not Logged in");
            }
            if (HttpContext.Session.GetString(SessionVars.LoginStatus) == "Not Logged in")
            {
                HttpContext.Session.SetString(SessionVars.Message, "Most functionality requires you to Login!");
            }
            ViewBag.Status = HttpContext.Session.GetString(SessionVars.LoginStatus);
            ViewBag.Message = HttpContext.Session.GetString(SessionVars.Message);
            return View();
        }
    }
}