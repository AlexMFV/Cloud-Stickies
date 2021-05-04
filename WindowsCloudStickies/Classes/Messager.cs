using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WindowsCloudStickies
{
    public static class Messager
    {
        public static void Process(Exception e)
        {
            MessageBox.Show(e.Message, "An error occured.", MessageBoxButton.OK);
        }
    }
}
