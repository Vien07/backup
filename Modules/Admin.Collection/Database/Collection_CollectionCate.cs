
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.Collection.Database
{
    public class Collection_CollectionCate : BaseEntity
    {

        public long CollectionPid { get; set; }
        public long CollectionCatePid { get; set; }

    }


}

