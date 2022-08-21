using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TECH.Service;

namespace TECH.Controllers.Components

{
    [ViewComponent(Name = "ProductsLikeComponent")]
    public class ProductsLikeComponent : ViewComponent
    {
        private readonly IProductsService _productService;
        public ProductsLikeComponent(IProductsService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
        {           
            var productRelate = _productService.GetProductLike(categoryId);
            return View(productRelate);
        }
    }
}