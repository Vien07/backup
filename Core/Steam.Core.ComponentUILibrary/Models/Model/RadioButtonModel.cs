namespace ComponentUILibrary.Models
{
    public class RadioButtonModel: MasterModel
    {
        public string Id { get; set; }
        public string Class { get; set; }
        public string Style { get; set; }
        public bool Display { get; set; } = true; //set false is display vertically
        public List<RadioButtonData> Items { get; set; }
    }
}