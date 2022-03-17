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
using VueJS.Services.Abstract;
using AutoMapper;
using VueJS.Services.Helper.Abstract;

namespace VueJS.Mvc.Helpers.Concrete
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _domainPath;
        private readonly IUploadService _uploadService;
        private readonly IMapper _mapper;
        private readonly IExtensionsHelper _extensionsHelper;
        private const string mediaFolder = "ClientApp/src/assets/images/media";

        public ImageHelper(IWebHostEnvironment env, IUploadService uploadService, IMapper mapper, IExtensionsHelper extensionsHelper)
        {
            _env = env;
            _uploadService = uploadService;
            _mapper = mapper;
            _extensionsHelper = extensionsHelper;
            _domainPath = _env.ContentRootPath;
        }

        public async Task<IDataResult<UploadDto>> Upload(IFormFile file, int userId)
        {
            string dosyaYolu = $"{_domainPath}/{mediaFolder}";
            if (!Directory.Exists(dosyaYolu))
            {
                Directory.CreateDirectory($"{_domainPath}/{mediaFolder}");
            }

            if (file != null)
            {
                string contentType = file.ContentType;

                /* Resimin yüklenme sırasındaki ilk adı oldFileName adlı değişkene atanır. */
                string oldFileName = Path.GetFileNameWithoutExtension(file.FileName);

                /* Resimin uzantısı fileExtension adlı değişkene atanır. */
                string fileExtension = Path.GetExtension(file.FileName);

                Regex regex = new Regex("[*'\",._&#^@]");
                string newFileNameNotExtension = regex.Replace(oldFileName, string.Empty);
                /* Kendi parametrelerimiz ile sistemimize uygun yeni bir dosya yolu (path) oluşturulur. */

                string timeFormat = DateTime.Now.ToString("yyMMddHHmmss");

                string newFileNameAndExtension = _extensionsHelper.FriendlySEOPostName(newFileNameNotExtension) + "-" + timeFormat + fileExtension;

                var path = Path.Combine(dosyaYolu, newFileNameAndExtension);

                /* Sistemimiz için oluşturulan yeni dosya yoluna resim kopyalanır. */
                using (FileStream stream = File.Create(path))
                {
                    await file.CopyToAsync(stream);
                    await stream.FlushAsync();
                }

                int width = 0, height = 0;
                if (file.ContentType.Contains("image") && !file.ContentType.Contains("svg+xml"))
                {
                    using var image = Image.FromStream(file.OpenReadStream());
                    width = image.Width;
                    height = image.Height;
                }

                var uploadAddDto = _mapper.Map<UploadAddDto>(file);
                uploadAddDto.FileName = newFileNameAndExtension;
                uploadAddDto.AltText = newFileNameAndExtension;
                uploadAddDto.ContentType = contentType;
                uploadAddDto.Size = file.Length;
                uploadAddDto.Width = width;
                uploadAddDto.Height = height;

                var uploadDto = await _uploadService.AddAsync(uploadAddDto, userId);

                string nameMessage = $"{newFileNameAndExtension} adlı medya dosyası başarıyla yüklenmiştir.";
                return new DataResult<UploadDto>(ResultStatus.Success, nameMessage, new UploadDto { Upload = uploadDto.Data.Upload });
            }
            else
            {
                return new DataResult<UploadDto>(ResultStatus.Error, "Dosya eklenirken hata oluştu.", new UploadDto { Upload = null });
            }
        }

        public async Task<IDataResult<FileDeleteDto>> Delete(int uploadId)
        {
            var upload = await _uploadService.GetAsync(uploadId);
            string dosyaYolu = $"{_domainPath}/{mediaFolder}";
            var fileToDelete = Path.Combine(dosyaYolu, upload.Data.Upload.FileName);

            if (File.Exists(fileToDelete))
            {
                var fileInfo = new FileInfo(fileToDelete);

                var imageDeletedDto = new FileDeleteDto
                {
                    FileFullName = upload.Data.Upload.FileName,
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