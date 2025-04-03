namespace DatingAppProject.Entities.ImageEntity;

public class Image {
    public long Id { get; set; }
    public required byte[] ImageData { get; set; }
}