using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VueJS.Entities.Concrete;
using VueJS.Mvc.Helpers.Abstract;
using VueJS.Services.Concrete;

namespace VueJS.Mvc.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected UserManager<User> UserManager { get; }
        protected IMapper Mapper { get; }
        protected IImageHelper ImageHelper { get; }
        protected User LoggedInUser => UserManager.GetUserAsync(HttpContext.User).Result;

        public BaseController(UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper)
        {
            UserManager = userManager;
            Mapper = mapper;
            ImageHelper = imageHelper;
        }

        public BaseController(UserManager<User> userManager, IMapper mapper)
        {
            UserManager = userManager;
            Mapper = mapper;
        }
    }
}