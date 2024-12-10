using AutoMapper;
using CMS.Areas.Customer.Models;
using CMS.Services.CommonServices;
using DTO.Customer;
using DTO;
using System;
using static CMS.Services.ExtensionServices;
namespace CMS.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>()
                 .ForMember(c => c.PicThumb, opt => opt.MapFrom(x => ConstantStrings.UrlCustomerImages + x.PicThumb));


            CreateMap<CustomerDto, Customer>()
                .ForMember(c => c.Enabled, opt => opt.MapFrom(x => false))
                .ForMember(c => c.PicThumb, opt => opt.MapFrom(x => ConstantStrings.DefaultAvatar))
                .ForMember(c => c.CreateDate, opt => opt.MapFrom(x => DateTime.Now))
                .ForMember(c => c.LastName, opt => opt.MapFrom(x => RemoveHtmlTag(x.LastName)))
                .ForMember(c => c.FirstName, opt => opt.MapFrom(x => RemoveHtmlTag(x.FirstName)))
                .ForMember(c => c.Email, opt => opt.MapFrom(x => RemoveHtmlTag(x.Email)))
                .ForMember(c => c.PhoneNumber, opt => opt.MapFrom(x => RemoveHtmlTag(x.PhoneNumber)))
                .ForMember(c => c.DateOfBirth, opt => opt.MapFrom(x => RemoveHtmlTag(x.DateOfBirth)))
                .ForMember(c => c.MonthOfBirth, opt => opt.MapFrom(x => RemoveHtmlTag(x.MonthOfBirth)))
                .ForMember(c => c.YearOfBirth, opt => opt.MapFrom(x => RemoveHtmlTag(x.YearOfBirth)))
                .ForMember(c => c.Address, opt => opt.MapFrom(x => RemoveHtmlTag(x.Address)))
                .ForMember(c => c.Ward, opt => opt.MapFrom(x => RemoveHtmlTag(x.Ward)))
                .ForMember(c => c.District, opt => opt.MapFrom(x => RemoveHtmlTag(x.District)))
                .ForMember(c => c.Province, opt => opt.MapFrom(x => RemoveHtmlTag(x.Province)))
                .ForMember(c => c.Facebook, opt => opt.MapFrom(x => RemoveHtmlTag(x.Facebook)))
                .ForMember(c => c.Website, opt => opt.MapFrom(x => RemoveHtmlTag(x.Website)))
                .ForMember(c => c.LinkedIn, opt => opt.MapFrom(x => RemoveHtmlTag(x.LinkedIn)))
                .ForMember(c => c.Twitter, opt => opt.MapFrom(x => RemoveHtmlTag(x.Twitter)))
                .ForMember(c => c.Zalo, opt => opt.MapFrom(x => RemoveHtmlTag(x.Zalo)));

            CreateMap<CustomerUpdateDto, Customer>()
               .ForMember(c => c.LastName, opt => opt.MapFrom(x => RemoveHtmlTag(x.LastName)))
               .ForMember(c => c.FirstName, opt => opt.MapFrom(x => RemoveHtmlTag(x.FirstName)))
               .ForMember(c => c.PhoneNumber, opt => opt.MapFrom(x => RemoveHtmlTag(x.PhoneNumber)))
               .ForMember(c => c.DateOfBirth, opt => opt.MapFrom(x => RemoveHtmlTag(x.DateOfBirth)))
               .ForMember(c => c.MonthOfBirth, opt => opt.MapFrom(x => RemoveHtmlTag(x.MonthOfBirth)))
               .ForMember(c => c.YearOfBirth, opt => opt.MapFrom(x => RemoveHtmlTag(x.YearOfBirth)))
               .ForMember(c => c.Address, opt => opt.MapFrom(x => RemoveHtmlTag(x.Address)))
               .ForMember(c => c.Facebook, opt => opt.MapFrom(x => RemoveHtmlTag(x.Facebook)))
               .ForMember(c => c.Website, opt => opt.MapFrom(x => RemoveHtmlTag(x.Website)))
               .ForMember(c => c.LinkedIn, opt => opt.MapFrom(x => RemoveHtmlTag(x.LinkedIn)))
               .ForMember(c => c.Twitter, opt => opt.MapFrom(x => RemoveHtmlTag(x.Twitter)))
               .ForMember(c => c.Zalo, opt => opt.MapFrom(x => RemoveHtmlTag(x.Zalo)));
        }
    }
}
