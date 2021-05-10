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

        public static bool CloseApplication()
        {
            MessageBoxResult result = MessageBox.Show("The application will fully close, are you sure?", "Close warning", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
                return true;
            return false;
        }

        public static bool Logout()
        {
            MessageBoxResult result = MessageBox.Show("This will log you out of the account, are you sure?", "Logout warning", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
                return true;
            return false;
        }

        public static void CookieError()
        {
            MessageBoxResult result = MessageBox.Show("There was an error reading the file, you need to login again!", "Save error warning", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
