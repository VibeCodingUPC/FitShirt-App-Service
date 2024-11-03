using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FitShirt.Domain.Shared.Models.ImageCloudinary;
using FitShirt.Domain.Shared.Services.ImageCloudinary;
using Microsoft.Extensions.Options;

namespace FitShirt.Infrastructure.Shared.ImageCloudinary.Services;

public class ManageImageService : IManageImageService
{
    public CloudinarySettings _cloudinarySettings { get; }

    public ManageImageService(IOptions<CloudinarySettings> cloudinarySettings)
    {
        _cloudinarySettings = cloudinarySettings.Value;
    }

    public async Task<ImageResponse> UploadImage(ImageData imageStream)
    {
        var account = new Account(
            _cloudinarySettings.CloudName,
            _cloudinarySettings.ApiKey,
            _cloudinarySettings.ApiSecret
        );
        
        var cloudinary = new Cloudinary(account);

        var uploadImage = new ImageUploadParams()
        {
            File = new FileDescription(imageStream.Name, imageStream.ImageStream)
        };

        var uploadResult = await cloudinary.UploadAsync(uploadImage);

        if(uploadResult.StatusCode == HttpStatusCode.OK)
        {
            return new ImageResponse {
                PublicCode = uploadResult.PublicId,
                Url = uploadResult.Url.ToString()
            };
        }

        throw new Exception("No se pudo guardar la imagen");
    }
}