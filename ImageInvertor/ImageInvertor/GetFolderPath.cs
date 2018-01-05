using System.Windows.Forms;

namespace ImageInvertor
{
    public static class FolderDialog
    {
        /// <summary>
        /// Get folder path
        /// </summary>
        /// <returns></returns>
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
