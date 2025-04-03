using DatingAppProject.DTO;
using DatingAppProject.Entities.NewsEntity;

namespace DatingAppProject.Repositories.NewsRepository;

public interface INewsRepository {

    Task SaveNews(NewsRequestDto newsRequestDto);
    Task<IEnumerable<NewsDto>> GetAllNews();
}