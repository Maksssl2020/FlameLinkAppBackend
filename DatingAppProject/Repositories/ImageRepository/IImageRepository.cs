using DatingAppProject.Entities.ImageEntity;

namespace DatingAppProject.Repositories.ImageRepository;

public interface IImageRepository {
    Task<Image> SaveImage(IFormFile file);
}