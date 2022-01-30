using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Casestudy.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Casestudy.ViewModels
{
    public class BrandViewModel
    {
        private List<Brand> _brands;
        [Required]
        public int Qty { get; set; }
        public string Id { get; set; }
        public string BrandName { get; set; }
        public int BrandId { get; set; }
        public IEnumerable<SelectListItem> GetBrands()
        {
            return _brands.Select(brand => new SelectListItem
            {
                Text = brand.Name,
                Value = Convert.ToString(brand.Id)
            });
        }
        public void SetBrands(List<Brand> brnds)
        {
            _brands = brnds;
        }
    }
}