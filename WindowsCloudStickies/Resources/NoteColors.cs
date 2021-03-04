using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WindowsCloudStickies
{
    public static class NoteColors
    {
        //Note Colors
        public static SolidColorBrush GreenNote = (SolidColorBrush)Application.Current.Resources["GreenNote"];
        public static SolidColorBrush PinkNote = (SolidColorBrush)Application.Current.Resources["PinkNote"];
        public static SolidColorBrush OrangeNote = (SolidColorBrush)Application.Current.Resources["OrangeNote"];
        public static SolidColorBrush AquaNote = (SolidColorBrush)Application.Current.Resources["AquaNote"];
        public static SolidColorBrush YellowNote = (SolidColorBrush)Application.Current.Resources["YellowNote"];

        //Note Grip Bar Color
        public static SolidColorBrush GreenTitle = (SolidColorBrush)Application.Current.Resources["GreenTitle"];
        public static SolidColorBrush PinkTitle = (SolidColorBrush)Application.Current.Resources["PinkTitle"];
        public static SolidColorBrush OrangeTitle = (SolidColorBrush)Application.Current.Resources["OrangeTitle"];
        public static SolidColorBrush AquaTitle = (SolidColorBrush)Application.Current.Resources["AquaTitle"];
        public static SolidColorBrush YellowTitle = (SolidColorBrush)Application.Current.Resources["YellowTitle"];
    }
}
