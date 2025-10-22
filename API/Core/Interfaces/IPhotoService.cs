using CloudinaryDotNet.Actions;
using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<ImageUploadResult> AddBannerPhotoAsync(IFormFile file);
        Task<ImageUploadResult> AddBannerPhotoWithResponsiveAsync(IFormFile file);
        Task<ImageUploadResult> AddBannerMobilePhotoAsync(IFormFile file);
        Task<ImageUploadResult> AddOriginalPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
        Task<Photo> CreatePhotoAsync(Photo photo);
    }
}