using DatingAppProject.Entities;

namespace DatingAppProject.DTO;

public class UserDto {

    public long Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Gender { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public required string Preference { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public List<InterestDto> Interests { get; set; } = [];
    public Image MainPhoto { get; set; } = null!;
}