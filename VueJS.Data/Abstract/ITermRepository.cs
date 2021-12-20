using System.Threading.Tasks;
using VueJS.Entities.Concrete;
using VueJS.Shared.Data.Abstract;

namespace VueJS.Data.Abstract
{
    public interface ITermRepository : IEntityRepository<Term>
    {
        Task<Term> GetById(int termId);
    }
}