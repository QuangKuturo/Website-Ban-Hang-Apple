using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Models;
using TECH.Service;

namespace TECH.Controllers
{
    public class CartsController : Controller
    {
        private readonly ICartsService _cartsService;
        private readonly IProductsService _productsService;
        public IHttpContextAccessor _httpContextAccessor;
        public CartsController(ICartsService cartsService,
            IHttpContextAccessor httpContextAccessor,
            IProductsService productsService)
        {
            _cartsService = cartsService;
            _httpContextAccessor = httpContextAccessor;
            _productsService = productsService;
        }

        public IActionResult Index()
        {
            var carts = new List<CartsModelView>();
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserModelView();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (user != null)
                {
                    var model = _cartsService.GetAllCart(user.id);
                    if (model != null && model.Count > 0)
                    {
                        foreach (var item in model)
                        {
                            if (item.product_id.HasValue && item.product_id.Value > 0)
                            {
                                var _product = _productsService.GetByid(item.product_id.Value);
                                if (_product != null)
                                {
                                    item.productModelView = _product;
                                }
                            }
                        }
                        carts = model;
                    }
                }
            }
           
            return View(carts);
        }

        public IActionResult DeletailOrder()
        {
            var ordersCartDetailModelView = new OrdersCartDetailModelView();
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserModelView();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (user != null)
                {
                    ordersCartDetailModelView.UserModelView = user;
                    var model = _cartsService.GetAllCart(user.id);
                    if (model != null && model.Count > 0)
                    {
                        foreach (var item in model)
                        {
                            if (item.product_id.HasValue && item.product_id.Value > 0)
                            {
                                var _product = _productsService.GetByid(item.product_id.Value);
                                if (_product != null)
                                {
                                    item.productModelView = _product;
                                }
                            }
                        }
                        ordersCartDetailModelView.CartsModelView = model;
                    }
                }
            }

            return View(ordersCartDetailModelView);
        }



        [HttpPost]
        public JsonResult Add(CartsModelView cartsModelView)
        {

            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserModelView();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (user != null)
                {
                    cartsModelView.user_id = user.id;
                    //var product = _productsService.GetByid(cartsModelView.product_id.Value);

                    _cartsService.Add(cartsModelView);
                    _cartsService.Save();
                    return Json(new
                    {
                        success = true
                    });
                }
            }
         
            return Json(new
            {
                success = false
            });
        }

        [HttpPost]
        public JsonResult Update(CartsModelView cartsModelView)
        {

            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserModelView();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (user != null)
                {
                    cartsModelView.user_id = user.id;
                    var result = _cartsService.Update(cartsModelView);
                    _cartsService.Save();
                    return Json(new
                    {
                        success = result
                    });
                }
            }
          
            return Json(new
            {
                success = false
            });
        }

        [HttpPost]
        public JsonResult Deleted(int id)
        {

            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserModelView();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (user != null)
                {
                    var result = _cartsService.Deleted(id);
                    _cartsService.Save();
                    return Json(new
                    {
                        success = result
                    });
                }
            }
       
            return Json(new
            {
                success = false
            });
        }



       
    }
}