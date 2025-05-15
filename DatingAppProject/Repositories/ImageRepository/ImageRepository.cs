using AutoMapper;
using DatingAppProject.Data;
using DatingAppProject.Entities;

namespace DatingAppProject.Repositories.ImageRepository;

public class ImageRepository(DataContext dataContext) : IImageRepository {

    public async Task<Image> SaveImage(IFormFile file){
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        var image = new Image {
            ImageData = memoryStream.ToArray(),
        };
        
        await dataContext.Images.AddAsync(image);
        await dataContext.SaveChangesAsync();
        
        return image;
    }

    public async Task<List<Image>> SaveImages(IFormFile[] files) {
        List<Image> images = [];
        using var memoryStream = new MemoryStream();

        foreach (var file in files) {
            await file.CopyToAsync(memoryStream);
            var image = new Image {
                ImageData = memoryStream.ToArray(),
            };
            
            images.Add(image);
        }
        
        await dataContext.Images.AddRangeAsync(images);
        await dataContext.SaveChangesAsync();

        return images;
    }
}