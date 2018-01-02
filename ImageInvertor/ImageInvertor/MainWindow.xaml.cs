using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace ImageInvertor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Image> Images = new ObservableCollection<Image>();
        List <string> FileList;
        string SourceFolderPath, DestinationFolderPath;
        public MainWindow()
        {
            InitializeComponent();
            ImageList.ItemsSource = Images;
        }

        void Search(string path)
        {
            try
            {
                Images.Clear();
                FileList = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories).Where(file => file.ToLower().EndsWith("jpg")).ToList();
                foreach (string FilePath in FileList)
                    Images.Add(new Image(FilePath));
            }
            catch (ArgumentException ex)
            {
                
            }
            catch (DirectoryNotFoundException ex)
            {
                
            }
        }

        private void SetSourceFolder_Click(object sender, RoutedEventArgs e)
        {
            SourceFolderPath = FolderDialog.GetFolderPath();
            TextBoxSourceFolder.Text = SourceFolderPath;
            
        }
        private async void Invertion()
        {
            Dispatcher.Invoke(new Action(() => { PBar.Maximum = Images.Count; }));
            try
            {
                await Task.Run(() =>
                {
                    int cnt = 0;
                    foreach (Image img in Images)
                    {

                        Bitmap bitmap = new Bitmap(img.FilePath);
                        for (int y = 0; (y <= bitmap.Height - 1); y++)
                        {
                            for (int x = 0; (x <= (bitmap.Width - 1)); x++)
                            {
                                System.Drawing.Color inv = bitmap.GetPixel(x, y);
                                inv = System.Drawing.Color.FromArgb(255, (255 - inv.R), (255 - inv.G), (255 - inv.B));
                                bitmap.SetPixel(x, y, inv);
                            }
                        }
                        bitmap.Save(DestinationFolderPath + cnt.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                        cnt++;
                        Dispatcher.Invoke(new Action(() => { PBar.Value++; }));
                    }
                    MessageBox.Show("Обработано");
                    Dispatcher.Invoke(new Action(() => { PBar.Value = 0; }));
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void StartProcessButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SourceFolderPath))
                MessageBox.Show("Не выбрана папка с файлами");
            else if (string.IsNullOrEmpty(DestinationFolderPath))
                MessageBox.Show("Не выбрана папка для результатов");
            else Invertion();
        }

        private void TextBoxSourceFolder_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Search(SourceFolderPath);
        }

        private void SetDestinationFolder_Click(object sender, RoutedEventArgs e)
        {
            DestinationFolderPath = FolderDialog.GetFolderPath();
            TextBoxDestinationFolder.Text = DestinationFolderPath;
        }
    }
}
