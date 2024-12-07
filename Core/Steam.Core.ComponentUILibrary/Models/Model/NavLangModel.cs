namespace ComponentUILibrary.Models
{
    public class NavLangModel
    {
        public string Id { get; set; }=String.Empty;
        public string Class { get; set; } = String.Empty;
        public string Active { get; set; } = String.Empty;
        public string DefaultLang { get; set; } = String.Empty;
        public List<MultilangModel> ListLangs { get; set; } 

    }
    public class MultilangModel
    {
        public string Lang { get; set; }
        public string Name { get; set; }
        public string nation { get; set; }
        public string Icon { get; set; }
        public bool isDefault { get; set; }
        public int Order { get; set; }
    }
}