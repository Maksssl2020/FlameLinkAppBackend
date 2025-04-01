using DatingAppProject.DTO;
using DatingAppProject.Exceptions;
using DatingAppProject.Repositories.InterestRepository;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppProject.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class InterestController(IInterestRepository interestRepository) : ControllerBase {

    [HttpGet]
    public async Task<ActionResult<List<InterestDto>>> GetInterests(){
        var interests = await interestRepository.FindAllInterests();
        return Ok(interests);
    }

    [HttpPost("create")]
    // [Authorize(Policy = "RequireAdminRole")]
    public async Task<ActionResult> CreateInterest(InterestRequestDto interestRequest){
        Console.WriteLine("User Claims: ");
        foreach (var claim in User.Claims) {
            Console.WriteLine(claim.Type + ": " + claim.Value);
        }
        
        try {
            await interestRepository.SaveInterest(interestRequest);

            if (await interestRepository.SaveChanges()) {
                return Ok("Interest created.");
            }

            return BadRequest("Cannot create interest.");
        }
        catch (Exception exception) {
            return BadRequest(new ApiException(BadRequest().StatusCode, exception.Message, exception.StackTrace));
        }
    }
}