
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Steam.Core.Base.Models;

namespace Admin.ProductManagement.Database
{
    public class MisaApiConfig : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}

