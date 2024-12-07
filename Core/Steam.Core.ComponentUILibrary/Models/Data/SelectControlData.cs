namespace ComponentUILibrary.Models
{
    public class SelectControlData
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public double? Order { get; set; } = 1;
        public int? ParrentID { get; set; } = 0;


    }
}