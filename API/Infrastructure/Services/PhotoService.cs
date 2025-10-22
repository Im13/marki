using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            var acc = new Account
            (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResults = new ImageUploadResult();

            if(file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation()
                        .Height(500)
                        .Width(500)
                        .Crop("fill")
                        .Quality("auto:good") // Chất lượng tốt cho bản free
                        .FetchFormat("auto"), // Tự động chọn format tối ưu
                    Folder = "marki"
                };

                uploadResults = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResults;
        }
        
        public async Task<ImageUploadResult> AddBannerPhotoAsync(IFormFile file)
        {
            var uploadResults = new ImageUploadResult();

            if(file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation()
                        .Width(1200)  // Banner width chuẩn
                        .Height(700)  // Banner height chuẩn (tỷ lệ 2:1)
                        .Crop("fill") // Fill toàn bộ kích thước, crop nếu cần
                        .Gravity("center") // Căn giữa khi crop
                        .Quality("auto:best") // Chất lượng tốt nhất cho bản free
                        .FetchFormat("auto"), // Tự động chọn format tối ưu
                    Folder = "marki/banners"
                };

                uploadResults = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResults;
        }

        public async Task<Photo> CreatePhotoAsync(Photo photo)
        {
            _unitOfWork.Repository<Photo>().Add(photo);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            return photo;
        }

        public async Task<ImageUploadResult> AddBannerPhotoWithResponsiveAsync(IFormFile file)
        {
            var uploadResults = new ImageUploadResult();

            if(file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation()
                        .Width(1200)  // Desktop width
                        .Height(700)  // Desktop height (tỷ lệ 2:1)
                        .Crop("fill")
                        .Gravity("center")
                        .Quality("auto:good") // Chất lượng tốt cho bản free
                        .FetchFormat("auto"), // Tự động chọn format tối ưu
                    Folder = "marki/banners/desktop"
                };

                uploadResults = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResults;
        }

        public async Task<ImageUploadResult> AddBannerMobilePhotoAsync(IFormFile file)
        {
            var uploadResults = new ImageUploadResult();

            if(file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation()
                        .Width(768)   // Mobile width
                        .Height(384)  // Mobile height (tỷ lệ 2:1)
                        .Crop("fill")
                        .Gravity("center")
                        .Quality("auto:good") // Chất lượng tốt cho bản free
                        .FetchFormat("auto"), // Tự động chọn format tối ưu
                    Folder = "marki/banners/mobile"
                };

                uploadResults = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResults;
        }

        public async Task<ImageUploadResult> AddOriginalPhotoAsync(IFormFile file)
        {
            var uploadResults = new ImageUploadResult();

            if(file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation()
                        .Quality("auto:best") // Chất lượng cao nhất cho bản free
                        .FetchFormat("auto"), // Tự động chọn format tối ưu
                    Folder = "marki/original"
                };

                uploadResults = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResults;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}