using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TECH.Service;

namespace TECH.Controllers.Components

{
    [ViewComponent(Name = "RelatedProductsComponent")]
    public class RelatedProductsComponent : ViewComponent
    {
        private readonly IProductsService _productService;
        public RelatedProductsComponent(IProductsService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int categoryId,int productId)
        {           
            var productRelate = _productService.GetProductReLated(categoryId, productId);
            return View(productRelate);
        }
    }
}