using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAppUserService _appUserService;
        public IHttpContextAccessor _httpContextAccessor;
        public UsersController(IAppUserService appUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            _appUserService = appUserService;
            _httpContextAccessor = httpContextAccessor;
        }  
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AppLogin(string userName,string passWord)
        {
            var result = _appUserService.AppUserLogin(userName, passWord);
            
            if (result != null)
            {
                _httpContextAccessor.HttpContext.Session.SetString("UserInfor", JsonConvert.SerializeObject(result));

                return Json(new
                {
                    success = true
                });
            }
            return Json(new
            {
                success = false
            });
        }


        [HttpPost]
        public JsonResult AddRegister(UserModelView UserModelView)
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
                if (result > 0)
                {
                    var _user = _appUserService.GetByid(result);
                    _httpContextAccessor.HttpContext.Session.SetString("UserInfor", JsonConvert.SerializeObject(_user));
                }
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

        public JsonResult AppLogin(UserModelView UserModelView)
        {
            var result = _appUserService.Add(UserModelView);
            _appUserService.Save();
            return Json(new
            {
                success = result
            });

        }

    }
}
