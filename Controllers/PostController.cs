using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Models;
using TECH.Service;

namespace TECH.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostsService _postsService;
        private readonly IAppUserService _appUserService;
        public PostController(IPostsService postService,
            IAppUserService appUserService)
        {
            _postsService = postService;
            _appUserService = appUserService;
        }
        public IActionResult Index()
        {
             var postsViewModelSearch = new PostsViewModelSearch();
            postsViewModelSearch.PageIndex = 1;
            postsViewModelSearch.PageSize = 10;
            var data = _postsService.GetAllPaging(postsViewModelSearch);
            foreach (var item in data.Results)
            {
                if (item.author.HasValue)
                {
                    var appUser = _appUserService.GetByid(item.author.Value);
                    if (appUser != null && !string.IsNullOrEmpty(appUser.full_name))
                    {
                        item.author_name = appUser.full_name;
                    }
                }

            }
            return View(data);
        }
        public IActionResult DetailPost(int postId)
        {
            var model = new PostModelView();
            if (postId > 0)
            {
                model = _postsService.GetByid(postId);
            }

            return View(model);
        }
        

    }
}