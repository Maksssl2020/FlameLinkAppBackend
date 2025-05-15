using AutoMapper;
using DatingAppProject.DTO;
using DatingAppProject.Entities;


namespace DatingAppProject.Helpers;

public class AutoMapperProfiles : Profile {

    public AutoMapperProfiles(){
        CreateMap<Interest, InterestDto>();
        CreateMap<RegisterRequestDto, AppUser>()
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateOnly.Parse(src.DateOfBirth)));
        CreateMap<Image, ImageDto>();
        CreateMap<AppUser, UserDto>()
            .ForMember(dest => dest.Interests, opt => opt.MapFrom(src => src.Interests))
            .ForMember(dest => dest.MainPhoto, opt => opt.MapFrom(src => src.UserProfile.MainPhoto));
        CreateMap<AppUser, UserProfile>()
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => CalculateAge.CalculateAgeFromDob(src.DateOfBirth)));
        CreateMap<UserProfile, UserProfileDto>();
        CreateMap<ForumPost, ForumPostDto>();
        CreateMap<Message, MessageDto>();
    }
}