using AutoMapper;
using DatingAppProject.Data;
using DatingAppProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Repositories.ImageRepository;

public class ImageRepository(DataContext dataContext) : IImageRepository {
    public async Task<Image?> GetImageByIdAsync(long imageId) {
        return await dataContext.Images
            .Where(i => i.Id == imageId)
            .FirstOrDefaultAsync();
    }

    public async Task<Image> SaveImageAsync(IFormFile file){
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        var image = new Image {
            ImageData = memoryStream.ToArray(),
        };
        
        await dataContext.Images.AddAsync(image);
        await dataContext.SaveChangesAsync();
        
        return image;
    }

    public async Task<List<Image>> SaveImagesAsync(IFormFile[] files) {
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

    public void RemoveImage(Image image) {
        dataContext.Images.Remove(image);
    }

    public async Task<bool> SaveChangesAsync() {
        return await dataContext.SaveChangesAsync() > 0;
    }
}