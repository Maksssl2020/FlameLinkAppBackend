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

        var interestNames  = new[] {
            "Music", "Movies", "Sports", "Gaming", "Travel", "Art", "Cooking", "Dancing", "Fitness",
            "Reading", "Photography", "Hiking", "Yoga", "Writing", "Running", "Cycling", "Board Games",
            "Video Games", "Fashion", "Technology", "Theatre", "Volunteering", "Swimming", "Pets",
            "Languages", "Cars", "Gardening", "Fishing", "Crafts", "Drawing", "Comics", "Meditation",
            "Astronomy", "Investing", "Coffee", "Tea", "Wines", "Beer Tasting", "Shopping", "Podcasts",
            "Blogging", "Painting", "Camping", "Skiing", "Snowboarding", "Surfing", "Skateboarding",
            "Travel Blogging", "Tattoo Art", "DIY Projects"
        };


        var interests = interestNames.Select(name => new Interest { InterestName = name });
        await dataContext.Interests.AddRangeAsync(interests);
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
            .RuleFor(u => u.Interests, f => f.PickRandom(interests, Random.Shared.Next(3, 11)).ToList());

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
            
            var trackedUser = await dataContext.Users
                .Include(u => u.Interests)
                .Include(u => u.UserProfile)
                .FirstAsync(u => u.UserName == user.UserName);

            List<Interest> createdInterests = [];
            
            foreach (var fakeInterest in fake.Interests) {
                var foundInterest = await dataContext.Interests.FirstOrDefaultAsync(i => i.InterestName == fakeInterest);
                
                if (foundInterest != null) {
                    createdInterests.Add(foundInterest);
                }
                
                
            }
            
            var profile = new UserProfile {
                DisplayName = $"{user.FirstName} {user.LastName}",
                Gender = user.Gender,
                Preference = user.Preference,
                City = user.City,
                Country = user.Country,
                Age = CalculateAge.CalculateAgeFromDob(user.DateOfBirth),
                LookingFor = fake.LookingFor,
                Bio = new Faker().Lorem.Sentences(3),
                ProfileOwner = trackedUser,
                Photos = [],
            };
            
            await dataContext.UserProfiles.AddAsync(profile);
            
            
            var mainPhoto = new Image {ImageData = imageBytes};
            await dataContext.Images.AddAsync(mainPhoto);
            await dataContext.SaveChangesAsync();
            
            profile.MainPhoto = mainPhoto;
            profile.MainPhotoId = mainPhoto.Id;
            
            trackedUser.UserProfile = profile;
            trackedUser.ProfileId = profile.Id;
            
            trackedUser.Interests = createdInterests;
        }
        
        await dataContext.SaveChangesAsync();
        Console.WriteLine("✅ Saved all profiles to database.");
    }
}