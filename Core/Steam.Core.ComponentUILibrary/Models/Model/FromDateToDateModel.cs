namespace ComponentUILibrary.Models
{
   
    public class FromDateToDateModel
    {

        public string Id { get; set; }
        public string FromDate_Id { get; set; }
        public string ToDate_Id { get; set; }
        public string Class { get; set; }
        public string ClassContainner { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int ValidRangeDay { get; set; } = 0;
        public int ValidRangeMonth { get; set; } = 0;
        public int ValidRangeYear { get; set; } = 0;
        public string Style { get; set; }
        public string Placeholder { get; set; }
        public string Value { get; set; }
        public string Container { get; set; } = "";
        public bool FirstLoadLIb { get; set; } = false;


    }
}