using System.Threading.Tasks;
using VueJS.Entities.ComplexTypes;

namespace VueJS.Services.Helper.Abstract
{
    public interface IExtensionsHelper
    {
        string FriendlySEOPostName(string url);
        Task<object> GetParentsAsync(ObjectType objectType, object entity);
        Task<string> GetParentsURLAsync(ObjectType objectType, object entity);
        Task<string> FriendlySEOUrlAsync(ObjectType objectType, object entity);
    }
}