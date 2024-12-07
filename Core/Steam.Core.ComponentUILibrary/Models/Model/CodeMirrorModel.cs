namespace ComponentUILibrary.Models
{
    public class CodeMirrorModel: MasterModel
    {
        public string Id { get; set; }
        public string Class { get; set; }
        public string Content { get; set; } 
        public string Cols { get; set; } 
        public string Rows { get; set; }
        public int Height { get; set; } = 400;
        public int Width { get; set; } = 400;
        public bool FirstLoadLib { get; set; } = false;
        public string CodeMode { get; set; } = "htmlmixed";

    }
}