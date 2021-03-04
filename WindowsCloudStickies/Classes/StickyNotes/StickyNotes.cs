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

        public StickyNote this[int index]
        {
            get { return (StickyNote)List[index]; }
            set { List[index] = value; }
        }
    }
}
