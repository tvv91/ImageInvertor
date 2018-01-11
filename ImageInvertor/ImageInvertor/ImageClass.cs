using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ImageInvertor
{
    public class ImageClass
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public ImageClass(string ImagePath)
        {
            FilePath = ImagePath;
            FileName = Path.GetFileName(FilePath);
        }
    }
}
