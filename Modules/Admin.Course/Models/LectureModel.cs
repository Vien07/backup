
using ComponentUILibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using X.PagedList;

namespace Admin.Course.Models
{

    public class LectureConfigDto
    {
        public int MinWidth { get; set; }
        public int MaxWidth { get; set; }
        public int PageSize { get; set; }
    }
}
