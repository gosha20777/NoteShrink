using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WIA;

namespace NoteShrink.Models
{
    class ScannerService
    {
        public ImageFile Scan()
        {
            ImageFile image;

            try
            {
                CommonDialog dialog = new CommonDialog();

                image = dialog.ShowAcquireImage(
                        WiaDeviceType.ScannerDeviceType,
                        WiaImageIntent.ColorIntent,
                        WiaImageBias.MaximizeQuality,
                        "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}", //FormatID.wiaFormatJPEG
                        false,
                        true,
                        false);

                return image;
            }
            catch (COMException ex)
            {
                if (ex.ErrorCode == -2145320939)
                {
                    throw new ScannerNotFoundException();
                }
                else
                {
                    throw new ScannerException("COM Exception", ex);
                }
            }
        }
    }
}
