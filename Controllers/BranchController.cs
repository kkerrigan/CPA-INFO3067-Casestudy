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
    public class BranchController : Controller
    {
        AppDbContext _db;
        public BranchController(AppDbContext context)
        {
            _db = context;
        }
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString(SessionVars.Message) != null)
            {
                ViewBag.Message = HttpContext.Session.GetString(SessionVars.Message);
            }
            return View();
        }
        [Route("[action]/{lat:double}/{lng:double}")]
        public IActionResult GetBranches(float lat, float lng)
        {
            BranchModel model = new BranchModel(_db);
            return Ok(model.GetThreeClosestStores(lat, lng));
        }
    }
}