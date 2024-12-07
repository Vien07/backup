namespace ComponentUILibrary.Models
{
    public class TableData:MasterModel
    {
        public List<TableHead> Header { get; set; }
        public dynamic Body { get; set; }
        public string Type { get; set; } = "View"; //Html
        public TableFoot Footer { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public bool CheckAllCol { get; set; } = true;
        public string ActionMove { get; set; } = "_action.Move";

    }
    public class TableHead
    {

        public string ID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public string Filter { get; set; } = string.Empty;
        public string OrderBy { get; set; } = string.Empty;
        public int Width { get; set; }

    }
    public class TableBody
    {
        public List<TableRow> Rows { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }

    }
    public class TableCell
    {
        public string ID { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public int Rowspan { get; set; }
        public int ColSpan { get; set; }

    }
    public class TableRow
    {
        public List<TableCell> Cells { get; set; }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }

    }
    public class TableFoot
    {

        public TableRow Row { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
    }
}