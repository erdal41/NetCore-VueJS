using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using VueJS.Entities.Dtos;
using VueJS.Mvc.Helpers.Abstract;
using VueJS.Services.Extensions;
using VueJS.Shared.Utilities.Results.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using VueJS.Shared.Utilities.Results.Concrete;
using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VueJS.Mvc.Helpers.Concrete
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _domainPath;
        private const string mediaFolder = "ClientApp/src/assets/images/media";
        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
            _domainPath = _env.ContentRootPath;
        }

        public IDataResult<FileUploadDto> Upload(IFormFile uploadFile)
        {
            string dosyaYolu = $"{_domainPath}/{mediaFolder}";
            string contentType = uploadFile.ContentType;

            if (!Directory.Exists(dosyaYolu))
            {
                Directory.CreateDirectory($"{_domainPath}/{mediaFolder}");
            }

            /* Resimin yüklenme sırasındaki ilk adı oldFileName adlı değişkene atanır. */
            string oldFileName = Path.GetFileNameWithoutExtension(uploadFile.FileName);

            /* Resimin uzantısı fileExtension adlı değişkene atanır. */
            string fileExtension = Path.GetExtension(uploadFile.FileName);

            Regex regex = new Regex("[*'\",._&#^@]");
            string newFileNameNotExtension = regex.Replace(oldFileName, string.Empty);
            /* Kendi parametrelerimiz ile sistemimize uygun yeni bir dosya yolu (path) oluşturulur. */

            string timeFormat = DateTime.Now.ToString("yyMMddHHmmss");

            string newFileNameAndExtension = UrlExtensions.FriendlySEOUrl(newFileNameNotExtension) + "-" + timeFormat + fileExtension;

            var path = Path.Combine(dosyaYolu, newFileNameAndExtension);

            /* Sistemimiz için oluşturulan yeni dosya yoluna resim kopyalanır. */
            using (var stream = new FileStream(path, FileMode.Create))
            {
                uploadFile.CopyTo(stream);
            }

            int width = 0, height = 0;
            //if (uploadFile.ContentType.Contains("image") && !uploadFile.ContentType.Contains("svg+xml"))
            //{
            //    using (var image = Image.FromStream(uploadFile.OpenReadStream()))
            //    {
            //        width = image.Width;
            //        height = image.Height;
            //    }
            //}

            /* Resim tipine göre kullanıcı için bir mesaj oluşturulur. */
            string nameMessage = $"{newFileNameAndExtension} adlı medya dosyası başarıyla yüklenmiştir.";

            //_toastNotification.AddSuccessToastMessage(nameMessage, new ToastrOptions
            //{
            //    Title = "Başarılı İşlem!"
            //});
            
            return new DataResult<FileUploadDto>(ResultStatus.Success, nameMessage, new FileUploadDto
            {
                FileFullName = newFileNameAndExtension,
                Path = path,
                ContentType = contentType,
                Extension = fileExtension,
                Size = uploadFile.Length,
                Width = width,
                Height = height
            });
        }

        public IDataResult<FileDeleteDto> Delete(string uploadFileName)
        {
            string dosyaYolu = $"{_domainPath}/{mediaFolder}";
            var fileToDelete = Path.Combine(dosyaYolu, uploadFileName);
          
            if (File.Exists(fileToDelete))
            {
                var fileInfo = new FileInfo(fileToDelete);
                
                var imageDeletedDto = new FileDeleteDto
                {
                    FileFullName = uploadFileName,
                    Extension = fileInfo.Extension,
                    Path = fileInfo.FullName,
                    Size = fileInfo.Length,
                };
                File.Delete(fileToDelete);
                return new DataResult<FileDeleteDto>(ResultStatus.Success, imageDeletedDto);
            }
            else
            {
                return new DataResult<FileDeleteDto>(ResultStatus.Error, $"Böyle bir medya dosyası bulunamadı.", null);
            }
        }

        public IDataResult<FileDeleteDto> MultiDelete(string uploadFileName)
        {
            string dosyaYolu = $"{_domainPath}/{mediaFolder}";
            var fileToDelete = Path.Combine(dosyaYolu, uploadFileName);
            if (File.Exists(fileToDelete))
            {
                var fileInfo = new FileInfo(fileToDelete);
                var imageDeletedDto = new FileDeleteDto
                {
                    FileFullName = uploadFileName,
                    Extension = fileInfo.Extension,
                    Path = fileInfo.FullName,
                    Size = fileInfo.Length
                };
                File.Delete(fileToDelete);
                return new DataResult<FileDeleteDto>(ResultStatus.Success, imageDeletedDto);
            }
            else
            {
                return new DataResult<FileDeleteDto>(ResultStatus.Error, $"Böyle bir medya dosyası bulunamadı.", null);
            }
        }
    }
}