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
        public Guid noteID { get; set; }
        public string noteText { get; set; }
        public string noteTitle { get; set; }
        public SolidColorBrush noteColor { get; set; } //Maybe Store this as a Hex Value OR send RGB values when sending to server
        public SolidColorBrush titleColor { get; set; } //Maybe Store this as a Hex Value OR send RGB values when sending to server
        public DateTime dateCreated { get; set; }
        public string baseFont { get; set; }
        public string baseFontSize { get; set; }
        public string baseFontColor { get; set; }

        public StickyNote(Guid _noteGuid, Tuple<SolidColorBrush, SolidColorBrush> _noteColors)
        {
            this.noteID = _noteGuid;
            this.noteTitle = "Note ID:" + noteID.ToString();
            this.dateCreated = DateTime.Now;
            this.noteColor = _noteColors.Item1;
            this.titleColor = _noteColors.Item2;
        }

        public Guid NoteID() { return noteID; }
        public string NoteText() { return noteText; }
        public string NoteTitle() { return noteTitle; }
        public SolidColorBrush NoteColor() { return noteColor; }
        public SolidColorBrush TitleColor() { return titleColor; }
        public DateTime DateCreated() { return dateCreated; }
        public string BaseFont() { return baseFont; }
        public string BaseFontSize() { return baseFontSize; }
        public string BaseFontColor() { return baseFontColor; }
    }
}
