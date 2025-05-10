using DatingAppProject.DTO;
using DatingAppProject.Repositories.ImageRepository;
using DatingAppProject.Repositories.ProfileRepository;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppProject.Controllers;

[ApiController]
[Route("api/v1/users-profiles")]
public class UserProfilesController(IUserProfileRepository userProfileRepository, IImageRepository imageRepository) : ControllerBase {

    [HttpGet("{ownerId:long}")]
    public async Task<ActionResult<UserProfileDto>> GetUserProfile([FromRoute] long ownerId){
        var userProfile = await userProfileRepository.GetProfileDtoByOwnerId(ownerId);
        if (userProfile == null) {
            return BadRequest("User's profile does not exist.");
        }
        
        return Ok(userProfile);
    }

    [HttpPut("upload-main-photo/{ownerId:long}")]
    public async Task<ActionResult<ImageDto>> UploadMainPhoto([FromRoute] long ownerId, [FromForm] IFormFile file){
        var userProfile = await userProfileRepository.GetProfileByOwnerId(ownerId);
        if (userProfile == null) {
            return BadRequest("User's profile does not exist.");
        }

        var savedImage = await imageRepository.SaveImage(file);
        userProfile.MainPhoto = savedImage;
        userProfile.MainPhotoId = savedImage.Id;

        if (await userProfileRepository.SaveAll()) {
            return Ok(new ImageDto {Id = savedImage.Id, ImageData = savedImage.ImageData});
        }
        
        return BadRequest("Error while saving profile.");
    }
}