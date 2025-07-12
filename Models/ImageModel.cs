namespace gymWebsite.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty; // Optional if you store path
        public string FilePath { get; set; } = string.Empty;
    }
}
