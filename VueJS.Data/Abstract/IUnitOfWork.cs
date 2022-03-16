using System;
using System.Threading.Tasks;

namespace VueJS.Data.Abstract
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IPostRepository Posts { get; }
        ITermRepository Terms { get; }
        IPostTermRepository PostTerms { get; }
        IUploadRepository Uploads { get; }
        IGalleryRepository Galleries { get; }
        IGeneralSettingRepository GeneralSettings { get; }
        ISeoObjectSettingRepository SeoObjectSettings { get; }
        ISeoGeneralSettingRepository SeoGeneralSettings { get; }
        IUrlRedirectRepository UrlRedirects { get; }
        ICommentRepository Comments { get; }
        IMenuRepository Menus { get; }
        IMenuDetailRepository MenuDetails { get; }
        Task<int> SaveAsync();
    }
}