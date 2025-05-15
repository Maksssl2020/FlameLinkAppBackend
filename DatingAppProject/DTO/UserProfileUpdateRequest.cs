namespace DatingAppProject.DTO;

public class UserProfileUpdateRequest {
    public string? Bio { get; set; }
    public string? LookingFor { get; set; }
    public List<string> Interests { get; set; } = [];

}