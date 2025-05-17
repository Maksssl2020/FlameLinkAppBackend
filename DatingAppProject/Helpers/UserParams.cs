using DatingAppProject.Entities;

namespace DatingAppProject.Helpers;

public class UserParams : PaginationParams {
    public string? UserUsername { get; set; }
    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; } = 100;
    public UserParamsImagesOptions UserParamsImagesOptions { get; set; }
    public List<string> Interests { get; set; } = [];
}