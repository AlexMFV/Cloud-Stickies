using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WindowsCloudStickies
{
    public static class Globals
    {
        public static string UserName = ""; //Debug
        public static StickyNotes stickies = new StickyNotes();
        public static string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static User user = null;
        public static Window helperWindow = SetHelper(); // Create helper window
        public static System.Windows.Forms.NotifyIcon ni = null;

        public static Window SetHelper()
        {
            Window win = new Window();
            win.Top = -100; // Location of new window is outside of visible part of screen
            win.Left = -100;
            win.Width = 1; // size of window is enough small to avoid its appearance at the beginning
            win.Height = 1;

            win.WindowStyle = WindowStyle.ToolWindow; // Set window style as ToolWindow to avoid its icon in AltTab 
            win.ShowInTaskbar = false;
            return win;
        }
    }
}
