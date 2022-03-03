using VueJS.Data.Abstract;
using VueJS.Data.Concrete.EntityFramework.Contexts;
using System.Threading.Tasks;
using VueJS.Data.Concrete.EntityFramework.Repositories;

namespace VueJS.Data.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebAppContext _context;
        private PostRepository _postRepository;
        private TermRepository _termRepository;
        private PostTermRepository _postTermRepository;
        private UploadRepository _uploadRepository;
        private GalleryRepository _galleryRepository;
        private GeneralSettingRepository _generalSettingRepository;
        private SeoObjectSettingRepository _seoObjectSettingRepository;
        private SeoGeneralSettingRepository _seoGeneralSettingRepository;
        private UrlRedirectRepository _urlRedirectRepository;
        private CommentRepository _commentRepository;

        public UnitOfWork(WebAppContext context)
        {
            _context = context;
        }

        public IPostRepository Posts => _postRepository ??= new PostRepository(_context);
        public ITermRepository Terms => _termRepository ??= new TermRepository(_context);
        public IPostTermRepository PostTerms => _postTermRepository ??= new PostTermRepository(_context);
        public IUploadRepository Uploads => _uploadRepository ??= new UploadRepository(_context);
        public IGalleryRepository Galleries => _galleryRepository ??= new GalleryRepository(_context);
        public IGeneralSettingRepository GeneralSettings => _generalSettingRepository ??= new GeneralSettingRepository(_context);
        public ISeoObjectSettingRepository SeoObjectSettings => _seoObjectSettingRepository ??= new SeoObjectSettingRepository(_context);
        public ISeoGeneralSettingRepository SeoGeneralSettings => _seoGeneralSettingRepository ??= new SeoGeneralSettingRepository(_context);
        public IUrlRedirectRepository UrlRedirects => _urlRedirectRepository ??= new UrlRedirectRepository(_context);
        public ICommentRepository Comments => _commentRepository ??= new CommentRepository(_context);

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}