using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageInvertor
{
    public static class FolderDialog
    {
        public static string GetFolderPath()
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Выберите папку с файлами ";
            if (folderDialog.ShowDialog() == DialogResult.OK)
                return folderDialog.SelectedPath;
            else return string.Empty;
        }
    }

}
