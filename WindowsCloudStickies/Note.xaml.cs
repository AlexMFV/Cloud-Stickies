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
using System.Windows.Shapes;

namespace WindowsCloudStickies
{
    /// <summary>
    /// Interaction logic for Note.xaml
    /// </summary>
    public partial class Note : Window
    {
        public Note()
        {
            InitializeComponent();
            randomizeColor();
            this.ShowInTaskbar = false;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void gripBar_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
        void randomizeColor()
        {
            Random r = new Random();
            int value = r.Next(0, 4);

            switch (value)
            {
                case 0: changeColor("GreenNote", "GreenTitle"); break;
                case 1: changeColor("PinkNote", "PinkTitle"); break;
                case 2: changeColor("AquaNote", "AquaTitle"); break;
                case 3: changeColor("OrangeNote", "OrangeTitle"); break;
                default: break;
            }
        }

        void changeColor(string bg, string topbar)
        {
            noteBackground.Background = (SolidColorBrush)Application.Current.Resources[bg];
            gripBar.Background = (SolidColorBrush)Application.Current.Resources[topbar];
        }
    }
}
