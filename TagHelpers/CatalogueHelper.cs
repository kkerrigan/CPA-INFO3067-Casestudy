using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

//added usings
using Microsoft.AspNetCore.Http;
using System.Text;
using Casestudy.ViewModels;
using Casestudy.Utils;
using Newtonsoft.Json;

namespace Casestudy.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("catalogue", Attributes = BrandIdAttribute)]
    public class CatalogueHelper : TagHelper
    {
        private const string BrandIdAttribute = "brand";
        [HtmlAttributeName(BrandIdAttribute)]
        public string BrandId { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public CatalogueHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (_session.Get<ProductViewModel[]>(SessionVars.Products) != null && Convert.ToInt32(BrandId) > 0)
            {
                var innerHtml = new StringBuilder();
                ProductViewModel[] products = _session.Get<ProductViewModel[]>(SessionVars.Products);
                innerHtml.Append("<div class=\"col-xs-12\" style=\"font-size:x-large;\"><span>Catalogue</span></div>");
                foreach (ProductViewModel product in products)
                {
                    if (product.BrandId == Convert.ToInt32(BrandId))
                    {
                        product.JsonData = JsonConvert.SerializeObject(product);
                        innerHtml.Append("<div class=\"col-sm-3 col-xs-12 text-center\" style=\"border:solid; background-color:lightskyblue;\">");
                        innerHtml.Append("<span class=\"col-xs-12\"><img src=\"/img/" + product.GraphicName + "\" style=\"height: 100px; width: 100px\"/></ span > ");
                        innerHtml.Append("<p><span style=\"font-size:large;\">" + product.ProductName.Substring(0,7) + "...</span></p><div>");
                        innerHtml.Append("<span>For More Info.<br />Click Details</span></div>");
                        innerHtml.Append("<div style=\"padding-bottom: 10px;\"><a href=\"#details_popup\" data-toggle=\"modal\" class=\"btn btn-default\"");
                        innerHtml.Append(" id=\"modalbtn" + product.Id + "\" data-id=\"" + product.Id + "\" data-details='" + product.JsonData + "'>Details</a></div></div>");
                    }
                }
                output.Content.SetHtmlContent(innerHtml.ToString());
            }
        }
    }
}
