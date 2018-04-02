using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace NoteShrink.Models
{
    class ConvertImageService
    {
        private readonly string _inputName = @"conv\input.png";
        private readonly string _outputName = @"page0000.png";
        private readonly string _processName = @"conv\noteshrink.exe";

        public async Task<BitmapSource> Convert(BitmapSource bitmapSource)
        {
            using (var fileStream = new FileStream(_inputName, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(fileStream);
            }

            var t = await Task.Run(() => RunProcess());

            File.Delete(_inputName);
            BitmapFrame img;
            // load the file to a WPF type
            using (FileStream stream = File.OpenRead(_outputName))
            {
                img = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                stream.Close();
            }
            File.Delete(_outputName);
            return img;
        }

        public bool RunProcess()
        {
            using (Process process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = _processName,
                    Arguments = _inputName,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true
                };
                bool sucsess = process.Start();
                process.WaitForExit();

                return sucsess;
            }
        }
    }
}
