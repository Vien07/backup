namespace ComponentUILibrary.Models
{
    public class Select2Model: MasterModel
    {
        public List<SelectControlData> Data { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Size { get; set; } //small wide
    }
}