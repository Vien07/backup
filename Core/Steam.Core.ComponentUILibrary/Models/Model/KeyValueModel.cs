namespace ComponentUILibrary.Models
{
    public class KeyValueModel
    {
        public string Id { get; set; }
        public string Class { get; set; }
        public string Style { get; set; }
        public string SelectedValue { get; set; }
        public string SeparateSympol { get; set; } = ";";
        public List<RadioData> Items { get; set; }

    }

}