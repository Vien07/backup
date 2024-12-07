
using FluentValidation;
using Steam.Core.Base.Constant;
using Steam.Core.Base.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.DashBoard.Database
{
    public class Dashboard_Shortcut : BaseEntity
    {


        public string Name { get; set; }
        public string IconSvg { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }



    }
    public class ShortcutValidator : AbstractValidator<Dashboard_Shortcut>
    {
        public ShortcutValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(nameof(Dashboard_Shortcut.Name) + " - " + ErrorCode.NotEmpty);
            RuleFor(x => x.Link).NotEmpty().WithMessage(nameof(Dashboard_Shortcut.Link) + " - " + ErrorCode.NotEmpty);
            RuleFor(x => x.IconSvg).NotEmpty().WithMessage(nameof(Dashboard_Shortcut.IconSvg) + " - " + ErrorCode.NotEmpty);
            RuleFor(x => x.Description).NotEmpty().WithMessage(nameof(Dashboard_Shortcut.Description) + " - " + ErrorCode.NotEmpty);

        }

        private bool CheckTypeImage(string postcode)
        {
            return true;
        }
    }
}
