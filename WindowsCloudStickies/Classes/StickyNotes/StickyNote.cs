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
        public SolidColorBrush noteColor { get; set; }
        public SolidColorBrush titleColor { get; set; }
        public DateTime dateCreated { get; set; }
        public string baseFont { get; set; }
        public string baseFontSize { get; set; }
        public string baseFontColor { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public bool isClosed { get; set; }
        public bool isLocked { get; set; }

        public StickyNote(Guid _noteGuid, Tuple<SolidColorBrush, SolidColorBrush> _noteColors)
        {
            this.noteID = _noteGuid;
            this.noteTitle = "Note ID:" + noteID.ToString();
            this.dateCreated = DateTime.Now;
            this.noteColor = _noteColors.Item1;
            this.titleColor = _noteColors.Item2;
        }

        public StickyNote() { }

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
