using AutoMapper;
using DatingAppProject.Data;
using DatingAppProject.Entities.ImageEntity;

namespace DatingAppProject.Repositories.ImageRepository;

public class ImageRepository(DataContext dataContext, IMapper mapper) : IImageRepository {

    public async Task<Image> SaveImage(IFormFile file){
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        var image = new Image {
            ImageData = memoryStream.ToArray(),
        };
        
        dataContext.Images.Add(image);
        await dataContext.SaveChangesAsync();
        
        return image;
    }
}