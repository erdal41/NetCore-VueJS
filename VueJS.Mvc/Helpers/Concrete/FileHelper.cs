using AspNetCore.SEOHelper.Sitemap;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Dtos;
using VueJS.Mvc.Helpers.Abstract;
using VueJS.Services.Abstract;
using VueJS.Shared.Utilities.Results.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using VueJS.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VueJS.Mvc.Helpers.Concrete
{
    public class FileHelper : IFileHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _http;
        private readonly IPostService _postService;
        private readonly ITermService _termService;
        private readonly ISeoService _seoService;
        private readonly HostString _domainName;
        private readonly string _domainPath;
        private readonly string _wwwroot;
        private const string cssFolder = "assets/web/css";
        private const string jsFolder = "assets/web/js";

        public FileHelper(IWebHostEnvironment env, IHttpContextAccessor http, IPostService postService, ITermService termService, ISeoService seoService)
        {
            _env = env;
            _http = http;
            _postService = postService;
            _termService = termService;
            _seoService = seoService;
            _domainName = _http.HttpContext.Request.Host;
            _domainPath = _env.ContentRootPath;
            _wwwroot = _env.WebRootPath;
        }

        public async Task<string> CreateSitemapInRootDirectoryAsync()
        {
            var pages = await _postService.GetAllAsync(ObjectType.page, null, null);
            var articles = await _postService.GetAllAsync(ObjectType.article, null, null);
            var categories = await _termService.GetAllAsync(ObjectType.category);
            var tags = await _termService.GetAllAsync(ObjectType.tag);
            var seoGeneraSetting = await _seoService.GetSeoGeneralSettingDtoAsync();

            if (pages.ResultStatus == ResultStatus.Success && categories.ResultStatus == ResultStatus.Success && tags.ResultStatus == ResultStatus.Success)
            {
                var sitemap = new List<SitemapNode>();
                if (pages.Data.Posts != null && seoGeneraSetting.Data.SeoGeneralSetting.IsActivePageSearchEngineIndex)
                {
                    foreach (var page in pages.Data.Posts)
                    {
                        if (page.PostStatus == PostStatus.publish)
                        {
                            string url = "";
                            if (page.Parents != null)
                            {
                                for (int i = page.Parents.Count; i-- > 0;)
                                {
                                    url += "/" + page.Parents[i].PostName.ToLower();
                                }
                            }
                            sitemap.Add(new SitemapNode { LastModified = page.ModifiedDate.ToUniversalTime(), Url = _domainName + url + "/" + page.PostName.ToLower() });
                        }
                    }
                }

                if (articles.Data.Posts != null && seoGeneraSetting.Data.SeoGeneralSetting.IsActiveArticleSearchEngineIndex)
                {
                    foreach (var article in articles.Data.Posts)
                    {
                        if (article.PostStatus == PostStatus.publish)
                        {
                            string url = "/" + article.PostName.ToLower();
                            sitemap.Add(new SitemapNode { LastModified = article.ModifiedDate.ToUniversalTime(), Url = _domainName + url });
                        }
                    }
                }

                if (categories.Data.Terms != null && seoGeneraSetting.Data.SeoGeneralSetting.IsActiveCategorySearchEngineIndex)
                {
                    foreach (var category in categories.Data.Terms)
                    {
                        string url = "/blog/kategori";
                        if (category.Parents != null)
                        {
                            for (int i = category.Parents.Count; i-- > 0;)
                            {
                                url += "/" + category.Parents[i].Slug.ToLower();
                            }
                        }
                        sitemap.Add(new SitemapNode { Url = _domainName + url + "/" + category.Slug.ToLower() });
                    }
                }

                if (tags.Data.Terms != null && seoGeneraSetting.Data.SeoGeneralSetting.IsActiveTagSearchEngineIndex)
                {
                    foreach (var tag in tags.Data.Terms)
                    {
                        string url = "/blog/etiket/" + tag.Slug.ToLower();
                        sitemap.Add(new SitemapNode { Url = _domainName + url });
                    }
                }

                new SitemapDocument().CreateSitemapXML(sitemap, _env.ContentRootPath);
                return "sitemap.xml file should be create in root directory";
            }
            return null;
        }

        public string DefaultRobotsTxt()
        {
            var sb = new StringBuilder();
            sb.AppendLine("User-agent: *")
                .Append("Disallow: ")
                .AppendLine("/admin/")
                .Append("Sitemap: ")
                .Append(_http.HttpContext.Request.Scheme)
                .Append("://")
                .Append(_http.HttpContext.Request.Host)
                .AppendLine("/sitemap.xml");

            return sb.ToString();
        }

        public IDataResult<FileDto> GetFileRead(string fileName)
        {
            var path = Path.Combine(_domainPath, fileName);
            if (!File.Exists(path))
            {
                return new DataResult<FileDto>(ResultStatus.Error, null, new FileDto
                {
                    Path = path
                });
            }

            using StreamReader sr = new StreamReader(path);
            var txt = sr.ReadToEnd();

            return new DataResult<FileDto>(ResultStatus.Success, null, new FileDto
            {
                Text = txt,
                Path = path
            });
        }

        public IDataResult<FileDto> GetJsFileRead(string fileName)
        {
            var filePath = $"{_wwwroot}/{jsFolder}";
            var path = Path.Combine(filePath, fileName);

            if (!File.Exists(path))
            {
                var file = File.Create(path);
                file.Close();
            }

            using StreamReader sr = new StreamReader(path);
            var txt = sr.ReadToEnd();

            return new DataResult<FileDto>(ResultStatus.Success, null, new FileDto
            {
                Text = txt,
                Path = path
            });
        }

        public IDataResult<FileDto> GetCssFileRead(string fileName)
        {
            var filePath = $"{_wwwroot}/{cssFolder}";
            var path = Path.Combine(filePath, fileName);

            if (!File.Exists(path))
            {
                var file = File.Create(path);
                file.Close();
            }

            using StreamReader sr = new StreamReader(path);
            var txt = sr.ReadToEnd();

            return new DataResult<FileDto>(ResultStatus.Success, null, new FileDto
            {
                Text = txt,
                Path = path
            });
        }

        public IDataResult<FileDto> JsFileUpdate(string fileName, string text)
        {
            var filePath = $"{_wwwroot}/{jsFolder}";
            var path = Path.Combine(filePath, fileName);
            File.WriteAllText(path, text);

            return new DataResult<FileDto>(ResultStatus.Success, $"{fileName} dosyası güncellendi.", new FileDto
            {
                Text = text,
                Path = path
            });
        }

        public IDataResult<FileDto> CssFileUpdate(string fileName, string text)
        {
            var filePath = $"{_wwwroot}/{cssFolder}";
            var path = Path.Combine(filePath, fileName);

            File.WriteAllText(path, text);

            return new DataResult<FileDto>(ResultStatus.Success, $"{fileName} dosyası güncellendi.", new FileDto
            {
                Text = text,
                Path = path
            });
        }

        public IDataResult<FileDto> FileAddOrUpdate(string fileName, string text)
        {
            var path = Path.Combine(_domainPath, fileName);

            if (!File.Exists(path))
            {
                var file = File.Create(path);
                file.Close();
            }

            File.WriteAllText(path, text);

            return new DataResult<FileDto>(ResultStatus.Success, $"{fileName} dosyası güncellendi.", new FileDto
            {
                Text = text,
                Path = path
            });
        }

        public IDataResult<FileDto> Delete(string fileName)
        {
            //var fileName = "robots.txt";
            var path = Path.Combine(_domainPath, fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            return new DataResult<FileDto>(ResultStatus.Success, $"{fileName} dosyası silindi.", new FileDto
            {
                Path = path
            });
        }
    }
}