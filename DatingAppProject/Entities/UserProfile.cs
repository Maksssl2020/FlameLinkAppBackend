namespace DatingAppProject.Entities;

public class UserProfile {
    public long Id { get; set; }
    
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
    public Image? MainPhoto { get; set; } = null!;
    
    public long ProfileOwnerId { get; set; }
    public AppUser ProfileOwner { get; set; } = null!;
    
    public List<Image> Photos { get; set; } = [];
    public List<Interest> Interests { get; set; } = [];
}