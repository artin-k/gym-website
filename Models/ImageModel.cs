namespace gymWebsite.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }
}
