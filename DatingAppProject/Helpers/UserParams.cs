namespace DatingAppProject.Helpers;

public class UserParams : PaginationParams {
    public string? UserUsername { get; set; }
    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; } = 100;
}