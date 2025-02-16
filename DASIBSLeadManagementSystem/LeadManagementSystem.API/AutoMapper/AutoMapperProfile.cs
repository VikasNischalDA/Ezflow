using AutoMapper;
using LeadManagementSystem.Contracts.Request.LeadSource;
using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.Model.Models;
using LeadManagementSystem.Shared.Contracts;
using LeadManagementSystem.Shared.Contracts.Request;


namespace LeadManagementSystem.API.AutoMapper
{
    /// <summary>
    /// 
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<LeadModel, Contracts.Request.LeadRequest>();
            CreateMap<LeadRequest, LeadModel>();
            CreateMap<LeadStatusUpdateRequest, LeadStatusModel>();
            CreateMap<Shared.Contracts.Request.LeadRequest, Contracts.Request.LeadRequest>();
            CreateMap<Shared.Contracts.Request.LeadStatusUpdateRequest, Contracts.Request.LeadStatusUpdateRequest>();
            CreateMap<Shared.Contracts.Request.LeadRequest, Contracts.Request.LeadRequest>();
            CreateMap<Contracts.Response.LeadResponse, LeadResponseModel>();
            CreateMap<LeadSourceModel,Shared.Contracts.Response.LeadSourceResponse>();
            CreateMap<Shared.Contracts.Response.LeadSourceResponse, LeadSourceModel>();
                //.ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.BrandModel));
            CreateMap<BrandModel, BrandResponse>();
            CreateMap<Shared.Contracts.Request.LeadRecordRequest, Contracts.Request.LeadRecordRequest>();
            CreateMap<LeadSourceRequest,LeadSourceModel>();
            CreateMap<Shared.Contracts.Request.LoginRequest, Contracts.Request.LoginRequest>();
            //CreateMap<Shared.Contracts.Response.LoginResponse, Contracts.Response.LoginResponse>();

            CreateMap<GetCustomerStatusResult, GetCustomerStatusResponseModel>()
            .ForMember(dest => dest.Result, opt => opt.MapFrom(src => src.Result))
            .ForMember(dest => dest.MessageCode, opt => opt.MapFrom(src => src.MessageCode))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
            .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.CustomerStatus.CustomerBrandStatus.BrandId))
            .ForMember(dest => dest.IsRepeat, opt => opt.MapFrom(src => src.CustomerStatus.CustomerBrandStatus.IsRepeat))
            .ForMember(dest => dest.HasApplication, opt => opt.MapFrom(src => src.CustomerStatus.CustomerBrandStatus.HasApplication))
            .ForMember(dest => dest.ApplicationStatus, opt => opt.MapFrom(src => src.CustomerStatus.CustomerBrandStatus.ApplicationStatus))
            .ForMember(dest => dest.ApplicationSubStatus, opt => opt.MapFrom(src => src.CustomerStatus.CustomerBrandStatus.ApplicationSubStatus))
            .ForMember(dest => dest.IsRepeatExpired, opt => opt.MapFrom(src => src.CustomerStatus.CustomerBrandStatus.IsRepeatExpired));
        }
    }
}
