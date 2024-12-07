
using ComponentUILibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using X.PagedList;

namespace Admin.Course.Models
{

    public class CourseConfigDto
    {
        public int MinWidth { get; set; }
        public int MaxWidth { get; set; }
        public int PageSize { get; set; }
    }

    public class Root
    {
        public int id { get; set; }
        public List<CourseChapterDto> chapters { get; set; }
    }
    public class CourseChapterDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public int order { get; set; }
        public List<CourseLectureDto> lectures { get; set; }
    }

    public class CourseLectureDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public string lectureName { get; set; }
        public bool preview { get; set; }
        public int order { get; set; }
    }
}
