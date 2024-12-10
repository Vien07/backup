﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Calendar.Models
{
    public class Calendar
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        public string Images { get; set; }
        //public string Link { get; set; }
        //public string Position { get; set; }
        public DateTime StartDate { get; set; }

        public int Order { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string CreateUser { get; set; }
        public System.Nullable<DateTime> CreateDate { get; set; } = DateTime.Now;
        public System.Nullable<DateTime> LastLogin { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public System.Nullable<DateTime> UpdateDate { get; set; }
        public virtual ICollection<MultiLang_Calendar> MultiLang_Calendars { get; set; }

    }
}
