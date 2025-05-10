using DatingAppProject.DTO;

namespace DatingAppProject.Helpers;

public class CalculateAge {
    public static int CalculateAgeFromDob(DateOnly dob){
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - dob.Year;
        return age;
    }
}