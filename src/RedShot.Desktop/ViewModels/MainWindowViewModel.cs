using Avalonia.Controls;
using ReactiveUI;
using RedShot.ScreenshotCapture;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reactive;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RedShot.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> ScreenShotCommand { get; }

        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get
            {
                return imageSource;
            }
            private set
            {
                this.RaiseAndSetIfChanged(ref imageSource, value);
            }
        }
        public MainWindowViewModel()
        {
            ScreenShotCommand = ReactiveCommand.Create(() =>
            {
                ConvertFromImage(ScreenShot.TakeScreenshot());
            });
        }

        public ImageSource ConvertFromImage(System.Drawing.Image image)
        {
           using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }
}
