namespace ComponentUILibrary.Models
{
    public class TabSEOCModel: MasterModel
    {
        public string Slug { get; set; }
        public string MetaDescription { get; set; }
        public string TagKeys { get; set; }
        public string Meta { get; set; }
        public string Image { get; set; }
        public bool Display { get; set; } = true; //set false is display vertically
    }
}