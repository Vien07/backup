
namespace Steam.Core.Base.Models
{
    public class SelectModel
    {
        public string Id { get; set; }
        public int Search { get; set; } = 0;
        public string Name { get; set; }
        public string SelectedValue { get; set; }
        public bool IsRequire { get; set; } = true;
        public string Class { get; set; }
        public string Event { get; set; }
        public string Size { get; set; } //small wide
        public long ParentId { get; set; }
    }
}
