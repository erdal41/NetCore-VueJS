using System.Threading.Tasks;
using VueJS.Entities.Concrete;
using VueJS.Shared.Data.Abstract;

namespace VueJS.Data.Abstract
{
    public interface IMenuDetailRepository : IEntityRepository<MenuDetail>
    {
        Task<MenuDetail> GetById(int menuId);
    }
}