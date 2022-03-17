using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VueJS.Data.Abstract;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using VueJS.Services.Helper.Abstract;

namespace VueJS.Services.Helper.Concrete
{
    public class ExtensionsHelper : IExtensionsHelper
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExtensionsHelper(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
        //@Url.link @Url.FriendlyUrlHelper()
        public string FriendlySEOPostName(string url)
        {
            if (string.IsNullOrEmpty(url)) return "";
            url = url.ToLower();
            url = url.Trim();
            if (url.Length > 100)
            {
                url = url.Substring(0, 100);
            }
            url = url.Replace("İ", "I");
            url = url.Replace("ı", "i");
            url = url.Replace("ğ", "g");
            url = url.Replace("Ğ", "G");
            url = url.Replace("ç", "c");
            url = url.Replace("Ç", "C");
            url = url.Replace("ö", "o");
            url = url.Replace("Ö", "O");
            url = url.Replace("ş", "s");
            url = url.Replace("Ş", "S");
            url = url.Replace("ü", "u");
            url = url.Replace("Ü", "U");
            url = url.Replace("'", "");
            url = url.Replace("\"", "");
            char[] replacerList = @"$%#@!*?;:~`+=()[]{}|\'<>,/^&"".".ToCharArray();
            for (int i = 0; i < replacerList.Length; i++)
            {
                string strChr = replacerList[i].ToString();
                if (url.Contains(strChr))
                {
                    url = url.Replace(strChr, string.Empty);
                }
            }
            Regex regex = new Regex("[^a-zA-Z0-9_-]");
            url = regex.Replace(url, "-");
            while (url.IndexOf("--", StringComparison.Ordinal) > -1)
                url = url.Replace("--", "-");
            return url;
        }

        public async Task<object> GetParentsAsync(ObjectType objectType, object entity)
        {
            if (objectType == ObjectType.basepage || objectType == ObjectType.page || objectType == ObjectType.article)
            {
                var post = (Post)entity;
                Post parent = post.Parent;
                post.Parents = new List<Post>();
                while (parent != null)
                {
                    post.Parents.Add(parent);
                    parent = await _unitOfWork.Posts.GetAsync(p => p.Id == parent.ParentId, p => p.Parent, p => p.Children);
                }
                return post;
            }
            else
            {
                var term = (Term)entity;
                Term parent = term.Parent;
                term.Parents = new List<Term>();
                while (parent != null)
                {
                    term.Parents.Add(parent);
                    parent = await _unitOfWork.Terms.GetAsync(p => p.Id == parent.ParentId, p => p.Parent, p => p.Children);
                }
                return term;
            }
        }

        public async Task<string> GetParentsURLAsync(ObjectType objectType, object entity)
        {
            string url = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value;
            if (objectType == ObjectType.basepage || objectType == ObjectType.page || objectType == ObjectType.article)
            {
                Post post = (Post)await GetParentsAsync(objectType, entity);
                List<Post> parents = post.Parents;

                for (int i = parents.Count; i > 0; --i)
                {
                    url += "/" + parents[i].PostName;
                }
                url += "/" + post.PostName;

                if (url == _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value) return "";
            }
            else
            {
                url += "/kategori";
                Term term = (Term)await GetParentsAsync(objectType, entity);
                List<Term> parents = term.Parents;
                for (int i = parents.Count; i > 0; --i)
                {
                    url += "/" + parents[i].Slug;
                }
                url += "/" + term.Slug;

                if (url == _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value) return "";

            }
            return url;
        }

        public async Task<string> FriendlySEOUrlAsync(ObjectType objectType, object entity)
        {
            string url = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value;
            if (objectType == ObjectType.basepage || objectType == ObjectType.page || objectType == ObjectType.article)
            {
                Post post = (Post)await GetParentsAsync(objectType, entity);
                List<Post> parents = post.Parents;

                for (int i = parents.Count; i > 0; --i)
                {
                    url += "/" + parents[i].PostName;
                }
                url += "/" + post.PostName;
            }
            else
            {
                Term term = (Term)await GetParentsAsync(objectType, entity);
                List<Term> parents = term.Parents;
                for (int i = parents.Count; i > 0; --i)
                {
                    url += "/" + parents[i].Slug;
                }
                url += "/" + term.Slug;
            }

            if (url == _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value) return "";
            url = url.ToLower();
            url = url.Trim();
            if (url.Length > 100)
            {
                url = url.Substring(0, 100);
            }
            url = url.Replace("İ", "I");
            url = url.Replace("ı", "i");
            url = url.Replace("ğ", "g");
            url = url.Replace("Ğ", "G");
            url = url.Replace("ç", "c");
            url = url.Replace("Ç", "C");
            url = url.Replace("ö", "o");
            url = url.Replace("Ö", "O");
            url = url.Replace("ş", "s");
            url = url.Replace("Ş", "S");
            url = url.Replace("ü", "u");
            url = url.Replace("Ü", "U");
            url = url.Replace("'", "");
            url = url.Replace("\"", "");
            char[] replacerList = @"$%#@!*?;:~`+=()[]{}|\'<>,/^&"".".ToCharArray();
            for (int i = 0; i < replacerList.Length; i++)
            {
                string strChr = replacerList[i].ToString();
                if (url.Contains(strChr))
                {
                    url = url.Replace(strChr, string.Empty);
                }
            }
            Regex regex = new Regex("[^a-zA-Z0-9_-]");
            url = regex.Replace(url, "-");
            while (url.IndexOf("--", StringComparison.Ordinal) > -1)
                url = url.Replace("--", "-");
            return url;
        }
    }
}