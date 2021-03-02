using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WindowsCloudStickies
{
    public class StickyNote
    {
        private string noteText { get; set; }
        private string noteTitle { get; set; }
        private SolidColorBrush noteColor { get; set; } //Maybe Store this as a Hex Value OR send RGB values when sending to server
        private SolidColorBrush titleColor { get; set; } //Maybe Store this as a Hex Value OR send RGB values when sending to server
        private string dateCreated { get; set; }
        private string baseFont { get; set; }
        private string baseFontSize { get; set; }
        private string baseFontColor { get; set; }

        public StickyNote()
        {

        }

        string NoteText() { return noteText; }
        string NoteTitle() { return noteTitle; }
        SolidColorBrush NoteColor() { return noteColor; }
        SolidColorBrush TitleColor() { return titleColor; }
        string DateCreated() { return dateCreated; }
        string BaseFont() { return baseFont; }
        string BaseFontSize() { return baseFontSize; }
        string BaseFontColor() { return baseFontColor; }
    }
}
