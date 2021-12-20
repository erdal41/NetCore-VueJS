using Microsoft.EntityFrameworkCore;
using VueJS.Data.Abstract;
using VueJS.Data.Concrete.EntityFramework.Contexts;
using VueJS.Entities.Concrete;
using VueJS.Shared.Data.Concrete.EntityFramework;
using System.Threading.Tasks;

namespace VueJS.Data.Concrete.EntityFramework.Repositories
{
    public class TermRepository : EntityRepositoryBase<Term>, ITermRepository
    {
        public TermRepository(DbContext context) : base(context)
        {
        }

        public async Task<Term> GetById(int termId)
        {
            return await WebAppContext.Terms.SingleOrDefaultAsync(c => c.Id == termId);
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