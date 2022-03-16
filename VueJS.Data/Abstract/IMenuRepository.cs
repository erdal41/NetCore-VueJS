using System.Threading.Tasks;
using VueJS.Entities.Concrete;
using VueJS.Shared.Data.Abstract;

namespace VueJS.Data.Abstract
{
    public interface IMenuRepository : IEntityRepository<Menu>
    {
        Task<Menu> GetById(int menuId);
    }
}