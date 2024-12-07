
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.PostsManagement.Database
{
    public class PostsManagement_PostsManagementCate : BaseEntity
    {

        public long PostsManagementPid { get; set; }
        public long PostsManagementCatePid { get; set; }

    }


}

