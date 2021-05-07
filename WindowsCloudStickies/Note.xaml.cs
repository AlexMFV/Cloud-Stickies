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
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using System.IO;

namespace WindowsCloudStickies
{
    /// <summary>
    /// Interaction logic for Note.xaml
    /// </summary>
    public partial class Note : Window
    {
        public StickyNote current_note;
        System.Timers.Timer saveWait = new System.Timers.Timer();
        NoteManager manager;

        double h = 0;

        public Note(Guid _noteID, NoteManager _manager)
        {
            InitializeComponent();
            current_note = Globals.stickies.GetNoteFromGUID(_noteID);

            this.ShowInTaskbar = false;

            textCanvas.Background = current_note.noteColor;
            gripBar.Background = current_note.titleColor;

            saveWait.Interval = 3000;
            saveWait.Elapsed += SaveWait_Elapsed;
            saveWait.AutoReset = false;

            manager = _manager;

            LoadNoteProperties();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (!this.current_note.isLocked)
            {
                MessageBoxResult res = MessageBox.Show("Are you sure you want to delete the note?", "Delete Note", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                    CloseAndDeleteNote();
            }
        }

        private void gripBar_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left && !current_note.isLocked)
                this.DragMove();
        }

        private void btnLockNote_Click(object sender, RoutedEventArgs e)
        {
            current_note.isLocked = !current_note.isLocked;
            if (current_note.isLocked)
            {
                btnLockNote.Background = (SolidColorBrush)Application.Current.Resources["ButtonSelected"]; //Set button Toggle color
                this.ResizeMode = ResizeMode.NoResize;
                textCanvas.IsReadOnly = true;
            }
            else
            {
                btnLockNote.Background = (SolidColorBrush)Application.Current.Resources["ButtonUnselected"];
                this.ResizeMode = ResizeMode.CanResizeWithGrip;
                textCanvas.IsReadOnly = false;
            }
        }

