namespace Models
{
    public static class FilePaths
    {
        private static readonly string AppPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Inventory");

        public static readonly string ImagesPath = Path.Combine(AppPath, "Images");

        public static readonly string DbPath = Path.Combine(AppPath, "Db");
    }
}
