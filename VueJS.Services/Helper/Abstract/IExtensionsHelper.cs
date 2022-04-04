using System.Collections.Generic;
using System.Threading.Tasks;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;

namespace VueJS.Services.Helper.Abstract
{
    public interface IExtensionsHelper
    {
        string FriendlySEOString(string url);
        Task<object> GetParentsAsync(ObjectType? objectType, object entity);
        Task<string> GetParentsURLAsync(ObjectType objectType, object entity);
        Task<string> FriendlySEOUrlAsync(ObjectType objectType, object entity);
        Task<ICollection<MenuDetail>> GetChildAsync(ICollection<MenuDetail> menuDetails);
    }
}