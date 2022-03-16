using Microsoft.EntityFrameworkCore;
using VueJS.Data.Abstract;
using VueJS.Data.Concrete.EntityFramework.Contexts;
using VueJS.Entities.Concrete;
using VueJS.Shared.Data.Concrete.EntityFramework;
using System.Threading.Tasks;

namespace VueJS.Data.Concrete.EntityFramework.Repositories
{
    public class MenuDetailRepository : EntityRepositoryBase<MenuDetail>, IMenuDetailRepository
    {
        public MenuDetailRepository(DbContext context) : base(context)
        {
        }

        public async Task<MenuDetail> GetById(int menuId)
        {
            return await WebAppContext.MenuDetails.SingleOrDefaultAsync(c => c.Id == menuId);
        }

        private WebAppContext WebAppContext
        {
            get
            {
                return _context as WebAppContext;
            }
        }
    }
}