namespace KampusGuvenlikAI.WinForms.Properties;

internal static class Resources
{
    public static Image kmu_nizamiye
    {
        get
        {
            var imagePath = Path.Combine(AppContext.BaseDirectory, "Assets", "kmu_nizamiye.jpg");
            return Image.FromFile(imagePath);
        }
    }
}
