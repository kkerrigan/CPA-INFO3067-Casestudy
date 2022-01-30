using Microsoft.AspNetCore.Mvc;
using Casestudy.Models;
using Casestudy.Utils;
using Casestudy.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
namespace Casestudy.Controllers
{
    public class BrandController : Controller
    {
        AppDbContext _db;
        public BrandController(AppDbContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            BrandViewModel vm = new BrandViewModel();
            // only build the catalogue once
            if (HttpContext.Session.Get<List<Brand>>(SessionVars.Brands) == null)
            {
                try
                {
                    BrandModel brandModel = new BrandModel(_db);
                    // now load the categories
                    List<Brand> brands = brandModel.GetAll();
                    HttpContext.Session.Set<List<Brand>>(SessionVars.Brands, brands);
                    vm.SetBrands(brands);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Catalogue Problem - " + ex.Message;
                }
            }
            else
            {
                vm.SetBrands(HttpContext.Session.Get<List<Brand>>(SessionVars.Brands));
            }
            return View(vm);
        }
        public IActionResult SelectBrand(BrandViewModel vm)
        {
            BrandModel brandModel = new BrandModel(_db);
            ProductModel productModel = new ProductModel(_db);
            List<Product> products = productModel.GetAllByBrand(vm.BrandId);
            List<ProductViewModel> vms = new List<ProductViewModel>();
            if (products.Count > 0)
            {
                foreach (Product product in products)
                {
                    ProductViewModel mvm = new ProductViewModel();
                    mvm.Qty = 0;
                    mvm.QtyOnHand = product.QtyOnHand;
                    mvm.QtyOnBackOrder = product.QtyOnBackOrder;
                    mvm.BrandId = product.BrandId;
                    mvm.BrandName = brandModel.GetName(product.BrandId);
                    mvm.Description = product.Description;
                    mvm.Id = product.Id;
                    mvm.CostPrice = Convert.ToDecimal(product.CostPrice);
                    mvm.GraphicName = product.GraphicName;
                    mvm.MSRP = Convert.ToDecimal(product.MSRP);
                    mvm.ProductName = product.ProductName;
                    vms.Add(mvm);
                }
                ProductViewModel[] myProducts = vms.ToArray();
                HttpContext.Session.Set<ProductViewModel[]>(SessionVars.Products, myProducts);
            }
            vm.SetBrands(HttpContext.Session.Get<List<Brand>>(SessionVars.Brands));
            return View("Index", vm); // need the original Index View here
        }
        [HttpPost]
        public ActionResult SelectProduct(BrandViewModel vm)
        {
            Dictionary<string, object> cart;
            if (HttpContext.Session.Get<Dictionary<string, Object>>(SessionVars.Cart) == null)
            {
                cart = new Dictionary<string, object>();
            }
            else
            {
                cart = HttpContext.Session.Get<Dictionary<string, object>>(SessionVars.Cart);
            }
            ProductViewModel[] products = HttpContext.Session.Get<ProductViewModel[]>(SessionVars.Products);
            String retMsg = "";
            foreach (ProductViewModel product in products)
            {
                if (product.Id.Equals(vm.Id))
                {
                    if (vm.Qty > 0) // update only selected item
                    {
                        product.Qty = vm.Qty;
                        retMsg = vm.Qty + " - item(s) Added!";
                        cart[product.Id] = product;
                    }
                    else
                    {
                        product.Qty = 0;
                        cart.Remove(product.Id);
                        retMsg = "item(s) Removed!";
                    }
                    vm.BrandId = product.BrandId;
                    break;
                }
            }
            ViewBag.AddMessage = retMsg;
            HttpContext.Session.Set<Dictionary<string, Object>>(SessionVars.Cart, cart);
            vm.SetBrands(HttpContext.Session.Get<List<Brand>>(SessionVars.Brands));
            return PartialView("AddToCartPartial");
        }
    }
}