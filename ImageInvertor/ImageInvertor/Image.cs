using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageInvertor
{
    public class Image
    {
        public string FilePath { get; set; }
        public Image(string ImagePath)
        {
            FilePath = ImagePath;
        }
    }
}
