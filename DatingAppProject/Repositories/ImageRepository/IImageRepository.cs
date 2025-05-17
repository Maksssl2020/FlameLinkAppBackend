using DatingAppProject.Entities;

namespace DatingAppProject.Repositories.ImageRepository;

public interface IImageRepository {
    Task<Image?> GetImageByIdAsync(long imageId);
    Task<Image> SaveImageAsync(IFormFile file);
    Task<List<Image>> SaveImagesAsync(IFormFile[] files);
    void RemoveImage(Image image);
    
    Task<bool> SaveChangesAsync();
}