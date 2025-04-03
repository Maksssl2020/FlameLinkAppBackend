using DatingAppProject.DTO;
using DatingAppProject.Exceptions;
using DatingAppProject.Repositories.NewsRepository;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppProject.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class NewsController(INewsRepository newsRepository) : ControllerBase {

    [HttpGet]
    public async Task<ActionResult<List<NewsDto>>> GetAllNews(){
        var news = await newsRepository.GetAllNews();
        return Ok(news);
    }

    [HttpPost]
    public async Task<ActionResult> CreateNews([FromForm] NewsRequestDto newsRequestDto){
        try {
            await newsRepository.SaveNews(newsRequestDto);
            return Ok(new {
                message = "News created successfully!"
            });
        }
        catch (Exception exception) {
            return BadRequest(new ApiException(BadRequest().StatusCode, exception.Message, exception.StackTrace));
        }
    }
}