using NoteShrink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WIA;

namespace NoteShrink.ViewModels
{
    class MainViewModel : Observable
    {
        private BitmapSource _image;
        private ImageBrush _imageBrush;
        private double _width = 800;
        private double _height = 600;
        private string _statusText = "Готово.";

        public double Width
        {
            get { return _width; }
            set { _width = value; NotifyPropertyChanged("Width"); }
        }
        public double Height
        {
            get { return _height; }
            set { _height = value; NotifyPropertyChanged("Height"); }
        }
        public ImageBrush ImageBrush
        {
            get { return _imageBrush; }
            set { _imageBrush = value; NotifyPropertyChanged("ImageBrush"); }
        }
        public string StatusText
        {
            get { return _statusText; }
            set { _statusText = value; NotifyPropertyChanged("StatusText"); }
        }
        #region Scan
        // Command for scanning
        private RelayCommand _scanCommand;
        public ICommand ScanCommand
        {
            get
            {
                
                if (_scanCommand == null)
                {
                    _scanCommand = new RelayCommand(param => this.Scan(), param => this.CanScan);
                }
                
                return _scanCommand;
            }
        }

        // implemented for the command pattern completeness. We don't currently
        // have any situations where we'd disable the ability to scan
        public bool CanScan
        {
            get { return true; }
        }

        // method to do the actual scanning
        public void Scan()
        {
            StatusText = "Сканирование...";
            var scanner = new ScannerService();

            try
            {
                ImageFile file = scanner.Scan();

                if (file != null)
                {
                    var converter = new ScannerImageConverter();

                    _image = converter.ConvertScannedImage(file);
                }
                else
                {
                    _image = null;
                }
                ImageBrush = new ImageBrush(_image) { Stretch = Stretch.Uniform };
            }
            catch (ScannerException ex)
            {
                // yeah, I know. Showing UI from the VM. Shoot me now.
                MessageBox.Show(ex.Message, "Не удалось выполнить сканирование изображения", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            StatusText = "Готово.";
        }
        #endregion

        #region LoadFile
        private RelayCommand _openFieCommand;
        public ICommand OpenFileCommand
        {
            get
            {
                if (_openFieCommand == null)
                {
                    _openFieCommand = new RelayCommand(param => this.OpenFile(), param => true);
                }
                return _openFieCommand;
            }
        }

        private void OpenFile()
        {
            StatusText = "Загрузка файла...";
            var fileSystem = new FileSystemService();

            try
            {
                _image = fileSystem.LoadFile();
                ImageBrush = new ImageBrush(_image) { Stretch = Stretch.Uniform };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            StatusText = "Готово.";
        }
        #endregion

        #region SaveFile
        private RelayCommand _saveFieCommand;
        public ICommand SaveFileCommand
        {
            get
            {
                if (_saveFieCommand == null)
                {
                    _saveFieCommand = new RelayCommand(param => this.SaveFile(), param => this.CanSave);
                }
                
                return _saveFieCommand;
            }
        }

        public bool CanSave { get { return _image != null; } }

        private void SaveFile()
        {
            StatusText = "Сохранение файла...";
            var fileSystem = new FileSystemService();

            try
            {
                fileSystem.SaveFile(_image);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            StatusText = "Готово.";
        }
        #endregion

        #region ConvertImage
        private RelayCommand _convertImageCommand;
        public ICommand ConvertImageCommand
        {
            get
            {
                if (_convertImageCommand == null)
                {
                    _convertImageCommand = new RelayCommand(param => this.ConvertAsync(), param => this.CanConvert);
                }
                
                return _convertImageCommand;
            }
        }

        public bool CanConvert { get { return _image != null; } }

        private async void ConvertAsync()
        {
            StatusText = "Конвертация...";
            var convertImage = new ConvertImageService();

            try
            {
                _image = await convertImage.Convert(_image);
                ImageBrush = new ImageBrush(_image) { Stretch = Stretch.Uniform };
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            StatusText = "Готово.";
        }
        #endregion

        #region PrintFile
        private RelayCommand _printCommand;
        public ICommand PrintCommand
        {
            get
            {
                
                if (_printCommand == null)
                {
                    _printCommand = new RelayCommand(param => this.Print(), param => this.CanPrint);
                }
                
                return _printCommand;
            }
        }

        public bool CanPrint { get { return _image != null; } }

        private void Print()
        {
            StatusText = "Печать...";
            var printService = new PrintService();

            try
            {
                printService.Print(_image);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            StatusText = "Готово.";
        }
        #endregion
    }
}
