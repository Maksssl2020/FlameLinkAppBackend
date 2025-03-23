namespace DatingAppProject.DTO;

public class AuthenticationDto {

    public long UserId { get; set; }
    public required string Username { get; set; }
    public string? Email { get; set; }
    public required string AccessToken { get; set; }
}