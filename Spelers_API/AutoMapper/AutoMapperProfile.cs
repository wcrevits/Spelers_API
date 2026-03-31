using AutoMapper;
using Spelers_API.Domain.EntitiesDB;
using Spelers_API.ViewModels;

namespace EmployeeAPI.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Entity -> VM (For GET)
            CreateMap<Speler, SpelerVM>()
                .ForMember(dest => dest.TeamNaam, opt => opt.MapFrom(src => src.Team.Naam))
                .ForMember(dest => dest.PositieNaam, opt => opt.MapFrom(src => src.Positie.Naam));

            // VM -> Entity (For POST)
            CreateMap<SpelerPostVM, Speler>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // The DB handles the ID
                .ForMember(dest => dest.Naam, opt => opt.MapFrom(src => src.Naam)); // Force the Naam mapping
        }
    }
}

