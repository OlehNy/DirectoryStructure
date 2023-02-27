namespace DirectoryStructure.Models
{
    public class Directory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ParentDirectoryId { get; set; }

        public Directory? Parent { get; set; }
        public ICollection<Directory>? Children { get; set; }
    }
}
