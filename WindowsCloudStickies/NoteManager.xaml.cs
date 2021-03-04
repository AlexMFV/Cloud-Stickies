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
        List<Note> notes = new List<Note>();

        public NoteManager()
        {
            InitializeComponent();
            LocalSave.LoadStickyNotes(Guid.NewGuid());
            lstNotes.ItemsSource = Globals.stickies;
            //this.WindowState = WindowState.Minimized;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Guid ID = Guid.NewGuid();
            Tuple<SolidColorBrush, SolidColorBrush> colors = randomizeColor();
            Note note = new Note(ID, colors);
            Globals.stickies.Add(new StickyNote(ID, colors));
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
            Globals.stickies = new StickyNotes();

            updateList();
        }

        Tuple<SolidColorBrush, SolidColorBrush> randomizeColor()
        {
            Random r = new Random();
            int value = r.Next(0, 5);

            SolidColorBrush color1, color2;
            switch (value)
            {
                case 0: color1 = NoteColors.GreenNote; color2 = NoteColors.GreenTitle; break;
                case 1: color1 = NoteColors.PinkNote; color2 = NoteColors.PinkTitle; break;
                case 2: color1 = NoteColors.AquaNote; color2 = NoteColors.AquaTitle; break;
                case 3: color1 = NoteColors.OrangeNote; color2 = NoteColors.OrangeTitle; break;
                default: color1 = NoteColors.YellowNote; color2 = NoteColors.YellowTitle; break;
            }

            return createColorObject(color1, color2);
        }

        public Tuple<SolidColorBrush, SolidColorBrush> createColorObject(SolidColorBrush color1, SolidColorBrush color2)
        {
            return new Tuple<SolidColorBrush, SolidColorBrush>(color1, color2);
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
            lstNotes.ItemsSource = Globals.stickies;
        }

        private void btnSaveAll_Click(object sender, RoutedEventArgs e)
        {
            if(Globals.stickies.Count > 0)
            {
                LocalSave.SaveAllStickyNotes(Guid.NewGuid()); //Change to User GUID later
            }
        }
    }
}