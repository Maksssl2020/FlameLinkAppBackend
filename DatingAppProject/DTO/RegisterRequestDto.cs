namespace DatingAppProject.DTO;

public class RegisterRequestDto {
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string DateOfBirth { get; set; }
    public required string Gender { get; set; }
    public required string Preference { get; set; }
    public required string Country { get; set; }
    public required string City { get; set; }
    public List<string> Interests { get; set; } = [];
}