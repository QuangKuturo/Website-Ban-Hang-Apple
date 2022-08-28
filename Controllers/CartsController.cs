﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly IOrdersService _ordersService;
        private readonly IProductsService _productsService;
        public IHttpContextAccessor _httpContextAccessor;
        public CartsController(ICartsService cartsService,
            IHttpContextAccessor httpContextAccessor,
            IOrdersService ordersService,
            IProductsService productsService)
        {
            _cartsService = cartsService;
            _ordersService = ordersService;
            _httpContextAccessor = httpContextAccessor;
            _productsService = productsService;
        }

        [HttpPost]
        public JsonResult AddOrder(OrdersModelView OrdersModelView)
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserModelView();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserModelView>(userString);
                OrdersModelView.user_id = user.id;
                OrdersModelView.code = "MDH00" + user.id.ToString() + DateTime.Now.Second.ToString();
                var result = _ordersService.AddOrder(OrdersModelView);             

                if (result > 0)
                {
                    var model = _cartsService.GetAllCart(user.id);
                    if (model != null && model.Count > 0)
                    {
                        foreach (var item in model)
                        {
                            var ordersDetailModelView = new OrdersDetailModelView();
                            ordersDetailModelView.order_id = result;
                            ordersDetailModelView.product_id = item.product_id;
                            ordersDetailModelView.color = item.color;
                            ordersDetailModelView.price = item.price;
                            ordersDetailModelView.quantity = item.quantity;
                            _ordersService.AddOrderDetail(ordersDetailModelView);
                            _cartsService.Deleted(item.id);
                            //_ordersService.Save();
                        }
                        return Json(new
                        {
                            success = true
                        });
                    }
                }

            }

            return Json(new
            {
                success = false
            }); 
        }

        public IActionResult HistoryOrder()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserModelView();
            var model = new List<OrdersModelView>();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (user != null)
                {
                    var data = _ordersService.GetOrderForUserId(user.id);
                    if (data != null && data.Count > 0)
                    {
                        foreach (var item in data)
                        {
                            if (item != null && item.user_id.HasValue)
                            {
                                if (item.payment == 1)
                                {
                                    item.paymentstr = "Ship Cod";
                                }
                                else if (item.payment == 2)
                                {
                                    item.paymentstr = "VnPay";
                                }
                                else if (item.payment == 0)
                                {
                                    item.paymentstr = "Mua trực tiếp";
                                }
                            }
                            if (item.status == 0)
                            {
                                item.statusstr = "Đang chờ xử lý";
                            }
                            else if (item.status == 1)
                            {
                                item.statusstr = "Đã hoàn thành";
                            }
                            else if (item.status == 2)
                            {
                                item.statusstr = "Đã hủy";
                            }

                        }
                        model = data;
                    }
                    return View(model);
                }
            }
            return Redirect("/home");
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
                return View(carts);
            }
            return Redirect("/home");

        }

        public IActionResult OrderPay()
        {
            var ordersCartDetailModelView = new OrdersCartDetailModelView();
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserModelView();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (user != null)
                {
                    user.address = !string.IsNullOrEmpty(user.address) && user.address !="null" ? user.address : "";
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
            if (ordersCartDetailModelView != null)
            {
                return View(ordersCartDetailModelView);
            }
            return Redirect("/home");
            
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