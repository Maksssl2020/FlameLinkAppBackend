namespace DatingAppProject.DTO;

public class RegisterAdminRequestDto {
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
}