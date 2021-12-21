using AutoMapper;
using Microsoft.AspNetCore.Http;
using VueJS.Data.Abstract;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using VueJS.Services.Abstract;
using VueJS.Shared.Utilities.Results.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using VueJS.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VueJS.Services.Concrete
{
    public class SeoManager : ManagerBase, ISeoService
    {
        private readonly IHttpContextAccessor _http;
        private readonly string _domainName;

        public SeoManager(IHttpContextAccessor http, IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _http = http;
            _domainName = _http.HttpContext.Request.Scheme + "://" + _http.HttpContext.Request.Host;
        }
        public async Task<IDataResult<SeoGeneralSettingDto>> GetHeadSeoSettingAsync()
        {
            var seoSetting = await UnitOfWork.SeoGeneralSettings.FirstOrDefaultAsync(sgs => sgs.OpenGraphImage, sgs => sgs.User);
            if (seoSetting != null)
            {
                var seoSettingDto = Mapper.Map<SeoGeneralSettingDto>(seoSetting);
                return new DataResult<SeoGeneralSettingDto>(ResultStatus.Success, new SeoGeneralSettingDto
                {
                    SeoGeneralSetting = seoSetting
                });
            }
            return new DataResult<SeoGeneralSettingDto>(ResultStatus.Error, "Seo ayarları yüklenirken bir hata oluştu. Hata devam ederse lütfen yönetici ile iletişime geçiniz.", null);
        }

        public async Task<IDataResult<GeneralSettingDto>> GetRobotsTxtAsync()
        {
            var generalSetting = await UnitOfWork.GeneralSettings.FirstOrDefaultAsync();
            if (generalSetting != null)
            {
                Mapper.Map<GeneralSettingDto>(generalSetting);
                return new DataResult<GeneralSettingDto>(ResultStatus.Success, new GeneralSettingDto
                {
                    GeneralSetting = generalSetting
                });
            }
            return new DataResult<GeneralSettingDto>(ResultStatus.Error, "Genel ayarlar yüklenirken bir hata oluştu. Hata devam ederse lütfen yönetici ile iletişime geçiniz.", null);
        }


        #region GENERAL

        public async Task<IDataResult<SeoGeneralSettingDto>> GetSeoGeneralSettingDtoAsync()
        {
            var seoGeneralSetting = await UnitOfWork.SeoGeneralSettings.FirstOrDefaultAsync(sgs => sgs.OpenGraphImage, sgs => sgs.SiteMainImage);
            if (seoGeneralSetting != null)
            {
                return new DataResult<SeoGeneralSettingDto>(ResultStatus.Success, new SeoGeneralSettingDto
                {
                    SeoGeneralSetting = seoGeneralSetting
                });
            }
            else
            {
                return new DataResult<SeoGeneralSettingDto>(ResultStatus.Error, "Seo ayarları yüklenirken bir hata oluştu. Hata devam ederse lütfen yönetici ile iletişime geçiniz.", new SeoGeneralSettingDto
                {
                    SeoGeneralSetting = seoGeneralSetting
                });
            }
        }

        public async Task<IDataResult<SeoGeneralSettingUpdateDto>> GetSeoGeneralSettingUpdateDtoAsync()
        {
            var seoSetting = await UnitOfWork.SeoGeneralSettings.FirstOrDefaultAsync(sgs => sgs.OpenGraphImage, sgs => sgs.SiteMainImage);
            if (seoSetting != null)
            {
                var seoSettingUpdateDto = Mapper.Map<SeoGeneralSettingUpdateDto>(seoSetting);
                return new DataResult<SeoGeneralSettingUpdateDto>(ResultStatus.Success, seoSettingUpdateDto);
            }
            else
            {
                return new DataResult<SeoGeneralSettingUpdateDto>(ResultStatus.Error, "Seo ayarları yüklenirken bir hata oluştu. Hata devam ederse lütfen yönetici ile iletişime geçiniz.", null);
            }
        }

        public async Task<IDataResult<SeoGeneralSettingDto>> SeoGeneralSettingUpdateAsync(SeoGeneralSettingUpdateDto generalSettingUpdateDto, int userId)
        {
            var oldSeoSetting = await UnitOfWork.SeoGeneralSettings.GetAsync(gs => gs.Id == generalSettingUpdateDto.Id);
            var seoSetting = Mapper.Map<SeoGeneralSettingUpdateDto, SeoGeneralSetting>(generalSettingUpdateDto, oldSeoSetting);
            seoSetting.UserId = userId;
            var updatedSeoSetting = await UnitOfWork.SeoGeneralSettings.UpdateAsync(seoSetting);
            await UnitOfWork.SaveAsync();
            return new DataResult<SeoGeneralSettingDto>(ResultStatus.Success, "Seo ayarları başarılı bir şekilde güncellendi!", new SeoGeneralSettingDto
            {
                SeoGeneralSetting = updatedSeoSetting,
                ResultStatus = ResultStatus.Success,
                Message = "Seo ayarları başarılı bir şekilde güncellendi!"
            });
        }

        #endregion

        #region OBJECT

        public async Task<IDataResult<SeoObjectSettingDto>> GetSeoObjectSettingDtoAsync(int objectId, SubObjectType subSeoObjectType)
        {
            var seoObjectSetting = await UnitOfWork.SeoObjectSettings.GetAsync(s => s.ObjectId == objectId && s.SubObjectType == subSeoObjectType, s => s.OpenGraphImage, s => s.TwitterImage, s => s.User);

            if (seoObjectSetting != null)
            {
                return new DataResult<SeoObjectSettingDto>(ResultStatus.Success, new SeoObjectSettingDto
                {
                    SeoObjectSetting = seoObjectSetting
                });
            }
            else
            {
                return new DataResult<SeoObjectSettingDto>(ResultStatus.Error, new SeoObjectSettingDto
                {
                    SeoObjectSetting = seoObjectSetting
                });
            }
        }

        public async Task<IDataResult<SeoObjectSettingUpdateDto>> GetSeoObjectSettingUpdateDtoAsync(int objectId, SubObjectType subSeoObjectType)
        {
            var seoObject = await UnitOfWork.SeoObjectSettings.GetAsync(s => s.ObjectId == objectId && s.SubObjectType == subSeoObjectType, s => s.OpenGraphImage, s => s.TwitterImage, s => s.User);

            if (seoObject != null)
            {
                var seoObjectSettingUpdateDto = Mapper.Map<SeoObjectSettingUpdateDto>(seoObject);
                return new DataResult<SeoObjectSettingUpdateDto>(ResultStatus.Success, seoObjectSettingUpdateDto);
            }
            else
            {
                return new DataResult<SeoObjectSettingUpdateDto>(ResultStatus.Error, "Seo ayarları yüklenirken bir hata oluştu. Hata devam ederse lütfen yönetici ile iletişime geçiniz.", null);
            }
        }

        public async Task<IResult> SeoObjectSettingUpdateAsync(int objectId, SubObjectType subSeoObjectType, SeoObjectSettingUpdateDto seoObjectSettingUpdateDto, int userId)
        {
            var oldSeoObjectSetting = await UnitOfWork.SeoObjectSettings.GetAsync(s => s.ObjectId == objectId && s.SubObjectType == subSeoObjectType);
            if (oldSeoObjectSetting != null)
            {
                var seoObjectSetting = Mapper.Map<SeoObjectSettingUpdateDto, SeoObjectSetting>(seoObjectSettingUpdateDto, oldSeoObjectSetting);
                seoObjectSetting.SubObjectType = subSeoObjectType;
                seoObjectSetting.UserId = userId;
                seoObjectSetting.ModifiedDate = DateTime.Now;
                var updatedSeoSetting = await UnitOfWork.SeoObjectSettings.UpdateAsync(seoObjectSetting);
                await UnitOfWork.SaveAsync();
                return new DataResult<SeoObjectSetting>(ResultStatus.Success, seoObjectSetting);
            }
            else
            {
                return new DataResult<SeoObjectSetting>(ResultStatus.Success, null);
            }
        }

        public async Task<IDataResult<SeoObjectSettingDto>> SeoObjectSettingAddAsync(ObjectType seoObjectType, SubObjectType subSeoObjectType, int objectId, SeoObjectSettingAddDto seoObjectSettingAddDto, int userId)
        {
            var seoObject = Mapper.Map<SeoObjectSetting>(seoObjectSettingAddDto);
            seoObject.ObjectId = objectId;
            seoObject.UserId = userId;
            seoObject.CreatedDate = DateTime.Now;
            seoObject.ModifiedDate = DateTime.Now;


            switch (seoObjectType)
            {
                case ObjectType.post:
                    if (subSeoObjectType == SubObjectType.page)
                    {
                        var post = await UnitOfWork.Posts.GetAsync(p => p.PostType == SubObjectType.page && p.Id == objectId);
                        if (post != null)
                        {
                            if (seoObjectSettingAddDto.SeoTitle == null)
                            {
                                seoObject.SeoTitle = post.Title;
                            }
                            seoObject.ObjectType = seoObjectType;
                            seoObject.SubObjectType = subSeoObjectType;

                            Post parent = post.Parent;
                            post.Parents = new List<Post>();
                            while (parent != null)
                            {
                                post.Parents.Add(parent);
                                parent = await UnitOfWork.Posts.GetAsync(p => p.PostType == SubObjectType.page && p.Id == parent.ParentId, p => p.Parent, p => p.Children);
                            }

                            if (post.Parents != null)
                            {
                                string url = _domainName;
                                for (int i = post.Parents.Count; i-- > 0;)
                                {
                                    url += "/" + post.Parents[i].PostName.ToLower();
                                }
                                seoObject.Permalink = url + "/" + post.PostName.ToLower();
                            }
                            else
                            {
                                string url = _domainName + "/";
                                seoObject.Permalink = url + post.PostName;
                            }
                        }
                    }
                    else if (subSeoObjectType == SubObjectType.article)
                    {
                        var post = await UnitOfWork.Posts.GetAsync(p => p.PostType == SubObjectType.article && p.Id == objectId);
                        if (post != null)
                        {
                            if (seoObjectSettingAddDto.SeoTitle == null)
                            {
                                seoObject.SeoTitle = post.Title;
                            }
                            seoObject.ObjectType = seoObjectType;
                            seoObject.SubObjectType = subSeoObjectType;
                            seoObject.Permalink = _domainName + "/" + post.PostName;
                        }
                    }
                    else if (subSeoObjectType == SubObjectType.basepage)
                    {
                        var post = await UnitOfWork.Posts.GetAsync(p => p.PostType == SubObjectType.basepage && p.Id == objectId);
                        if (post != null)
                        {
                            if (seoObjectSettingAddDto.SeoTitle == null)
                            {
                                seoObject.SeoTitle = post.Title;
                            }
                            seoObject.ObjectType = seoObjectType;
                            seoObject.SubObjectType = subSeoObjectType;
                            seoObject.Permalink = _domainName + "/" + post.PostName;
                        }
                    }
                    break;
                case ObjectType.term:
                    if (subSeoObjectType == SubObjectType.category)
                    {
                        var term = await UnitOfWork.Terms.GetAsync(p => p.TermType == SubObjectType.category && p.Id == objectId);
                        if (term != null)
                        {
                            if (seoObjectSettingAddDto.SeoTitle == null)
                            {
                                seoObject.SeoTitle = term.Name;
                            }
                            seoObject.ObjectType = seoObjectType;
                            seoObject.SubObjectType = subSeoObjectType;
                            seoObject.Permalink = _domainName + "/blog/kategori/" + term.Slug;
                        }
                    }
                    else if (subSeoObjectType == SubObjectType.tag)
                    {
                        var term = await UnitOfWork.Terms.GetAsync(p => p.TermType == SubObjectType.tag && p.Id == objectId);
                        if (term != null)
                        {
                            if (seoObjectSettingAddDto.SeoTitle == null)
                            {
                                seoObject.SeoTitle = term.Name;
                            }
                            seoObject.ObjectType = seoObjectType;
                            seoObject.SubObjectType = subSeoObjectType;
                            seoObject.Permalink = _domainName + "/blog/etiket/" + term.Slug;
                        }
                    }
                    break;
            }

            if (seoObject != null)
            {
                await UnitOfWork.SeoObjectSettings.AddAsync(seoObject);
                await UnitOfWork.SaveAsync();
                return new DataResult<SeoObjectSettingDto>(ResultStatus.Success, null, new SeoObjectSettingDto
                {
                    SeoObjectSetting = seoObject,
                    ResultStatus = ResultStatus.Success,
                });
            }
            else
            {
                return new DataResult<SeoObjectSettingDto>(ResultStatus.Error, null, new SeoObjectSettingDto
                {
                    SeoObjectSetting = seoObject,
                    ResultStatus = ResultStatus.Error,
                });
            }
        }

        #endregion
    }
}