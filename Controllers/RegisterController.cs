using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Casestudy.ViewModels;
using Casestudy.Models;
using Casestudy.Utils;
namespace Casestudy.Controllers
{
    public class RegisterController : Controller
    {
        UserManager<ApplicationUser> _usrMgr;
        SignInManager<ApplicationUser> _signInMgr;
        public RegisterController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
        {
            _usrMgr = userManager;
            _signInMgr = signInManager;
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        //
        // POST:/Register/Register
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Age = model.Age,
                    Address1 = model.Address1,
                    City = model.City,
                    Mailcode = model.Mailcode,
                    Country = model.Country,
                    CreditcardType = model.CreditcardType,
                    Region = model.Region
                };
                var result = await _usrMgr.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInMgr.SignInAsync(user, isPersistent: false);
                    HttpContext.Session.SetString(SessionVars.LoginStatus, "Logged on as " + model.Email);
                    HttpContext.Session.SetString(SessionVars.Message, "Registered, Logged on as " + model.Email);
                }
                else
                {
                    ViewBag.message = "Registration failed - " + result.Errors.First().Description;
                    return View("Index");
                }
            }
            return Redirect("/Home");
        }
    }
}