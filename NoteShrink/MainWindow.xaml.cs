using System;
using System.Collections.Generic;
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

namespace NoteShrink
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void mDocumentIncrease_Click(object sender, RoutedEventArgs e)
        {
            ScannerImage.Width += ScannerImage.Width * 0.25;
            ScannerImage.Height += ScannerImage.Height * 0.25;
        }

        private void mDocumentShrink_Click(object sender, RoutedEventArgs e)
        {
            ScannerImage.Width -= ScannerImage.Width * 0.25;
            ScannerImage.Height -= ScannerImage.Height * 0.25;
        }

        private void W_Main_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ScannerImage.Width = W_Main.Width - 50 - PageView.Width;
            ScannerImage.Height = W_Main.Height - 110;
        }

        private void mFileExit_Click(object sender, RoutedEventArgs e)
        {
            W_Main.Close();
        }

        private void mHelpAbout_Click(object sender, RoutedEventArgs e)
        {
            string msg = "NoteShrink v0.5.0.\nCreated by Georgy Perevozchikov (gosha20777@live.ru).\n\nNoteShrink - улучшает ваши сканы и уменьшает их размер.\n\nИсходные коды проекта доступны по ссылке:\nhttps://github.com/gosha20777/NoteShrink";
            MessageBox.Show(msg, "NoteShrink", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
