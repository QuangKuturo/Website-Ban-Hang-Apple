using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Models;
using TECH.Service;

namespace TECH.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductsService productsService,
          ICategoryService categoryService)
        {
            _productsService = productsService;
            _categoryService = categoryService;
        }

        public IActionResult ProductCategory(int categoryId)
        {
            var productViewModelSearch = new ProductViewModelSearch();
            productViewModelSearch.PageIndex = 1;
            productViewModelSearch.PageSize = 8;

            productViewModelSearch.categoryId = categoryId;
            var data = _productsService.GetAllPaging(productViewModelSearch);
            return View(data.Results.ToList());
        }

        public IActionResult ProductDetail(int productId)
        {
            var model = new ProductModelView();
            if (productId > 0)
            {
                model = _productsService.GetByid(productId);
                if (model != null && !string.IsNullOrEmpty(model.name))
                {
                    var category = _categoryService.GetByid(model.category_id.Value);
                    model.categorystr = category.name;
                }
            }
            return View(model);
        }

        public IActionResult Index()
        {
            return View();
        }
       
    }
}