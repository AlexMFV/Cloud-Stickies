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
using System.Windows;
using System.Windows.Forms;

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
            AutoUpdater.CheckForUpdateEvent += AutoUpdater_CheckForUpdateEvent;

            Version version = Assembly.GetEntryAssembly().GetName().Version;
            txtVersion.Text = "v" + version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;
            AutoUpdater.Synchronous = true;
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

            Globals.ni.Icon = Properties.Resources.notes;

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

        private async void btnRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            ///DELETE ALL
            //TODO: Update this to use the StickyNote.CloseNoteIfExists(), instead of a global list
            for(int i = notes.Count-1; i >= 0; i--)
                notes[i].Close();

            notes = new List<Note>();

            List<string> noteIDs = new List<string>();

            foreach (StickyNote note in Globals.stickies)
                noteIDs.Add(note.Note_ID.ToString());

            Globals.stickies = new StickyNotes();

            if(!Globals.user.IsGuest)
                await DAL.DeleteNotesFromUser(Globals.user.ID, noteIDs);

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

        private async void btnRemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            List<Guid> toDelete = new List<Guid>();

            for(int i = 0; i < Globals.stickies.Count; i++)
            {
                for(int j = 0; j < lstNotes.SelectedItems.Count; j++)
                {
                    if (lstNotes.SelectedItems[j].Equals(Globals.stickies[i]))
                        toDelete.Add(Globals.stickies[i].Note_ID);
                }
            }

            List<string> noteIDs = new List<string>();

            foreach (Guid delID in toDelete)
            {
                noteIDs.Add(delID.ToString());
                StickyNote note = Globals.stickies.GetNoteFromGUID(delID);
                note.CloseNoteIfExists();
                LocalSave.DeleteNote(Globals.user.ID, note.Note_ID);
                Globals.stickies.Remove(note);
            }

            //TODO: Needs to check DB connection first, and if successful proceed
            if (noteIDs.Count > 0 && !Globals.user.IsGuest)
                await DAL.DeleteNotesFromUser(Globals.user.ID, noteIDs);

            updateList();
        }

        public void updateList()
        {
            lstNotes.ItemsSource = null;
            lstNotes.ItemsSource = Globals.stickies;
        }

        private async void btnSaveAll_Click(object sender, RoutedEventArgs e)
        {
            if (Messager.SaveAllNotes() == MessageBoxResult.Yes)
            {
                bool hasError = false;
                if (Globals.stickies.Count > 0)
                {
                    if (!Globals.user.IsGuest)
                    {
                        //TODO: Instead of launching multiple updates, create a new save all notes and run only once but for all the notes
                        foreach (StickyNote note in Globals.stickies)
                        {
                            bool updateResult = await DAL.UpdateNote(Globals.user.Username, note, true);
                            if(!updateResult && !hasError)
                                hasError = true;
                        }
                    }
                    LocalSave.SaveAllStickyNotes(Globals.user.ID);
                }

                if(hasError)
                    Messager.SaveAllNotesError();
                else
                    Messager.SaveAllNotesSuccess();
            }
        }

        private void btnCloseAll_Click(object sender, RoutedEventArgs e)
        {
            notes.ForEach(note => note.Close());
            notes = new List<Note>();
        }

        private void btnShowHide_Click(object sender, RoutedEventArgs e)
        {
            ShowPressedNote(((sender as System.Windows.Controls.Button).DataContext as StickyNote));            
        }

        public void DeleteNoteForm(Guid id)
        {
            Note toDelete = notes.First(note => note.current_note.Note_ID == id);
            notes.Remove(toDelete);
        }

        #region Updates

        private void btnUpdates_Click(object sender, RoutedEventArgs e)
        {
            //TODO: If no new updates are available, show a message to the user.
            AutoUpdater.Start("http://alexmfv.com/StickyUpdates/latest_update.xml");
        }

        private void AutoUpdater_CheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args.Error == null)
            {
                if (args.IsUpdateAvailable)
                {
                    DialogResult dialogResult;
                    if (args.Mandatory.Value)
                    {
                        dialogResult =
                            System.Windows.Forms.MessageBox.Show(
                                $@"There is new version {args.CurrentVersion} available. You are using version {args.InstalledVersion}. This is required update. Press Ok to begin updating the application.", @"Update Available",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                    }
                    else
                    {
                        dialogResult =
                            System.Windows.Forms.MessageBox.Show(
                                $@"There is new version {args.CurrentVersion} available. You are using version {args.InstalledVersion}. Do you want to update the application now?", @"Update Available",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);
                    }

                    // Uncomment the following line if you want to show standard update dialog instead.
                    // AutoUpdater.ShowUpdateForm(args);

                    if (dialogResult.Equals(System.Windows.Forms.DialogResult.Yes) || dialogResult.Equals(System.Windows.Forms.DialogResult.OK))
                    {
                        try
                        {
                            if (AutoUpdater.DownloadUpdate(args))
                            {
                                System.Windows.Forms.Application.Exit();
                            }
                        }
                        catch (Exception exception)
                        {
                            System.Windows.Forms.MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(@"There is no update available please try again later.", @"No update available",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (args.Error is WebException)
                {
                    System.Windows.Forms.MessageBox.Show(
                        @"There is a problem reaching update server. Please check your internet connection and try again later.",
                        @"Update Check Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(args.Error.Message,
                        args.Error.GetType().ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        private void lstNotes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstNotes.SelectedItem != null)
                ShowPressedNote(lstNotes.SelectedItem as StickyNote);
        }

        void ShowPressedNote(StickyNote pressedNote)
        {
            if (!notes.Exists(note => note.current_note.Note_ID == pressedNote.Note_ID))
            {
                Tuple<SolidColorBrush, SolidColorBrush> colors = new Tuple<SolidColorBrush, SolidColorBrush>(pressedNote.NoteColor, pressedNote.TitleColor);
                Note note = new Note(pressedNote.Note_ID, this);
                notes.Add(note);
                note.Show();
            }
            else
            {
                Note open = notes.First(note => note.current_note.Note_ID == pressedNote.Note_ID);
                notes.Remove(open);
                open.Close();
            }
        }

        private async void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            //If it's a connected user, save all the notes to the cloud first, then logout
            await WindowManager.OpenLogin(this);
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
                    System.Windows.Application.Current.Shutdown();
                }
                else
                    e.Cancel = true;
            }
        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(Globals.user.AuthType == AuthType.Guest)
            {
                LocalSave.LoadStickyNotes(Globals.user.ID);
            }
            else
            {
                if (!Network.HasInternetAccess())
                    LocalSave.LoadStickyNotes(Globals.user.ID);
                else
                {
                    if (!Globals.user.IsGuest)
                        await DAL.GetNotesFromUser(Globals.user.ID);
                }
            }

            LocalSave.SaveAllStickyNotes(Globals.user.ID);
            lstNotes.ItemsSource = Globals.stickies;
            txtUsername.Text = "Logged in as: " + Globals.user.Username;
        }
    }
}