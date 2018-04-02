using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace NoteShrink.Models
{
    class FileSystemService
    {
        public BitmapSource LoadFile()
        {
            OpenFileDialog openDig = new OpenFileDialog
            {
                Title = "Выбирете файл...",
                Filter = "Картинки (*.png; *.jpg;)|*.png;*.jpg;",
                Multiselect = false
            };
            if (openDig.ShowDialog() == true && openDig.CheckFileExists)
            {
                BitmapFrame img;
                // load the file to a WPF type
                using (FileStream stream = File.OpenRead(openDig.FileName))
                {
                    img = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    stream.Close();
                }

                return img;
            }
            else
                return null;
        }

        public void SaveFile(BitmapSource bitmapSource)
        {
            SaveFileDialog saveDig = new SaveFileDialog
            {
                FileName = "Output",
                Title = "Сохранить...",
                Filter = "Картинки (*.png)|*.png"
            };
            if (saveDig.ShowDialog() == true && saveDig.CheckPathExists)
            {
                using (var fileStream = new FileStream(saveDig.FileName, FileMode.Create))
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    encoder.Save(fileStream);
                }
            }
        }
    }
}
