using System.Linq;
using api_dating_app.DTOs;
using api_dating_app.models;
using AutoMapper;

namespace api_dating_app.Helpers
{
    /// <summary>
    /// Helper class used to manage the custom parameter mappings
    /// for <see cref="Mapper"/>.
    /// </summary>
    public class AutoMapperProfilesHelper : Profile
    {
        /// <summary>
        /// Construotor. Contains all specific mapper logic to convert
        /// the models to DTOs.
        /// </summary>
        public AutoMapperProfilesHelper()
        {
            CreateMap<UserModel, UserForListDto>()
                // Map the photo url
                .ForMember(destination => destination.PhotoUrl,
                    option => { option.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); }
                )
                // Map and calculate the age
                .ForMember(destination => destination.Age,
                    option => { option.ResolveUsing(d => d.DateOfBirth.CalculateAge()); }
                );

            CreateMap<UserModel, UserForDetailDto>()
                // Map the photo url
                .ForMember(destination => destination.PhotoUrl,
                    option => { option.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); }
                )
                // Map and calculate the age
                .ForMember(destination => destination.Age,
                    option => { option.ResolveUsing(d => d.DateOfBirth.CalculateAge()); }
                );

            CreateMap<PhotoModel, PhotoForDetailDto>();
        }
    }
}