using Microsoft.AspNetCore.Http;
using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VueJS.Mvc.Helpers.Abstract
{
    public interface IImageHelper
    {
        Task<IDataResult<UploadDto>> Upload(IFormFile file, int userId);

        IDataResult<FileDeleteDto> Delete(string uploadFileName);
    }
}