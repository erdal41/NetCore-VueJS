using Microsoft.EntityFrameworkCore;
using VueJS.Data.Abstract;
using VueJS.Entities.Concrete;
using VueJS.Shared.Data.Concrete.EntityFramework;

namespace VueJS.Data.Concrete.EntityFramework.Repositories
{
    public class UploadRepository : EntityRepositoryBase<Upload>, IUploadRepository
    {
        public UploadRepository(DbContext context) : base(context)
        {
        }
    }
}