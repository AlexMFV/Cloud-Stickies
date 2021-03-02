using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindowsCloudStickies
{
    /// <summary>
    /// Interaction logic for NoteManager.xaml
    /// </summary>
    public partial class NoteManager : MetroWindow
    {
        public static List<StickyNote> stickies = new List<StickyNote>();
        List<Note> notes = new List<Note>();

        public NoteManager()
        {
            InitializeComponent();
            lstNotes.ItemsSource = stickies;
            //this.WindowState = WindowState.Minimized;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Guid ID = Guid.NewGuid();
            Tuple<SolidColorBrush, SolidColorBrush> colors = randomizeColor();
            Note note = new Note(ID, colors);
            stickies.Add(new StickyNote(ID, colors));
            notes.Add(note);
            note.Show();
            updateList();
            //lstNotes.Items[lstNotes.Items.Count - 1];
        }

        private void btnRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            ///DELETE ALL
            for(int i = notes.Count-1; i >= 0; i--)
                notes[i].Close();

            notes = new List<Note>();
            stickies = new List<StickyNote>();

            updateList();
        }

        Tuple<SolidColorBrush, SolidColorBrush> randomizeColor()
        {
            Random r = new Random();
            int value = r.Next(0, 5);

            Tuple<SolidColorBrush, SolidColorBrush> colors;

            switch (value)
            {
                case 0: colors = new Tuple<SolidColorBrush, SolidColorBrush>(NoteColors.GreenNote,NoteColors.GreenTitle); break;
                case 1: colors = new Tuple<SolidColorBrush, SolidColorBrush>(NoteColors.PinkNote, NoteColors.PinkTitle); break;
                case 2: colors = new Tuple<SolidColorBrush, SolidColorBrush>(NoteColors.AquaNote, NoteColors.AquaTitle); break;
                case 3: colors = new Tuple<SolidColorBrush, SolidColorBrush>(NoteColors.OrangeNote, NoteColors.OrangeTitle); break;
                default: colors = new Tuple<SolidColorBrush, SolidColorBrush>(NoteColors.YellowNote, NoteColors.YellowTitle); break;
            }

            return colors;
        }

        private void btnRemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            /*IList<Note> selectedNotes = (IList<Note>)lstNotes.SelectedItems;
            Guid[] idSublist = (Guid[])selectedNotes.Select(note => note.noteID);

            Globals.stickies.RemoveAll(note => idSublist.Contains(note.NoteID()));
            notes.ToList().ForEach(note => note.Close());
            notes.RemoveAll(note => idSublist.Contains(note.noteID));

            updateList();*/

            ///DELETE SELECTED
            //bool[] toDelete = new bool[notes.Count];
            //int aux = 0;
            //foreach (StickyNote note in Globals.stickies)
            //{
            //    for (int i = 0; i < lstNotes.SelectedItems.Count; i++)
            //    {
            //        if (note == lstNotes.SelectedItems[i])
            //        {
            //            toDelete[note.NoteID()] = true;
            //            notes[note.NoteID()].Close();
            //        }
            //    }
            //    aux++;
            //}

            //for (int i = notes.Count - 1; i >= 0; i--)
            //{
            //    if (toDelete[i] == true)
            //    {
            //        Globals.stickies.RemoveAt(i);
            //        notes.RemoveAt(i);
            //    }
            //}
        }

        void updateList()
        {
            lstNotes.ItemsSource = null;
            lstNotes.ItemsSource = stickies;
        }
    }
}