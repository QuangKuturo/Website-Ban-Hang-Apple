﻿using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class FeeController : BaseController
    {
        private readonly IFeeService _feeService;
        private readonly ICityService _cityService;
        private readonly IDistrictsService _districtsService;
        private readonly IWardsService _wardsService;
        public FeeController(IFeeService feeService,
            ICityService cityService,
            IDistrictsService districtsService,
            IWardsService wardsService)
        {
            _feeService = feeService;
            _cityService = cityService;
            _districtsService = districtsService;
            _wardsService = wardsService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new FeeModelView();
            if (id > 0)
            {
                model = _feeService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }

        [HttpGet]
        public JsonResult GetAllCity()
        {
            var model = _cityService.GetAll();
            return Json(new
            {
                Data = model
            });
        }

        [HttpGet]
        public JsonResult GetAllDistricts()
        {
            var model = _districtsService.GetAll();
            return Json(new
            {
                Data = model
            });
        }

        [HttpGet]
        public JsonResult GetDistrictsForCityId(int cityId)
        {
            var model = _districtsService.GetDistrictForCityId(cityId);
            return Json(new
            {
                Data = model
            });
        }

        [HttpGet]
        public JsonResult GetWardsForDistrictId(int districtId)
        {
            var model = _wardsService.GetWardsForDistrictId(districtId);
            return Json(new
            {
                Data = model
            });
        }


        [HttpGet]
        public JsonResult GetAllWards()
        {
            var model = _wardsService.GetAll();
            return Json(new
            {
                Data = model
            });
        }


        //[HttpGet]
        //public JsonResult GetAll()
        //{            
        //    var data = _feeService.GetAll();
        //    return Json(new
        //    {
        //        Data = data
        //    });
        //}

        [HttpGet]
        public IActionResult AddOrUpdate()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Add(FeeModelView FeeModelView)
        {

            _feeService.Add(FeeModelView);
            _feeService.Save();
            return Json(new
            {
                success = true
            });
        }


        [HttpPost]
        public JsonResult Update(FeeModelView FeeModelView)
        {
            var result = _feeService.Update(FeeModelView);
            _feeService.Save();
            return Json(new
            {
                success = result
            });


        }

     
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _feeService.Deleted(id);
            _feeService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(FeeViewModelSearch feeViewModelSearch)
        {
            if (!string.IsNullOrEmpty(feeViewModelSearch.name))
            {
                var city = _cityService.GetByName(feeViewModelSearch.name);
                if (city != null)
                {
                    feeViewModelSearch.city_id = city.id;
                }
                var district = _districtsService.GetByName(feeViewModelSearch.name);
                if (district != null)
                {
                    feeViewModelSearch.district_id = district.id;
                }

                var wards = _wardsService.GetByName(feeViewModelSearch.name);
                if (wards != null)
                {
                    feeViewModelSearch.ward_id = wards.id;
                }

            }
            var data = _feeService.GetAllPaging(feeViewModelSearch);
            if (data != null && data.Results != null && data.Results.Count > 0)
            {
                foreach (var item in data.Results)
                {
                    if (item.city_id.HasValue && item.city_id.Value > 0)
                    {
                        var dataModel = _cityService.GetById(item.city_id.Value);
                        if (dataModel != null)
                        {
                            item.CityModelView = dataModel;
                        }
                    }

                    if (item.district_id.HasValue && item.district_id.Value > 0)
                    {
                        var dataModel = _districtsService.GetById(item.district_id.Value);
                        if (dataModel != null)
                        {
                            item.DistrictsModelView = dataModel;
                        }
                    }

                    if (item.ward_id.HasValue && item.ward_id.Value > 0)
                    {
                        var dataModel = _wardsService.GetById(item.ward_id.Value);
                        if (dataModel != null)
                        {
                            item.WardsModelView = dataModel;
                        }
                    }
                    if (item.fee.HasValue && item.fee.Value > 0)
                    {
                        item.feestr = item.fee.Value.ToString("#,###");
                    }
                }
            }
            return Json(new { data = data });
        }
    }
}
