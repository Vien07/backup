
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Admin.Sample.Models;
using FluentValidation;
using FluentValidation.Results;
using Steam.Core.Base.Models;
using System;

namespace Admin.Sample.Database
{
    public class SampleDetail : BaseEntity
    {


        [Required(ErrorMessage = "Required")]
        [IsMultilang(true)]

        public string Title { get; set; }
        public string SampleDatePicker { get; set; }
        public string? Description { get; set; } = String.Empty;
        [Required(ErrorMessage = "Required")]
        public string Images { get; set; } = String.Empty;
        public string? Link { get; set; } = String.Empty;
        public string? Position { get; set; } = String.Empty;
        public string? FilePath { get; set; } = String.Empty;
        public string? Images_Caption { get; set; } = String.Empty;
        public string? Images_Description { get; set; } = String.Empty;
        public string? Images_Alt { get; set; } = String.Empty;

    }




}

