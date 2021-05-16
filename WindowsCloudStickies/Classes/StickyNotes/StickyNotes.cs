using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsCloudStickies
{
    public class StickyNotes : CollectionBase
    {
        public void Add (StickyNote note)
        {
            List.Add(note);
        }

        public void Remove(StickyNote note)
        {
            List.Remove(note);
        }

        public List<StickyNote> ToList()
        {
            List<StickyNote> clone = new List<StickyNote>();

            for (int i = 0; i < List.Count; i++)
                clone.Add((List[i] as StickyNote));

            return clone;
        }

        public StickyNote GetNoteFromGUID(Guid id)
        {
            return List.Cast<StickyNote>().First(note => note.Note_ID == id);
        }

        public int GetNoteIndex(Guid id)
        {
            return List.IndexOf(List.Cast<StickyNote>().First(note => note.Note_ID == id));
        }

        public StickyNote this[int index]
        {
            get { return (StickyNote)List[index]; }
            set { List[index] = value; }
        }
    }
}
