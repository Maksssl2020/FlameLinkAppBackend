using AutoMapper;
using DatingAppProject.DTO;
using DatingAppProject.Entities;
using DatingAppProject.Entities.InterestEntity;

namespace DatingAppProject.Helpers;

public class AutoMapperProfiles : Profile {

    public AutoMapperProfiles(){
        CreateMap<Interest, InterestDto>();
        CreateMap<RegisterRequestDto, AppUser>();
    }
}