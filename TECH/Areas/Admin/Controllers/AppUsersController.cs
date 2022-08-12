using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class AppUsersController : BaseController
    {
        private readonly IAppUserService _appUserService;
        public AppUsersController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new UserModelView();
            if (id > 0)
            {
                model = _appUserService.GetByid(id);
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

        //[HttpPost]
        //public IActionResult UploadImageAvatar()
        //{
        //    var files = Request.Form.Files;
        //    if (files != null && files.Count > 0)
        //    {
        //        string folder = _hostingEnvironment.WebRootPath + $@"\avatar";

        //        if (!Directory.Exists(folder))
        //        {
        //            Directory.CreateDirectory(folder);
        //        }
        //        var _lstImageName = new List<string>();

        //        foreach (var itemFile in files)
        //        {
        //            string filePath = Path.Combine(folder, itemFile.FileName);
        //            if (!System.IO.File.Exists(filePath))
        //            {
        //                _lstImageName.Add(itemFile.FileName);
        //                using (FileStream fs = System.IO.File.Create(filePath))
        //                {
        //                    itemFile.CopyTo(fs);
        //                    fs.Flush();
        //                }
        //            }
        //        }                
        //    }
        //    return Json(new
        //    {
        //        success = true
        //    });
        //}

        //[HttpPost]
        //public JsonResult IsEmailExist(string email)
        //{
        //    bool isMail = false;
        //    if (!string.IsNullOrEmpty(email))
        //    {
        //        isMail = _appUserService.IsMailExist(email);
        //    }

        //    return Json(new
        //    {
        //        success = isMail
        //    });
        //}

        //[HttpPost]
        //public JsonResult IsPhoneExist(string phone)
        //{
        //    bool isphone = false;
        //    if (!string.IsNullOrEmpty(phone))
        //    {
        //        isphone = _appUserService.IsPhoneExist(phone);
        //    }

        //    return Json(new
        //    {
        //        success = isphone
        //    }) ;
        //}



        [HttpPost]
        public JsonResult Add(UserModelView UserModelView)
        {            
            bool isMailExist = false;
            bool isPhoneExist = false;
            if (UserModelView != null && !string.IsNullOrEmpty(UserModelView.email))
            {
                var isMail = _appUserService.IsMailExist(UserModelView.email);
                if (isMail)
                {
                    isMailExist = true;
                }
            }

            if (UserModelView != null && !string.IsNullOrEmpty(UserModelView.phone_number))
            {
                var isPhone = _appUserService.IsPhoneExist(UserModelView.phone_number);
                if (isPhone)
                {
                    isPhoneExist = true;
                }
            }

            if (!isMailExist && !isPhoneExist)
            {
                var result = _appUserService.Add(UserModelView);
                _appUserService.Save();
                return Json(new
                {
                    success = result
                });
            }
            return Json(new
            {
                success = false,
                isMailExist = isMailExist,
                isPhoneExist = isPhoneExist
            });

        }

        [HttpGet]
        public JsonResult UpdateStatus(int id,int status)
        {
            if (id > 0)
            {
               var  model = _appUserService.UpdateStatus(id, status);
                _appUserService.Save();
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
        public JsonResult Update(UserModelView UserModelView)
        {

            bool isMailExist = false;
            bool isPhoneExist = false;
            if (UserModelView != null && !string.IsNullOrEmpty(UserModelView.email))
            {
                var isMail = _appUserService.IsMailExist(UserModelView.email);
                if (isMail)
                {
                    isMailExist = true;
                }
            }

            if (UserModelView != null && !string.IsNullOrEmpty(UserModelView.phone_number))
            {
                var isPhone = _appUserService.IsPhoneExist(UserModelView.phone_number);
                if (isPhone)
                {
                    isPhoneExist = true;
                }
            }

            var result = _appUserService.Update(UserModelView);
            _appUserService.Save();
            return Json(new
            {
                success = result
            });



        }

        //[HttpPost]
        //public JsonResult AddUserRoles (int userId, int[] rolesId)
        //{
        //    try
        //    {
        //        _appUserRoleService.AddAppUserRole(userId, rolesId);

        //        return Json(new
        //        {
        //            success = true
        //        });
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new
        //        {
        //            success = false
        //        });
        //    }

        //}

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _appUserService.Deleted(id);
            _appUserService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(UserModelViewSearch colorViewModelSearch)
        {
            var data = _appUserService.GetAllPaging(colorViewModelSearch);
            return Json(new { data = data });
        }
        //[HttpGet]
        //public JsonResult GetAll()
        //{
        //    var data = _appUserService.GetAll();
        //    return Json(new { Data = data });
        //}
    }
}
