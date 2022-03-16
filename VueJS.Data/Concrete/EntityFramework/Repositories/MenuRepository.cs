using Microsoft.EntityFrameworkCore;
using VueJS.Data.Abstract;
using VueJS.Data.Concrete.EntityFramework.Contexts;
using VueJS.Entities.Concrete;
using VueJS.Shared.Data.Concrete.EntityFramework;
using System.Threading.Tasks;

namespace VueJS.Data.Concrete.EntityFramework.Repositories
{
    public class MenuRepository : EntityRepositoryBase<Menu>, IMenuRepository
    {
        public MenuRepository(DbContext context) : base(context)
        {
        }

        public async Task<Menu> GetById(int menuId)
        {
            return await WebAppContext.Menus.SingleOrDefaultAsync(c => c.Id == menuId);
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