using System.IO;

namespace ImageInvertor
{
    public class Image
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public Image(string ImagePath)
        {
            FilePath = ImagePath;
            FileName = Path.GetFileName(FilePath);
        }
    }
}
