using Microsoft.EntityFrameworkCore;
using VueJS.Data.Abstract;
using VueJS.Entities.Concrete;
using VueJS.Shared.Data.Concrete.EntityFramework;

namespace VueJS.Data.Concrete.EntityFramework.Repositories
{
    public class GeneralSettingRepository : EntityRepositoryBase<GeneralSetting>, IGeneralSettingRepository
    {
        public GeneralSettingRepository(DbContext context) : base(context)
        {
        }
    }
}