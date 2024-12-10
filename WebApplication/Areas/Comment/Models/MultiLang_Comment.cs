using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Comment.Models
{
    public class MultiLang_Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public int CommentPid { get; set; }
        public virtual Comment Comment { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string LangKey { get; set; }
    }
}
