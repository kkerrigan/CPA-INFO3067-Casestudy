using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using Casestudy.Utils;
using System.Collections.Generic;
using Casestudy.Models;
using Casestudy.ViewModels;
using Newtonsoft.Json;

namespace Casestudy.Controllers
{
    public class CartController : Controller
    {
        AppDbContext _db;
        public CartController(AppDbContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult AddOrder()
        {
            OrderModel model = new OrderModel(_db);
            int retVal = -1;
            string backOrderMessage = "";
            string retMessage = "";
            try
            {
                Dictionary<string, object> cartItems = HttpContext.Session.Get<Dictionary<string, object>>(SessionVars.Cart);
                retVal = model.AddOrder(cartItems, HttpContext.Session.GetString(SessionVars.User), ref backOrderMessage);
                if (retVal > 0) // Tray Added
                {
                    retMessage = "Order " + retVal + " Created!" + "\n" + backOrderMessage;
                }
                else // problem
                {
                    retMessage = "Order not added, try again later";
                }
            }
            catch (Exception ex) // big problem
            {
                retMessage = "Order was not created, try again later! - " + ex.Message;
            }
            HttpContext.Session.Remove(SessionVars.Cart); // clear out current tray once persisted
            HttpContext.Session.SetString(SessionVars.Message, retMessage);
            return Redirect("/Home");
        }
        public ActionResult ClearCart() // clear out current tray
        {
            HttpContext.Session.Remove(SessionVars.Cart);
            HttpContext.Session.Set<String>(SessionVars.Message, "Cart Cleared");
            return Redirect("/Home");
        }
    }
}