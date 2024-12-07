namespace ComponentUILibrary.Models
{
    public class PaginationModel
    {
        public int PageCount { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;
        public string Id { get; set; }
        public string Class { get; set; }

    }
}