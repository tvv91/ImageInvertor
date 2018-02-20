using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ImageInvertor
{
    public class Invertor : INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Number of task = number of logical processors?
        /// </summary>
        public Invertor()
        {
            ProcessorCount = Environment.ProcessorCount;
            ThreadPool.SetMaxThreads(ProcessorCount, ProcessorCount);
        }
        #endregion

        #region Events
        /// <summary>
        /// Event for change UI control during processing
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Fields
        ///
        /// <summary>
        /// Collection of Image class as source for WrapPanel
        /// </summary>
        private ObservableCollection<ImageClass> _Images = new ObservableCollection<ImageClass>();

        /// <summary>
        /// List of file pathes
        /// </summary>
        private List<string> FileList;

        /// <summary>
        /// Source folder, where files for processing
        /// </summary>
        private string _SourceFolderPath;

        /// <summary>
        /// Destination folder, where processed files will saved
        /// </summary>
        public string _DestinationFolderPath;

        /// <summary>
        /// Number of processor count
        /// </summary>
        private int ProcessorCount;

        /// <summary>
        /// ProgressBar.Value
        /// </summary>
        private int _ProgressValue = 0;

        /// <summary>
        /// ProgressBar.Maximum
        /// </summary>
        private int _ImagesCount = 1;
        #endregion

        #region Methods
        /// <summary>
        /// Notify if property was changed, need for WPF elements
        /// </summary>
        /// <param name="propertyName"></param>
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ResetProgress()
        {
            ImagesCount = 1;
            ProgressValue = 0;
        }

        /// <summary>
        /// Recursive search files
        /// </summary>
        /// <param name="path"></param>
        public void Search()
        {
            ResetProgress();
            if (!string.IsNullOrEmpty(SourceFolderPath))
            {
                try
                {
                    Images.Clear();
                    FileList = Directory.EnumerateFiles(SourceFolderPath, "*.*", SearchOption.AllDirectories).Where(file => file.ToLower().EndsWith("jpg")).ToList();
                    foreach (string FilePath in FileList)
                        Images.Add(new ImageClass(FilePath));
                    ImagesCount = Images.Count;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        /// <summary>
        /// Fastest solution for invert bitmap. In feature I want create native DLL with SIMD extenstions.
        /// Get from: https://stackoverflow.com/questions/541331/effective-way-of-making-negative-of-image-without-external-dlls
        /// </summary>
        /// <param name="image"></param>
        private Bitmap Negative(ImageClass img)
        {
            const int RED_PIXEL = 2;
            const int GREEN_PIXEL = 1;
            const int BLUE_PIXEL = 0;
            Bitmap bitmap = new Bitmap(img.FilePath);
            BitmapData bmData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            try
            {
                int stride = bmData.Stride;
                int bytesPerPixel = (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb ? 3 : 4);
                unsafe
                {
                    byte* pixel = (byte*)(void*)bmData.Scan0;
                    int yMax = bitmap.Height;
                    int xMax = bitmap.Width;
                    for (int y = 0; y < yMax; y++)
                    {
                        int yPos = y * stride;
                        for (int x = 0; x < xMax; x++)
                        {
                            int pos = yPos + (x * bytesPerPixel);

                            pixel[pos + RED_PIXEL] = (byte)(255 - pixel[pos + RED_PIXEL]);
                            pixel[pos + GREEN_PIXEL] = (byte)(255 - pixel[pos + GREEN_PIXEL]);
                            pixel[pos + BLUE_PIXEL] = (byte)(255 - pixel[pos + BLUE_PIXEL]);
                        }
                    }
                    ProgressValue++;
                    if (ProgressValue == ImagesCount)
                        MessageBox.Show("Файлы обработаны", "Image Invertor", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return bitmap;
                }
            }
            finally
            {
                bitmap.UnlockBits(bmData);
            }
            
        }

        /// <summary>
        /// Iterate images
        /// </summary>
        public async void InvertImages()
        {
            await Task.Factory.StartNew(() =>
            Parallel.ForEach(Images, (img) =>
            {
                Bitmap bitmap = Negative(img);
                bitmap.Save(DestinationFolderPath + "\\" + img.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }));

        }
        #endregion

        #region Properties
        /// <summary>
        /// Source folder path
        /// </summary>
        public string SourceFolderPath
        {
            get
            {
                return _SourceFolderPath;
            }
            set
            {
                _SourceFolderPath = value;
                RaisePropertyChanged("SourceFolderPath");
            }
        }

        /// <summary>
        /// Destination folder path
        /// </summary>
        public string DestinationFolderPath
        {
            get
            {
                return _DestinationFolderPath;
            }
            set
            {
                _DestinationFolderPath = value;
                RaisePropertyChanged("DestinationFolderPath");
            }
        }

        /// <summary>
        /// Access to collection of images (need for source WPF wrap panel)
        /// </summary>
        public ObservableCollection<ImageClass> Images
        {
            get
            {
                return _Images;
            }
            set
            {
                _Images = value;
                RaisePropertyChanged("Images");
            }
        }

        /// <summary>
        /// For progressbar.Value
        /// </summary>
        public int ProgressValue
        {
            get
            {
                return _ProgressValue;
            }
            private set
            {
                _ProgressValue = value;
                RaisePropertyChanged("ProgressValue");
            }
        }

        /// <summary>
        /// For progressbar.Maximum
        /// </summary>
        public int ImagesCount
        {
            get
            {
                return _ImagesCount;
            }
            set
            {
                _ImagesCount = value;
                RaisePropertyChanged("ImagesCount");
            }
        }
        #endregion
    }
}
