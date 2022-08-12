using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;
using System.Text.RegularExpressions;

namespace TECH.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IOrdersService _ordersService;
        private readonly IAppUserService _appUserService;
        public OrdersController(IOrdersService ordersService,
            IAppUserService appUserService)
        {
            _ordersService = ordersService;
            _appUserService = appUserService;        }
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public IActionResult AddView()
        //{
        //    return View();
        //}

        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new OrdersModelView();
            if (id > 0)
            {
                model = _ordersService.GetByid(id);

               
                if (model != null && model.user_id.HasValue)
                {
                    var appuser = _appUserService.GetByid(model.user_id.Value);
                    model.customerStr = appuser.full_name;
                }

            }
            return Json(new
            {
                Data = model
            });
        }

        //[HttpGet]
        //public IActionResult AddOrUpdate()
        //{
        //    return View();
        //}

       
        [HttpPost]
        public JsonResult Add(OrdersModelView OrdersModelView)
        {
            var result = _ordersService.Add(OrdersModelView);
            _ordersService.Save();
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
               var  model = _ordersService.UpdateStatus(id, status);
                _ordersService.Save();
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
        public JsonResult Update(OrdersModelView OrdersModelView)
        {

           
            var result = _ordersService.Update(OrdersModelView);
            _ordersService.Save();
            return Json(new
            {
                success = result
            });

        }

      

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _ordersService.Deleted(id);
            _ordersService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(OrdersViewModelSearch ordersViewModelSearch)
        {
            var data = _ordersService.GetAllPaging(ordersViewModelSearch);
            foreach (var item in data.Results)
            {
                if (item != null && item.user_id.HasValue)
                {
                    var appuser = _appUserService.GetByid(item.user_id.Value);
                    item.customerStr = appuser.full_name;
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

            }
            return Json(new { data = data });
        }
    }
}
