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
        private readonly IOrdersService _ordersService;
        private readonly IAppUserService _appUserService;
        public ReviewsController(
            IReviewsService reviewsService,
            IProductsService productService,
            IOrdersService ordersService,  
            IAppUserService appUserService
            )
        {
            _reviewsService = reviewsService;
            _productService = productService;
            _appUserService = appUserService;
            _ordersService = ordersService;
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
                if (model.order_id.HasValue && model.order_id.Value > 0)
                {
                    var dataOrders = _ordersService.GetByid(model.order_id.Value);
                    if (dataOrders != null)
                    {
                        model.ordersModelView = dataOrders;

                        if (dataOrders.id > 0 && dataOrders.user_id.HasValue && dataOrders.user_id.Value > 0)
                        {
                            var dataUser = _appUserService.GetByid(dataOrders.user_id.Value);
                            if (dataUser != null)
                            {
                                model.userModelView = dataUser;
                            }
                        }
                    }
                }
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
                if (item.order_id.HasValue && item.order_id.Value > 0)
                {
                    var dataOrders = _ordersService.GetByid(item.order_id.Value);
                    if (dataOrders != null)
                    {
                        item.ordersModelView = dataOrders;

                        if (dataOrders.id > 0 && dataOrders.user_id.HasValue && dataOrders.user_id.Value > 0)
                        {
                            var dataUser = _appUserService.GetByid(dataOrders.user_id.Value);
                            if (dataUser != null)
                            {
                                item.userModelView = dataUser;
                            }
                        }
                    }
                }

            }
            return Json(new { data = data });
        }
    }
}