        private void noteWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.Height < 15 && !current_note.isClosed)
                this.Height = 30;

            if (this.Width <= 100)
                this.Width = 100;

            this.current_note.width = (int)this.Width;
            this.current_note.height = (int)this.Height;

            saveWait.Stop();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() => ChangeSavedState(State.NotSaved)));
            saveWait.Start();
        }

        private void gripBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && !current_note.isClosed)
            {
                current_note.isClosed = !current_note.isClosed;
                h = this.Height;
                this.Height = 14;
                this.ResizeMode = ResizeMode.NoResize;
                return;
            }

            if(e.ClickCount == 2 && current_note.isClosed)
            {
                current_note.isClosed = !current_note.isClosed;
                this.Height = h;
                this.ResizeMode = ResizeMode.CanResizeWithGrip;
                return;
            }
        }

        private void btnHideNote_Click(object sender, RoutedEventArgs e)
        {
            //Save Note, position, color and size
            manager.DeleteNoteForm(this.current_note.noteID);
            this.Close();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            //Open Settings Context Menu
            //Change Color
            //Always On Top (Toggle)
            //Force Save (Ctrl+S, does the same, when focused)
        }

        private void noteWindow_StateChanged(object sender, EventArgs e)
        {
            //Prevent the notes from maximizing, when grabbed to the top
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
        }

        private void textCanvas_TextChanged(object sender, TextChangedEventArgs e)
        {
            saveWait.Stop();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() => ChangeSavedState(State.NotSaved)));
            saveWait.Start();
        }

        private void SaveWait_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            saveWait.Stop();
            this.current_note.noteText = (string)Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                new Func<string>(() => TextFromRichTextBox(this.textCanvas)));
            this.current_note.ChangeTitleToParagraph();
            SaveFullNote();
        }

        string TextFromRichTextBox(RichTextBox rtb)
        {
            foreach(Block block in rtb.Document.Blocks)
            {
                break;
            }

            return new TextRange( rtb.Document.ContentStart,
                rtb.Document.ContentEnd).Text;
        }

        private void textCanvas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool ctrlV = e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.V;
            bool shiftIns = e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.Insert;
            if (ctrlV || shiftIns)
                if (Clipboard.ContainsImage())
                {
                    BitmapSource source = Clipboard.GetImage();
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.QualityLevel = 100;

                    using (MemoryStream stream = new MemoryStream())
                    {
                        encoder.Frames.Add(BitmapFrame.Create(source));
                        encoder.Save(stream);
                        byte[] bit = stream.ToArray();
                        stream.Close();
                        Paragraph imageBlock = new Paragraph();
                        Figure image = new Figure(imageBlock);
                        this.textCanvas.Document.Blocks.Add(image);
                        string base64 = Convert.ToBase64String(bit); //Base64 String to add to the noteText, then convert it back to image (when coming from DB)
                    }
                    //Allow the user to also paste text
                    e.Handled = true;
                }
        }

        public void ChangeSavedState(State state)
        {
            switch (state)
            {
                case State.NotSaved: savedIndicator.Background = (System.Windows.Media.Brush)Application.Current.Resources["NotSaved"]; break;
                case State.Saved: savedIndicator.Background = (System.Windows.Media.Brush)Application.Current.Resources["Saved"]; break;
                case State.Unknown: savedIndicator.Background = (System.Windows.Media.Brush)Application.Current.Resources["Unknown"]; break;
            }
        }

        public async void SaveFullNote()
        {
            try
            {
                Globals.stickies[Globals.stickies.GetNoteIndex(this.current_note.noteID)] = this.current_note;

                //await Task.Factory.StartNew(() => LocalSave.SaveStickyNote(Guid.NewGuid(), this.current_note.noteID)); //DEBUG: Change later to user ID)

                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() => LocalSave.SaveStickyNote(Guid.NewGuid(), this.current_note.noteID)));

                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() => ChangeSavedState(State.Saved)));
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        public void SaveNoteSettings()
        {
            try
            {
                Globals.stickies[Globals.stickies.GetNoteIndex(this.current_note.noteID)] = this.current_note;
                //Task.Factory.StartNew(() => LocalSave.SaveStickyNote(Guid.NewGuid(), this.current_note.noteID)); //DEBUG: Change later to user ID)

                //TEST: add await if not working or uncomment code above, and delete this one, below
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() => LocalSave.SaveStickyNote(Guid.NewGuid(), this.current_note.noteID)));
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        public void CloseAndDeleteNote()
        {
            manager.DeleteNoteForm(this.current_note.noteID);
            Globals.stickies.RemoveAt(Globals.stickies.GetNoteIndex(this.current_note.noteID));
            manager.updateList();
            LocalSave.DeleteNote(Guid.NewGuid(), this.current_note.noteID);
            this.Close();
        }

        public void CloseNote()
        {
            manager.DeleteNoteForm(this.current_note.noteID);
            this.Close();
        }

        private void savedIndicator_Click(object sender, RoutedEventArgs e)
        {
            saveWait.Stop();
            SaveFullNote();
        }

        private void noteWindow_LocationChanged(object sender, EventArgs e)
        {
            this.current_note.x = (int)this.Left;
            this.current_note.y = (int)this.Top;

            saveWait.Stop();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() => ChangeSavedState(State.NotSaved)));
            saveWait.Start();
        }

        public void LoadNoteProperties()
        {
            this.textCanvas.AppendText(this.current_note.noteText); //Load Text

            //This is necessary to have, because if this code was on the Loaded event
            //the note would ignore the positions and size set, and move randomly across the screen
            if ((this.current_note.width > 0 && this.current_note.height > 0) &&
                (this.current_note.x > 0 && this.current_note.y > 0))
            {
                this.Width = this.current_note.width;
                this.Height = this.current_note.height;
                this.Left = this.current_note.x;
                this.Top = this.current_note.y;
            }
        }

        private void noteWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if ((this.current_note.width <= 0 || this.current_note.height <= 0) ||
                (this.current_note.x <= 0 || this.current_note.y <= 0))
            {
                this.current_note.x = (int)this.Left;
                this.current_note.y = (int)this.Top;
                this.current_note.width = (int)this.Width;
                this.current_note.height = (int)this.Height;
            }

            saveWait.Start();
        }

        //private void noteWindow_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    gripBar.Visibility = Visibility.Visible;
        //    btnHideNote.Visibility = Visibility.Visible;
        //    btnClose.Visibility = Visibility.Visible;
        //    btnLockNote.Visibility = Visibility.Visible;
        //    btnSettings.Visibility = Visibility.Visible;
        //}

        //private void noteWindow_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    gripBar.Visibility = Visibility.Hidden;
        //    btnHideNote.Visibility = Visibility.Hidden;
        //    btnClose.Visibility = Visibility.Hidden;
        //    btnLockNote.Visibility = Visibility.Hidden;
        //    btnSettings.Visibility = Visibility.Hidden;
        //}
    }
}