using AutoMapper;
using Beauty4u.Models.Api.Promotions;
using Beauty4u.Models.Dto.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Profiles
{
    public class PromotionProfile : Profile
    {
        public PromotionProfile()
        {
            CreateMap<PromotionDto, PromotionRequest>()
                .ForMember(dest => dest.PromoRate,
                            opt => opt.MapFrom(src => src.DC))
                        .ForMember(dest => dest.PromoDate,
                            opt => opt.MapFrom(src => DateTime.Parse(src.PromoDate)))
                        .ForMember(dest => dest.PromoStatus,
                            opt => opt.MapFrom(src => src.Status))
                        .ForMember(dest => dest.FromDate,
                            opt => opt.MapFrom(src => DateTime.Parse(src.StartDate)))
                        .ForMember(dest => dest.ToDate,
                            opt => opt.MapFrom(src => DateTime.Parse(src.EndDate)))
                        .ForMember(dest => dest.CurrentUser,
                            opt => opt.MapFrom(src => src.LastUser ?? src.WriteUser));
        }
    }
}
