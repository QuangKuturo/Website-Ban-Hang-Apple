using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Controllers.Components

{
    //[ViewComponent(Name = "CategoryMenuComponent")]
    public class ProductAllComponent : ViewComponent
    {
        private readonly IProductsService _productService;
        public ProductAllComponent(IProductsService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var colorViewModelSearch = new ProductViewModelSearch();
            colorViewModelSearch.PageIndex = 1;
            colorViewModelSearch.PageSize = 8;
            var categoryModel = _productService.GetAllPaging(colorViewModelSearch);
            var model = new List<ProductModelView>();
            if (categoryModel.Results != null && categoryModel.Results.Count > 0)
            {
                model = categoryModel.Results.ToList();
            }
            return View(model);
        }
    }
}