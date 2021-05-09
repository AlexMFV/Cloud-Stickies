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
        private Guid noteID { get; set; }
        private string noteText { get; set; }
        private string noteTitle { get; set; }
        private SolidColorBrush noteColor { get; set; }
        private SolidColorBrush titleColor { get; set; }
        private DateTime dateCreated { get; set; }
        private string baseFont { get; set; }
        private int baseFontSize { get; set; }
        private string baseFontColor { get; set; }
        private int x { get; set; }
        private int y { get; set; }
        private int width { get; set; }
        private int height { get; set; }
        private bool isClosed { get; set; }
        private bool isLocked { get; set; }

        //Both these variables are not present neither locally nor in the database
        //They only serve to choose wether or not a not needs to be synced, or created
        //This way we can only send the notes this way we can filter them when before sending the http request
        public bool hasUpdated { get; set; }
        public bool isNew { get; set; }

        public StickyNote(Guid _noteGuid, Tuple<SolidColorBrush, SolidColorBrush> _noteColors)
        {
            this.noteID = _noteGuid;
            this.noteTitle = "Note " + Globals.stickies.Count.ToString(); //"Note ID:" + noteID.ToString();
            this.dateCreated = DateTime.Now;
            this.noteColor = _noteColors.Item1;
            this.titleColor = _noteColors.Item2;
            this.baseFontSize = 0;
            this.isNew = true;
            this.hasUpdated = false;
        }

        public StickyNote() { }

        public Guid NoteID
        {
            get { return this.noteID; }
            set { this.noteID = value; }
        }
        public string NoteText
        {
            get { return this.noteText; }
            set { this.noteText = value; }
        }
        public string NoteTitle
        {
            get { return this.noteTitle; }
            set { this.noteTitle = value; }
        }
        public SolidColorBrush NoteColor
        {
            get { return this.noteColor; }
            set { this.noteColor = value; }
        }
        public string NoteColorHex
        {
            get { return this.noteColor.Color.ToString(); }
        }
        public SolidColorBrush TitleColor
        {
            get { return this.titleColor; }
            set { this.titleColor = value; }
        }
        public string TitleColorHex
        {
            get { return this.titleColor.Color.ToString(); }
        }
        public DateTime DateCreated
        {
            get { return this.dateCreated; }
            set { this.dateCreated = value; }
        }
        public string BaseFont
        {
            get { return this.baseFont; }
            set { this.baseFont = value; }
        }
        public int BaseFontSize
        {
            get { return this.baseFontSize; }
            set { this.baseFontSize = value; }
        }
        public string BaseFontColor
        {
            get { return this.baseFontColor; }
            set { this.baseFontColor = value; }
        }
        public int X
        {
            get { return this.x; }
            set { this.x = value; }
        }
        public int Y
        {
            get { return this.y; }
            set { this.y = value; }
        }
        public int Width
        {
            get { return this.width; }
            set { this.width = value; }
        }
        public int Height
        {
            get { return this.height; }
            set { this.height = value; }
        }
        public bool IsClosed
        {
            get { return this.isClosed; }
            set { this.isClosed = value; }
        }
        public bool IsLocked
        {
            get { return this.isLocked; }
            set { this.isLocked = value; }
        }

        public void ChangeTitleToParagraph()
        {
            if(noteText != null && noteText != "")
            {
                string pre = this.noteText.Split('\n', '\r')[0];
                this.noteTitle = pre.Length > 30 ? pre.Substring(0,30) : pre;
            }
        }
    }
}
