using DatingAppProject.DTO;
using DatingAppProject.Entities.InterestEntity;
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

    [HttpPost]
    public async Task<ActionResult> CreateInterest(InterestRequestDto interestRequest){
        try {
            await interestRepository.SaveInterest(interestRequest);

            if (await interestRepository.SaveChanges()) {
                return Ok("Interest created.");
            }

            return BadRequest("Cannot create interest.");
        }
        catch (InterestException interestException) {
            return BadRequest(new { message = interestException.Message });
        }
        catch (Exception exception) {
            return StatusCode(500, new { message = "An unexpected error has occurred. Please try again later." });
        }
    }
}