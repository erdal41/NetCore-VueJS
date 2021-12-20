using Microsoft.AspNetCore.Http;
using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;
using System.Threading.Tasks;

namespace VueJS.Mvc.Helpers.Abstract
{
    public interface IImageHelper
    {
        Task<IDataResult<FileUploadDto>> Upload(IFormFile uploadFile);

        IDataResult<FileDeleteDto> Delete(string uploadFileName);
    }
}