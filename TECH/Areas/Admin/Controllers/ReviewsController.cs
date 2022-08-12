using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace TECH.Areas.Admin.Controllers
{
    public class ReviewsController : BaseController
    {
        private readonly IReviewsService _reviewsService;
        private readonly IProductsService _productService;
        public ReviewsController(
            IReviewsService reviewsService,
            IProductsService productService)
        {
            _reviewsService = reviewsService;
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddView()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new ReviewsModelView();
            if (id > 0)
            {
                model = _reviewsService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }

        [HttpGet]
        public IActionResult AddOrUpdate()
        {
            return View();
        }
       
        [HttpPost]
        public JsonResult Add(ReviewsModelView ReviewsModelView)
        {
            var result = _reviewsService.Add(ReviewsModelView);
            _reviewsService.Save();
            return Json(new
            {
                success = result
            });

        }

        [HttpGet]
        public JsonResult UpdateStatus(int id,int status)
        {
            if (id > 0)
            {
               var  model = _reviewsService.UpdateStatus(id, status);
                _reviewsService.Save();
                return Json(new
                {
                    success = model
                });
            }
            return Json(new
            {
                success = false
            });

        }

        [HttpPost]
        public JsonResult Update(ReviewsModelView ReviewsModelView)
        {
            var result = _reviewsService.Update(ReviewsModelView);
            _reviewsService.Save();
            return Json(new
            {
                success = result
            });

        }        

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _reviewsService.Deleted(id);
            _reviewsService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(ReviewsViewModelSearch reviewsViewModelSearch)
        {
            var data = _reviewsService.GetAllPaging(reviewsViewModelSearch);
            foreach (var item in data.Results)
            {
                if (item.author.HasValue)
                {
                    var  appUser = _appUserService.GetByid(item.author.Value);
                    if (appUser != null && !string.IsNullOrEmpty(appUser.full_name))
                    {
                        item.author_name = appUser.full_name;
                    }                    
                }

            }
            return Json(new { data = data });
        }
    }
}
