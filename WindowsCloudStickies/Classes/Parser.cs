using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WindowsCloudStickies
{
    public static class Parser
    {
        public static SolidColorBrush ToColor(string values)
        {
            string[] rgb = values.Split(':');
            byte r = Byte.Parse(rgb[0]);
            byte g = Byte.Parse(rgb[1]);
            byte b = Byte.Parse(rgb[2]);
            return new SolidColorBrush(Color.FromRgb(r, g, b));
        }
    }
}