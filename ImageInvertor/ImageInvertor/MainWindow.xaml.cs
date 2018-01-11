using System.Windows;


namespace ImageInvertor
{
    public partial class MainWindow : Window
    {
        Invertor invertor = new Invertor();

        public MainWindow()
        {
            DataContext = invertor;
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void SetSourceFolder_Click(object sender, RoutedEventArgs e)
        {
            invertor.SourceFolderPath = FolderDialog.GetFolderPath();
        }


        private void StartProcessButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrEmpty(invertor.SourceFolderPath))
                MessageBox.Show("Не выбрана папка с файлами", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else if (string.IsNullOrEmpty(invertor.DestinationFolderPath))
                MessageBox.Show("Не выбрана папка для результатов", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else invertor.InvertImages();
        }

        private void TextBoxSourceFolder_SelectionChanged(object sender, RoutedEventArgs e)
        {
            invertor.Search();
        }

        private void SetDestinationFolder_Click(object sender, RoutedEventArgs e)
        {
            invertor.DestinationFolderPath = FolderDialog.GetFolderPath();
        }
    }
}
