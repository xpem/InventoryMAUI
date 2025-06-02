namespace Inventory.Infra.Models
{
    public partial class UIImagePath(string imageFilePath, string fileName, string externalFileName = null) : BindableObject
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public string ExternalFileName { get; init; } = externalFileName;

        public string FileName { get; init; } = fileName;

        public string ImageFilePath { get; init; } = imageFilePath;
    }
}
