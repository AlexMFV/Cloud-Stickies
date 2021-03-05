using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsCloudStickies
{
    public static class Globals
    {
        public static string UserName = ""; //Debug
        public static StickyNotes stickies = new StickyNotes();
        public static string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }
}
