namespace ComponentUILibrary.Models
{
    public class DatePickerModel
    {
        public string Id { get; set; }
        public string Class { get; set; }
        public string ClassContainner { get; set; }
        public string Style { get; set; }
        public string Placeholder { get; set; }
        public string Value { get; set; }
        public string Container { get; set; } = "";
        public bool FirstLoadLIb { get; set; } = false;

    }
}