using AutoMapper;
using WeightManager.Model.DTOs;
using WeightManager.Model.Models;

namespace WeightManager.Model;

public class MapperConfig : Profile
{
    public MapperConfig() 
    {
        CreateMap<WeightEntry, WeightEntryResponse>();
        CreateMap<WeightEntryRequest, WeightEntry>();
        CreateMap<User, UserResponse>();
        CreateMap<UserRequest, User>();
    }
}
