using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Casestudy.Models
{
    public class ProductModel
    {
        private AppDbContext _db;
        
        public ProductModel(AppDbContext context)
        {
            _db = context;
        }
        
        public List<Product> GetAll()
        {
            return _db.Products.ToList();
        }
        public List<Product> GetAllByBrand(int id)
        {
            return _db.Products.Where(item => item.BrandId == id).ToList();
        }

        public List<Product> GetAllByBrandName(string brdname)
        {
            Brand brand = _db.Brands.First(brd => brd.Name == brdname);
            return _db.Products.Where(item => item.BrandId == brand.Id).ToList();
        }
    }
}
