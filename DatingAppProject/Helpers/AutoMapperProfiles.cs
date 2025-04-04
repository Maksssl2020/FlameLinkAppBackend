using AutoMapper;
using DatingAppProject.DTO;
using DatingAppProject.Entities;
using DatingAppProject.Entities.ImageEntity;
using DatingAppProject.Entities.InterestEntity;
using DatingAppProject.Entities.NewsEntity;
using DatingAppProject.Entities.User;

namespace DatingAppProject.Helpers;

public class AutoMapperProfiles : Profile {

    public AutoMapperProfiles(){
        CreateMap<Interest, InterestDto>();
        CreateMap<RegisterRequestDto, AppUser>()
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateOnly.Parse(src.DateOfBirth)));
        CreateMap<Image, ImageDto>();
        CreateMap<News, NewsDto>();
    }
}