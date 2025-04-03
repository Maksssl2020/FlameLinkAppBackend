using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppProject.Data;
using DatingAppProject.DTO;
using DatingAppProject.Entities.NewsEntity;
using DatingAppProject.Repositories.ImageRepository;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Repositories.NewsRepository;

public class NewsRepository(DataContext dataContext, IMapper mapper, IImageRepository imageRepository) : INewsRepository {

    public async Task SaveNews(NewsRequestDto newsRequestDto){
        var savedImage = await imageRepository.SaveImage(newsRequestDto.CoverImage);

        if (savedImage == null) {
            throw new Exception("Cover image could not be saved.");
        }
        
        var news = new News {
            Title = newsRequestDto.Title,
            Description = newsRequestDto.Description,
            Content = newsRequestDto.Content,
            CoverImageId = savedImage.Id,
            CoverImage = savedImage
        };
        
        dataContext.News.Add(news);
        await dataContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<NewsDto>> GetAllNews(){
        return await dataContext.News
            .Include(news => news.CoverImage)
            .ProjectTo<NewsDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }
}