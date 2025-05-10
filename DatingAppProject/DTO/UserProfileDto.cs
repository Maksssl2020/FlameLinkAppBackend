namespace DatingAppProject.DTO;

public class UserProfileDto {
    public long Id { get; set; }
    public long OwnerId { get; set; }
    
    public required string DisplayName { get; set; }
    public required string Gender { get; set; }
    public required string Preference { get; set; }
    public required string LookingFor { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    
    public int Age { get; set; }
    public string? Bio { get; set; }
    
    public bool ShowGender { get; set; } = true;
    public bool ShowPreference { get; set; } = true;
    public bool ShowCity { get; set; }  = true;
    public bool ShowCountry { get; set; }  = true;

    public long? MainPhotoId { get; set; }
    public ImageDto? MainPhoto { get; set; }
    
    public List<ImageDto> Photos { get; set; } = [];
    public List<InterestDto> Interests { get; set; } = [];
}