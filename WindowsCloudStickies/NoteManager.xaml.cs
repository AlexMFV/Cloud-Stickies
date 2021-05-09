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
using AutoUpdaterDotNET;
using System.Windows.Threading;
using System.Reflection;
using System.Net;
using System.IO;

namespace WindowsCloudStickies
{
    /// <summary>
    /// Interaction logic for NoteManager.xaml
    /// </summary>
    public partial class NoteManager : MetroWindow
    {
        List<Note> notes = new List<Note>();
        public bool isLogout = false;

        public NoteManager()
        {
            InitializeComponent();
            SetSystemTrayNotification();

            Version version = Assembly.GetEntryAssembly().GetName().Version;
            txtVersion.Text = "v" + version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;
            AutoUpdater.Synchronous = true;
            LocalSave.LoadStickyNotes(Globals.user.ID);
            lstNotes.ItemsSource = Globals.stickies;
            txtUsername.Text = "Logged in as: " + Globals.user.Username;

            //Do stuff depending on Registered, Login or Guest user
            //Use: Globals.user.authType
        }

        public void SetSystemTrayNotification()
        {
            Globals.ni = new System.Windows.Forms.NotifyIcon();
            Globals.ni.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();

            Globals.ni.ContextMenuStrip.Items.Add("Close").Click +=
                delegate (object sender, EventArgs e)
                {
                    this.Close();
                };

            Globals.ni.Icon = new System.Drawing.Icon(new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName + "/Images/notes.ico");
            Globals.ni.Visible = false;
            Globals.ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                    Globals.ni.Visible = false;
                };
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                this.Hide();
                Globals.ni.Visible = true;
            }

            base.OnStateChanged(e);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Guid ID = Guid.NewGuid();
            Tuple<SolidColorBrush, SolidColorBrush> colors = randomizeColor();
            Globals.stickies.Add(new StickyNote(ID, colors));
            Note note = new Note(ID, this);
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
            LocalSave.DeleteAllNotes(Globals.user.ID);

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
            List<Guid> toDelete = new List<Guid>();

            for(int i = 0; i < Globals.stickies.Count; i++)
            {
                for(int j = 0; j < lstNotes.SelectedItems.Count; j++)
                {
                    if (lstNotes.SelectedItems[j].Equals(Globals.stickies[i]))
                        toDelete.Add(Globals.stickies[i].NoteID);
                }
            }

            foreach (Guid delID in toDelete)
            {
                StickyNote note = Globals.stickies.GetNoteFromGUID(delID);
                LocalSave.DeleteNote(Globals.user.ID, note.NoteID);
                Globals.stickies.Remove(note);
            }

            updateList();
        }

        public void updateList()
        {
            lstNotes.ItemsSource = null;
            lstNotes.ItemsSource = Globals.stickies;
        }

        private void btnSaveAll_Click(object sender, RoutedEventArgs e)
        {
            if(Globals.stickies.Count > 0)
                LocalSave.SaveAllStickyNotes(Globals.user.ID);
        }

        private void btnCloseAll_Click(object sender, RoutedEventArgs e)
        {
            notes.ForEach(note => note.Close());
            notes = new List<Note>();
        }

        private void btnShowHide_Click(object sender, RoutedEventArgs e)
        {
            ShowPressedNote(((sender as Button).DataContext as StickyNote));            
        }

        public void DeleteNoteForm(Guid id)
        {
            Note toDelete = notes.First(note => note.current_note.NoteID == id);
            notes.Remove(toDelete);
        }

        #region Updates

        private void btnUpdates_Click(object sender, RoutedEventArgs e)
        {
            AutoUpdater.Start("http://alexmfv.com/StickyUpdates/latest_update.xml");
        }

        #endregion

        private void lstNotes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstNotes.SelectedItem != null)
                ShowPressedNote(lstNotes.SelectedItem as StickyNote);
        }

        void ShowPressedNote(StickyNote pressedNote)
        {
            if (!notes.Exists(note => note.current_note.NoteID == pressedNote.NoteID))
            {
                Tuple<SolidColorBrush, SolidColorBrush> colors = new Tuple<SolidColorBrush, SolidColorBrush>(pressedNote.NoteColor, pressedNote.TitleColor);
                Note note = new Note(pressedNote.NoteID, this);
                notes.Add(note);
                note.Show();
            }
            else
            {
                Note open = notes.First(note => note.current_note.NoteID == pressedNote.NoteID);
                notes.Remove(open);
                open.Close();
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            //If it's a connected user, save all the notes to the cloud first, then logout
            WindowManager.OpenLogin(this);
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!this.isLogout)
            {
                bool result = Messager.CloseApplication();
                if (result)
                {
                    foreach (Note n in notes)
                        n.Close(); //Maybe save first, the notes
                    notes = null;
                    Globals.stickies = null;
                    Globals.ni.Visible = false;
                    Globals.ni.Icon.Dispose();
                    Globals.ni.Dispose();
                    Application.Current.Shutdown();
                }
                else
                    e.Cancel = true;
            }
        }
    }
}