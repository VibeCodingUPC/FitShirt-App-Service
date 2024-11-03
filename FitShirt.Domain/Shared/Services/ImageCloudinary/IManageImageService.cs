using FitShirt.Domain.Shared.Models.ImageCloudinary;

namespace FitShirt.Domain.Shared.Services.ImageCloudinary;

public interface IManageImageService
{
    Task<ImageResponse> UploadImage(ImageData imageStream);
}