using System.Linq;
using api_dating_app.DTOs;
using api_dating_app.models;
using AutoMapper;

namespace api_dating_app.Helpers
{
    /// <summary>
    /// Author: Petar Krastev
    /// </summary>
    public class AutoMapperProfilesHelper : Profile
    {
        public AutoMapperProfilesHelper()
        {
            CreateMap<UserModel, UserForListDto>()
                .ForMember(destination => destination.PhotoUrl,
                    option => { option.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); }
                )
                .ForMember(destination => destination.Age,
                    option => { option.ResolveUsing(d => d.DateOfBirth.CalculateAge()); }
                );

            CreateMap<UserModel, UserForDetailDto>()
                .ForMember(destination => destination.PhotoUrl,
                    option => { option.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); }
                )
                .ForMember(destination => destination.Age,
                    option => { option.ResolveUsing(d => d.DateOfBirth.CalculateAge()); }
                );

            CreateMap<PhotoModel, PhotoForDetailDto>();

            CreateMap<UserForUpdateDto, UserModel>();

            CreateMap<PhotoForCreationDto, PhotoModel>();

            CreateMap<PhotoModel, PhotoForReturnDto>();

            CreateMap<UserForRegisterDto, UserModel>();

            CreateMap<MessageForCreationDto, MessageModel>().ReverseMap();

            CreateMap<MessageModel, MessageForReturnDto>()
                .ForMember(m => m.SenderKnownAs, opt => opt.MapFrom(u => u.Sender.KnownAs))
                .ForMember(m => m.RecipientKnowAs, opt => opt.MapFrom(u => u.Recipient.KnownAs))
                .ForMember(m => m.SenderPhotoUrl,
                    opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(m => m.RecipientPhotoUrl,
                    opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));
        }
    }
}