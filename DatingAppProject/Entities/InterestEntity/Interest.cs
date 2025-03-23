using System.ComponentModel.DataAnnotations;

namespace DatingAppProject.Entities.InterestEntity;

public class Interest {

    public long Id { get; set; }
    public required string InterestName { get; set; }
}