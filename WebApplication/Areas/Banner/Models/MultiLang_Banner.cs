﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Banner.Models
{
    public class MultiLang_Banner
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public int BannerPid { get; set; }
        public virtual Banner Banner { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string Description { get; set; }
        public string LangKey { get; set; }
    }
}
