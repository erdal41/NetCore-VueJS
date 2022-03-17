using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using VueJS.Mvc.Helpers.Abstract;
using VueJS.Mvc.Areas.Web.Models;
using VueJS.Services.Abstract;
using VueJS.Shared.Utilities.Results.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace VueJS.Mvc.Areas.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IPostService _postService;
        private readonly ISeoService _seoService;
        private readonly ISettingService _settingService;
        //private readonly IFileHelper _fileHelper;
        //private readonly ICacheService _cacheService;
        //private readonly IMailService _mailService;
        private readonly IUrlRedirectService _urlRedirectService;
        //private readonly ArticleRightSideBarWidgetOptions _articleRightSideBarWidgetOptions;
        //private readonly ReCaptcha _reCaptcha;

        public HomeController(IPostService postService, ISeoService seoService, ISettingService settingService, IUrlRedirectService urlRedirectService)
        {
            _postService = postService;
            _seoService = seoService;
            _settingService = settingService;
            _urlRedirectService = urlRedirectService;
        }

        [HttpGet("/web/home/anasayfa")]
        public async Task<IActionResult> Index()
        {
            var postResult = await _postService.GetAsync("anasayfa");
            if (postResult.ResultStatus == ResultStatus.Success && postResult.Data.Post.PostType == ObjectType.basepage)
            {
                var generalSettings = await _settingService.GetGeneralSettingAsync();
                var seoGeneralSettings = await _seoService.GetSeoGeneralSettingDtoAsync();
                var seoObjectSettings = await _seoService.GetSeoObjectSettingDtoAsync(postResult.Data.Post.Id, postResult.Data.Post.PostType);
                return Json(new PostDetailViewModel
                {
                    PostDto = postResult.Data,
                    GeneralSettingDto = generalSettings.Data,
                    SeoGeneralSettingDto = seoGeneralSettings.Data,
                    SeoObjectSettingDto = seoObjectSettings.Data
                });

            }
            return NotFound();
        }

        [HttpGet("/web/home/{postName}")]
        public async Task<JsonResult> PostDetail(string postName)
        {
            var urlRedirects = await _urlRedirectService.GetAllAsync();
            var postResult = await _postService.GetAsync(postName);
            if (postResult.ResultStatus == ResultStatus.Success)
            {
                var seoGeneralSettings = await _seoService.GetSeoGeneralSettingDtoAsync();
                var seoObjectSettings = await _seoService.GetSeoObjectSettingDtoAsync(postResult.Data.Post.Id, postResult.Data.Post.PostType);
                var generalSetting = await _settingService.GetGeneralSettingAsync();
                if (postResult.Data.Post.PostType == ObjectType.article)
                {
                    var posts = await _postService.GetAllAsync(ObjectType.article, null);

                    return Json(new PostDetailViewModel
                    {
                        UrlRedirectListDto = urlRedirects.Data,
                        PostDto = postResult.Data,
                        SeoGeneralSettingDto = seoGeneralSettings.Data,
                        SeoObjectSettingDto = seoObjectSettings.Data,
                        GeneralSettingDto = generalSetting.Data
                    });

                }
                else if (postResult.Data.Post.PostType == ObjectType.page)
                {
                    return Json(new PostDetailViewModel
                    {
                        UrlRedirectListDto = urlRedirects.Data,
                        PostDto = postResult.Data,
                        SeoGeneralSettingDto = seoGeneralSettings.Data,
                        SeoObjectSettingDto = seoObjectSettings.Data,
                        GeneralSettingDto = generalSetting.Data
                    });
                }
            }
            return Json(new PostDetailViewModel
            {
                UrlRedirectListDto = urlRedirects.Data,
                PostDto = postResult.Data,
            });
        }
    }

    //[HttpGet]
    //[Route("hakkimizda")]
    //public async Task<IActionResult> Hakkimizda()
    //{
    //    if (_cacheService.TryGetValue("hakkimizda", out PostDetailViewModel postDetailViewModel))
    //    {
    //        return View(postDetailViewModel);
    //    }
    //    else
    //    {
    //        var postResult = await _postService.GetAsync("hakkimizda");
    //        if (postResult.ResultStatus == ResultStatus.Success && postResult.Data.Post.PostType == SubObjectType.basepage)
    //        {
    //            var generalSettings = await _settingService.GetGeneralSettingAsync();
    //            var seoGeneralSettings = await _seoService.GetSeoGeneralSettingDtoAsync();
    //            var seoObjectSettings = await _seoService.GetSeoObjectSettingDtoAsync(postResult.Data.Post.Id, postResult.Data.Post.PostType);

    //            _cacheService.Set("hakkimizda", new PostDetailViewModel
    //            {
    //                PostDto = postResult.Data,
    //                GeneralSettingDto = generalSettings.Data,
    //                SeoGeneralSettingDto = seoGeneralSettings.Data,
    //                SeoObjectSettingDto = seoObjectSettings.Data

    //            }, TimeSpan.FromDays(30));

    //            return View(new PostDetailViewModel
    //            {
    //                PostDto = postResult.Data,
    //                GeneralSettingDto = generalSettings.Data,
    //                SeoGeneralSettingDto = seoGeneralSettings.Data,
    //                SeoObjectSettingDto = seoObjectSettings.Data
    //            });
    //        }
    //        return NotFound();
    //    }
    //}

    //[HttpGet]
    //[Route("fuarlarimiz")]
    //public async Task<IActionResult> Fuarlarimiz()
    //{
    //    if (_cacheService.TryGetValue("fuarlarimiz", out PostDetailViewModel postDetailViewModel))
    //    {
    //        return View(postDetailViewModel);
    //    }
    //    else
    //    {
    //        var postResult = await _postService.GetAsync("fuarlarimiz");
    //        if (postResult.ResultStatus == ResultStatus.Success && postResult.Data.Post.PostType == SubObjectType.basepage)
    //        {
    //            var generalSettings = await _settingService.GetGeneralSettingAsync();
    //            var seoGeneralSettings = await _seoService.GetSeoGeneralSettingDtoAsync();
    //            var seoObjectSettings = await _seoService.GetSeoObjectSettingDtoAsync(postResult.Data.Post.Id, postResult.Data.Post.PostType);

    //            _cacheService.Set("fuarlarimiz", new PostDetailViewModel
    //            {
    //                PostDto = postResult.Data,
    //                GeneralSettingDto = generalSettings.Data,
    //                SeoGeneralSettingDto = seoGeneralSettings.Data,
    //                SeoObjectSettingDto = seoObjectSettings.Data

    //            }, TimeSpan.FromDays(30));

    //            return View(new PostDetailViewModel
    //            {
    //                PostDto = postResult.Data,
    //                GeneralSettingDto = generalSettings.Data,
    //                SeoGeneralSettingDto = seoGeneralSettings.Data,
    //                SeoObjectSettingDto = seoObjectSettings.Data
    //            });
    //        }
    //        return NotFound();
    //    }
    //}

    //[HttpGet]
    //[Route("referanslar")]
    //public async Task<IActionResult> Referanslar()
    //{
    //    if (_cacheService.TryGetValue("referanslar", out PostDetailViewModel postDetailViewModel))
    //    {
    //        return View(postDetailViewModel);
    //    }
    //    else
    //    {
    //        var postResult = await _postService.GetAsync("referanslar");
    //        if (postResult.ResultStatus == ResultStatus.Success && postResult.Data.Post.PostType == SubObjectType.basepage)
    //        {
    //            var generalSettings = await _settingService.GetGeneralSettingAsync();
    //            var seoGeneralSettings = await _seoService.GetSeoGeneralSettingDtoAsync();
    //            var seoObjectSettings = await _seoService.GetSeoObjectSettingDtoAsync(postResult.Data.Post.Id, postResult.Data.Post.PostType);

    //            _cacheService.Set("referanslar", new PostDetailViewModel
    //            {
    //                PostDto = postResult.Data,
    //                GeneralSettingDto = generalSettings.Data,
    //                SeoGeneralSettingDto = seoGeneralSettings.Data,
    //                SeoObjectSettingDto = seoObjectSettings.Data

    //            }, TimeSpan.FromDays(30));

    //            return View(new PostDetailViewModel
    //            {
    //                PostDto = postResult.Data,
    //                GeneralSettingDto = generalSettings.Data,
    //                SeoGeneralSettingDto = seoGeneralSettings.Data,
    //                SeoObjectSettingDto = seoObjectSettings.Data
    //            });
    //        }
    //        return NotFound();
    //    }
    //}

    //[HttpGet]
    //[Route("e-katalog")]
    //public async Task<IActionResult> EKatalog()
    //{
    //    if (_cacheService.TryGetValue("e-katalog", out PostDetailViewModel postDetailViewModel))
    //    {
    //        return View(postDetailViewModel);
    //    }
    //    else
    //    {
    //        var postResult = await _postService.GetAsync("e-katalog");
    //        if (postResult.ResultStatus == ResultStatus.Success && postResult.Data.Post.PostType == SubObjectType.basepage)
    //        {
    //            var generalSettings = await _settingService.GetGeneralSettingAsync();
    //            var seoGeneralSettings = await _seoService.GetSeoGeneralSettingDtoAsync();
    //            var seoObjectSettings = await _seoService.GetSeoObjectSettingDtoAsync(postResult.Data.Post.Id, postResult.Data.Post.PostType);

    //            _cacheService.Set("e-katalog", new PostDetailViewModel
    //            {
    //                PostDto = postResult.Data,
    //                GeneralSettingDto = generalSettings.Data,
    //                SeoGeneralSettingDto = seoGeneralSettings.Data,
    //                SeoObjectSettingDto = seoObjectSettings.Data

    //            }, TimeSpan.FromDays(30));

    //            return View(new PostDetailViewModel
    //            {
    //                PostDto = postResult.Data,
    //                GeneralSettingDto = generalSettings.Data,
    //                SeoGeneralSettingDto = seoGeneralSettings.Data,
    //                SeoObjectSettingDto = seoObjectSettings.Data
    //            });
    //        }
    //        return NotFound();
    //    }
    //}

    //[HttpGet]
    //[Route("iletisim")]
    //public async Task<IActionResult> Iletisim()
    //{
    //    if (_cacheService.TryGetValue("iletisim", out PostDetailViewModel postDetailViewModel))
    //    {
    //        return View(postDetailViewModel);
    //    }
    //    else
    //    {
    //        var postResult = await _postService.GetAsync("iletisim");
    //        if (postResult.ResultStatus == ResultStatus.Success && postResult.Data.Post.PostType == SubObjectType.basepage)
    //        {
    //            var generalSettings = await _settingService.GetGeneralSettingAsync();
    //            var seoGeneralSettings = await _seoService.GetSeoGeneralSettingDtoAsync();
    //            var seoObjectSettings = await _seoService.GetSeoObjectSettingDtoAsync(postResult.Data.Post.Id, postResult.Data.Post.PostType);

    //            _cacheService.Set("iletisim", new PostDetailViewModel
    //            {
    //                PostDto = postResult.Data,
    //                GeneralSettingDto = generalSettings.Data,
    //                SeoGeneralSettingDto = seoGeneralSettings.Data,
    //                SeoObjectSettingDto = seoObjectSettings.Data,
    //                SiteKey = _reCaptcha.SiteKey

    //            }, TimeSpan.FromDays(30));

    //            return View(new PostDetailViewModel
    //            {
    //                PostDto = postResult.Data,
    //                GeneralSettingDto = generalSettings.Data,
    //                SeoGeneralSettingDto = seoGeneralSettings.Data,
    //                SeoObjectSettingDto = seoObjectSettings.Data,
    //                SiteKey = _reCaptcha.SiteKey
    //            });
    //        }
    //        return NotFound();
    //    }
    //}
    //[HttpPost]
    //[Route("iletisim")]
    //public JsonResult Iletisim(EmailSendDto emailSendDto)
    //{
    //    var response = Request.Form["g-recaptcha-response"];
    //    string secretKey = _reCaptcha.SecretKey;
    //    //Kendi Secret keyinizle değiştirin.

    //    var client = new WebClient();
    //    var reply =
    //        client.DownloadString(
    //            string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));

    //    var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponseDto>(reply);

    //    if (captchaResponse.Success)
    //    {
    //        var result = _mailService.SendContactEmail(emailSendDto, "İletişim Sayfasındaki Formdan Gönderildi");
    //        if (result.ResultStatus == ResultStatus.Success)
    //        {
    //            var mailPostAjax = JsonConvert.SerializeObject(result);
    //            return Json(mailPostAjax);
    //        }
    //        else
    //        {
    //            var mailPostAjaxError = JsonConvert.SerializeObject(result);
    //            return Json(mailPostAjaxError);
    //        }
    //    }
    //    else
    //    {
    //        var mailPostAjaxError = JsonConvert.SerializeObject(captchaResponse);
    //        return Json(mailPostAjaxError);
    //    }
    //}

    //[HttpGet]
    //[Route("/robots.txt")]
    //public IActionResult RobotsTxt()
    //{
    //    if (_cacheService.TryGetValue("robots.txt", out IDataResult<FileDto> fileDto))
    //    {
    //        return View(fileDto.Data);
    //    }
    //    else
    //    {
    //        var result = _fileHelper.GetFileRead("robots.txt");
    //        if (result.ResultStatus == ResultStatus.Success)
    //        {
    //            _cacheService.Set("robots.txt", result, TimeSpan.FromDays(30));

    //            return View(result.Data);
    //        }
    //        return NotFound();
    //    }
    //}
}
