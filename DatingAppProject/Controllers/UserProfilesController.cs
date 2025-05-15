using DatingAppProject.DTO;
using DatingAppProject.Repositories.ImageRepository;
using DatingAppProject.Repositories.InterestRepository;
using DatingAppProject.Repositories.ProfileRepository;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppProject.Controllers;

[ApiController]
[Route("api/v1/users-profiles")]
public class UserProfilesController(IUserProfileRepository userProfileRepository, IImageRepository imageRepository, IInterestRepository interestRepository) : ControllerBase {

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

    [HttpPut("upload-photo-to-gallery/{ownerId:long}")]
    public async Task<ActionResult<ImageDto>> UploadPhotoToGallery([FromRoute] long ownerId, [FromForm] IFormFile file) {
        var userProfile = await userProfileRepository.GetProfileByOwnerId(ownerId);
        if (userProfile == null) {
            return BadRequest("User's profile does not exist.");
        }

        if (userProfile.Photos.Count >= 6) {
            return BadRequest("You reached the maximum number of photos in profile.");
        }
        
        var savedImage = await imageRepository.SaveImage(file);
        userProfile.Photos.Add(savedImage);
        
        if (await userProfileRepository.SaveAll()) {
            return Ok(new ImageDto {Id = savedImage.Id, ImageData = savedImage.ImageData});
        }
        
        return BadRequest("Error while saving profile.");
    }

    [HttpPatch("update-profile/{ownerId:long}")]
    public async Task<ActionResult> UpdateProfile([FromRoute] long ownerId,
        [FromForm] UserProfileUpdateRequest userProfileUpdate) {
        var userProfile = await userProfileRepository.GetProfileByOwnerId(ownerId);
        if (userProfile == null) {
            return BadRequest("User's profile does not exist.");
        }

        if (userProfileUpdate.Bio != null) {
            userProfile.Bio = userProfileUpdate.Bio;
        }

        if (userProfileUpdate.LookingFor != null) {
            userProfile.LookingFor = userProfileUpdate.LookingFor;
        }

        if (userProfileUpdate.Interests.Count > 0) {
            var foundInterest = await interestRepository.GetAllByNames(userProfileUpdate.Interests);

            if (foundInterest.Count > 0) {
                userProfile.ProfileOwner.Interests.Clear();

                foreach (var interest in foundInterest) {
                    userProfile.ProfileOwner.Interests.Add(interest);
                }
            }
        }

        await userProfileRepository.SaveAll();
        return NoContent();
    }
 }