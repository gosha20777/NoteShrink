using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace NoteShrink.Models
{
    class PrintService
    {
        public void Print(BitmapSource bitmapSource)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                var capability = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
                var pageArea = capability.PageImageableArea;
                Image img = new Image
                {
                    Source = bitmapSource
                };
                Size size = new Size { Width = pageArea.ExtentWidth, Height = pageArea.ExtentHeight };
                img.Measure(size);
                img.Arrange(new Rect { Width = pageArea.ExtentWidth, Height = pageArea.ExtentHeight });
                
                printDialog.PrintVisual(img, "Печать");
            }
        }

        
    }
}
