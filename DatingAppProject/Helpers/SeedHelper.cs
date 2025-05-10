using System.Text.Json;
using Bogus;
using DatingAppProject.Data;
using DatingAppProject.DTO;
using DatingAppProject.Entities;
using DatingAppProject.Repositories.ImageRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingAppProject.Helpers;

public static class SeedHelper {

    public static async Task SeedInterestsAsync(DataContext dataContext){
        if (await dataContext.Interests.AnyAsync()) return;

        var interests = new[] {
            "Music", "Movies", "Sports", "Gaming", "Travel", "Art", "Cooking", "Dancing", "Fitness",
            "Reading", "Photography", "Hiking", "Yoga", "Writing", "Running", "Cycling", "Board Games",
            "Video Games", "Fashion", "Technology", "Theatre", "Volunteering", "Swimming", "Pets",
            "Languages", "Cars", "Gardening", "Fishing", "Crafts", "Drawing", "Comics", "Meditation",
            "Astronomy", "Investing", "Coffee", "Tea", "Wines", "Beer Tasting", "Shopping", "Podcasts",
            "Blogging", "Painting", "Camping", "Skiing", "Snowboarding", "Surfing", "Skateboarding",
            "Travel Blogging", "Tattoo Art", "DIY Projects"
        };

        foreach (var interestName in interests) {

            if (await dataContext.Interests.AnyAsync(interest => interest.InterestName == interestName)) continue; 
            
            {
                var interest = new Interest { InterestName = interestName };
                await dataContext.Interests.AddAsync(interest);
            }
        }

        await dataContext.SaveChangesAsync();
    }
    
    public static async Task SeedUsersAsync(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        DataContext dataContext
    ){
        
        var genders = new [] {"Male", "Female"};
        var preferences = new[] { "Males", "Females", "Both" };
        var interests = new[] {
            "Music", "Movies", "Sports", "Gaming", "Travel", "Art", "Cooking", "Dancing", "Fitness",
            "Reading", "Photography", "Hiking", "Yoga", "Writing", "Running", "Cycling", "Board Games",
            "Video Games", "Fashion", "Technology", "Theatre", "Volunteering", "Swimming", "Pets",
            "Languages", "Cars", "Gardening", "Fishing", "Crafts", "Drawing", "Comics", "Meditation",
            "Astronomy", "Investing", "Coffee", "Tea", "Wines", "Beer Tasting", "Shopping", "Podcasts",
            "Blogging", "Painting", "Camping", "Skiing", "Snowboarding", "Surfing", "Skateboarding",
            "Travel Blogging", "Tattoo Art", "DIY Projects"
        };
        var lookingForOptions = new [] {"Friends", "Serious relationship", "Fun with people", "I don't know yet"};

        var faker = new Faker<RegisterRequestDto>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, _ => "Pa$$w0rd")
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Gender, f => f.PickRandom(genders))
            .RuleFor(u => u.City, f => f.Address.City())
            .RuleFor(u => u.Country, f => f.Address.Country())
            .RuleFor(u => u.Preference, f => f.PickRandom(preferences))
            .RuleFor(u => u.DateOfBirth, f => f.Date.Past(50, DateTime.Today.AddYears(-18)).ToString("yyyy-MM-dd"))
            .RuleFor(u => u.LookingFor, f => f.PickRandom(lookingForOptions))
            .RuleFor(u => u.Interests, f => f.PickRandom(interests, 10).ToList());

        var fakeUsers = faker.Generate(10);
        var client = new HttpClient();

        foreach (var fake in fakeUsers) {
            var user = new AppUser {
                UserName = fake.Username,
                Email = fake.Email,
                FirstName = fake.FirstName,
                LastName = fake.LastName,
                Gender = fake.Gender,
                City = fake.City,
                Country = fake.Country,
                Preference = fake.Preference,
                DateOfBirth = DateOnly.Parse(fake.DateOfBirth)
            };

            Console.WriteLine($"Creating user {fake.Username}...");
            var response = await client.GetStringAsync("https://randomuser.me/api/");
            var json = JsonDocument.Parse(response);
            var photoUrl = json.RootElement
                .GetProperty("results")[0]
                .GetProperty("picture")
                .GetProperty("large")
                .GetString();
            var imageBytes = await client.GetByteArrayAsync(photoUrl);
            
            var result = await userManager.CreateAsync(user, fake.Password);
            if (!result.Succeeded) {
                Console.WriteLine($"❌ Failed to create user {user.UserName}:");
                foreach (var error in result.Errors) {
                    Console.WriteLine($"   - {error.Description}");
                }
                continue;
            }

            await userManager.AddToRoleAsync(user, "User");
            
            var profile = new UserProfile {
                DisplayName = $"{user.FirstName} {user.LastName}",
                Gender = user.Gender,
                Preference = user.Preference,
                City = user.City,
                Country = user.Country,
                Age = CalculateAge.CalculateAgeFromDob(user.DateOfBirth),
                LookingFor = fake.LookingFor,
                Bio = "",
                ProfileOwner = user,
                Interests = [],
                Photos = [],
            };
  
            
            foreach (var interestName in fake.Interests.Distinct()) {
                var interest = await dataContext.Interests
                    .FirstOrDefaultAsync(i => i.InterestName == interestName);

                if (interest != null)
                    profile.Interests.Add(interest);
            }

            await dataContext.UserProfiles.AddAsync(profile);
            
            var mainPhoto = new Image {ImageData = imageBytes};
            await dataContext.Images.AddAsync(mainPhoto);
            await dataContext.SaveChangesAsync();
            
            profile.MainPhoto = mainPhoto;
            profile.MainPhotoId = mainPhoto.Id;
            user.UserProfile = profile;
        }
        
        await dataContext.SaveChangesAsync();
        Console.WriteLine("✅ Saved all profiles to database.");
    }
}