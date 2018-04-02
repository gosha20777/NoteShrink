using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WIA;

namespace NoteShrink.Models
{
    public class ScannerImageConverter
    {
        // this could be in the ScannerService, but then that service
        // takes a dependency on WPF, which I didn't want. Better to have
        // the dependencies wrapped into this service instead. Requires
        // FileIOPermission
        public BitmapSource ConvertScannedImage(ImageFile imageFile)
        {
            if (imageFile == null)
                return null;

            // save the image out to a temp file
            string fileName = Path.GetTempFileName();

            // this is pretty hokey, but since SaveFile won't overwrite, we 
            // need to do something to both guarantee a unique name and
            // also allow SaveFile to write the file
            File.Delete(fileName);

            // now save using the same filename
            imageFile.SaveFile(fileName);

            BitmapFrame img;

            // load the file back in to a WPF type, this is just 
            // to get around size issues with large scans
            using (FileStream stream = File.OpenRead(fileName))
            {
                img = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);

                stream.Close();
            }

            // clean up
            File.Delete(fileName);

            return img;
        }


        // this will choke on large images (like 1200dpi scans)
        // for that reason, you may want to do the conversion by
        // saving the ImageFile to a temp file and then loading it
        // in to convert it, as we do in the revised method above
        public BitmapSource InMemoryConvertScannedImage(ImageFile imageFile)
        {
            if (imageFile == null)
                return null;

            Vector vector = imageFile.FileData;

            if (vector != null)
            {
                byte[] bytes = vector.get_BinaryData() as byte[];

                if (bytes != null)
                {
                    var ms = new MemoryStream(bytes);
                    return BitmapFrame.Create(ms);
                }
            }

            return null;
        }

    }
}
