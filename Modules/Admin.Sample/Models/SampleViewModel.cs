
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;
using AutoMapper;
using Admin.Sample.Database;
using FluentValidation.Results;
using FluentValidation;
using Steam.Core.Base.Models;
using AutoMapper;

namespace Admin.Sample.Models
{


    public class SampleModelEdit : Database.SampleDetail
    {
        public string CheckBox { get; set; }
        public string ListFiles { get; set; }
        public IFormFile files { get; set; }
        public string fileStatus { get; set; }
        public string FilePath { get; set; }
        public string LangCode { get; set; }
        public Admin.Sample.Database.SampleDetail GetDatabaseModel(IMapper mapper)
        {

            return mapper.Map<Admin.Sample.Database.SampleDetail>(this);
        }
        public ValidateModel ValiDate()
        {
            var rs = new ValidateModel();
            var validator = new SampleModelEditValidator();

            FluentValidation.Results.ValidationResult results = validator.Validate(this);

            bool success = results.IsValid;
            if (!results.IsValid)
            {
                List<ValidationFailure> failures = results.Errors;
                rs.Message = failures.FirstOrDefault().ErrorMessage;
                rs.isValidate = false;
            }
            return rs;
        }
        class SampleModelEditValidator : AbstractValidator<SampleModelEdit>
        {
            public SampleModelEditValidator()
            {
                RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!");

            }
            private bool CheckDeposit(string deposit)
            {
                return decimal.TryParse(deposit, out _);
            }

        }
    }

    public class SampleDetailDTO : Database.SampleDetail
    {

        public string ShowCheck { get; set; }
        public string ShowTextMute { get; set; }


    }
    public class SampleConfigDTO
    {
        public string Type { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
    }




    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SampleModelEdit, SampleDetail>();
                cfg.CreateMap<SampleDetail, SampleDetailDTO>()
            .ForMember(dest => dest.ShowCheck, opt => opt.MapFrom(src => src.Enabled ? "checked" : ""))
            .ForMember(dest => dest.ShowTextMute, opt => opt.MapFrom(src => src.Enabled ? "text-primary" : "text-mute"));
                cfg.CreateMap<SampleConfig, SampleConfigDTO>();

            });

            IMapper mapper = config.CreateMapper();
            return mapper;
        }
    }

}
