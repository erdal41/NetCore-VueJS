using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;
using System.Threading.Tasks;

namespace VueJS.Mvc.Helpers.Abstract
{
    public interface IFileHelper
    {
        Task<string> CreateSitemapInRootDirectoryAsync();
        string DefaultRobotsTxt();
        IDataResult<FileDto> GetFileRead(string fileName);
        IDataResult<FileDto> GetJsFileRead(string fileName);
        IDataResult<FileDto> GetCssFileRead(string fileName);
        IDataResult<FileDto> FileAddOrUpdate(string fileName, string text);
        IDataResult<FileDto> JsFileUpdate(string fileName, string text);
        IDataResult<FileDto> CssFileUpdate(string fileName, string text);
        IDataResult<FileDto> Delete(string text);
    }
}