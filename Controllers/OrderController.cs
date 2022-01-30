using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Casestudy.Models;
using Microsoft.AspNetCore.Http;
using Casestudy.Utils;

namespace Casestudy.Controllers
{
    public class OrderController : Controller
    {
        AppDbContext _db;
        public OrderController(AppDbContext context)
        {
            _db = context;
        }
        [Route("[action]")]
        public IActionResult GetOrders()
        {
            string user = HttpContext.Session.GetString(SessionVars.User);
            OrderModel model = new OrderModel(_db);
            return Ok(model.GetOrders(user));
        }

        [Route("[action]/{oid:int}")]
        public IActionResult GetOrderDetails(int oid)
        {
            OrderModel model = new OrderModel(_db);
            return Ok(model.GetOrderDetails(oid, HttpContext.Session.GetString(SessionVars.User)));
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}