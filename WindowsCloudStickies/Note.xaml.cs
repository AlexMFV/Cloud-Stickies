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
using System.Text.RegularExpressions;

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
        bool isInitialRender = false;

        double h = 0;

        public Note(Guid _noteID, NoteManager _manager)
        {
            MakeWin();
            InitializeComponent();

            current_note = Globals.stickies.GetNoteFromGUID(_noteID);

            this.ShowInTaskbar = false;

            textCanvas.Background = current_note.NoteColor;
            gripBar.Background = current_note.TitleColor;

            saveWait.Interval = 3000;
            saveWait.Elapsed += SaveWait_Elapsed;
            saveWait.AutoReset = false;

            manager = _manager;

            LoadNoteProperties();

            textCanvas.IsDocumentEnabled = true;

            RemoveFromAltTab();
        }

        public void SaveRichTextXAML()
        {
            TextRange range;
            FileStream fStream;
            range = new TextRange(textCanvas.Document.ContentStart, textCanvas.Document.ContentEnd);
            fStream = new FileStream("D:\\test.xaml", FileMode.Create);
            range.Save(fStream, DataFormats.Xaml);
            fStream.Close();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hP, IntPtr hC, string sC, string sW);

        void MakeWin()
        {
            IntPtr nWinHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Progman", null);
            nWinHandle = FindWindowEx(nWinHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
            SetParent(new WindowInteropHelper(this).Handle, nWinHandle);
        }

        /// <summary>
        /// Method responsible for hiding the notes from the Alt+Tab menu
        /// </summary>
        protected void RemoveFromAltTab()
        {
            Globals.helperWindow.Show(); // We need to show window before set is as owner to our main window
            this.Owner = Globals.helperWindow; // Okey, this will result to disappear icon for main window.
            Globals.helperWindow.Hide(); // Hide helper window just in case
        }

        private async void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (!this.current_note.IsLocked)
            {
                MessageBoxResult res = MessageBox.Show("Are you sure you want to delete the note?", "Delete Note", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                    await CloseAndDeleteNote();
            }
        }

        private void gripBar_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left && !current_note.IsLocked)
            {
                this.DragMove();
                this.current_note.hasUpdated = true;
            }
        }

        private void btnLockNote_Click(object sender, RoutedEventArgs e)
        {
            current_note.IsLocked = !current_note.IsLocked;
            LockNote();
            this.current_note.hasUpdated = true;

            saveWait.Stop();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() => ChangeSavedState(State.NotSaved)));
            saveWait.Start();
        }

        public void LockNote()
        {
            if (current_note.IsLocked)
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
            if (!isInitialRender)
            {
                if (this.Height < 15 && !current_note.IsClosed)
                    this.Height = 30;

                if (this.Width <= 100)
                    this.Width = 100;

                this.current_note.Width = (int)this.Width;
                this.current_note.Height = (int)this.Height;
                this.current_note.hasUpdated = true;

                saveWait.Stop();
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() => ChangeSavedState(State.NotSaved)));
                saveWait.Start();
            }
        }

        private void gripBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && !current_note.IsClosed)
            {
                current_note.IsClosed = !current_note.IsClosed;
                CloseTheNote();   
                return;
            }

            if(e.ClickCount == 2 && current_note.IsClosed)
            {
                current_note.IsClosed = !current_note.IsClosed;
                OpenTheNote();
                return;
            }
        }

        public void CloseTheNote()
        {
            h = this.Height;
            this.Height = 14;
            this.ResizeMode = ResizeMode.NoResize;
            this.current_note.hasUpdated = true;
        }

        public void OpenTheNote()
        {
            this.Height = h;
            this.ResizeMode = ResizeMode.CanResizeWithGrip;
            this.current_note.hasUpdated = true;
        }

        private void btnHideNote_Click(object sender, RoutedEventArgs e)
        {
            //Save Note, position, color and size
            manager.DeleteNoteForm(this.current_note.Note_ID);
            SaveRichTextXAML();
            this.Close();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ContextMenu contextMenu = btn.ContextMenu;
            contextMenu.PlacementTarget = btn;
            contextMenu.IsOpen = true;
            e.Handled = true;
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

            //if (this.current_note != null && this.current_note.hasUpdated != true && !this.current_note.isNew && this.current_note.NoteText != null &&
                //e.Changes.Count > 0 && e.Changes.First<TextChange>().AddedLength < this.current_note.NoteText.Length)
                if(this.current_note != null && !this.current_note.isNew)
                    this.current_note.hasUpdated = true;

            saveWait.Start();
        }

        private void SaveWait_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            saveWait.Stop();
            this.current_note.NoteText = (string)Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                new Func<string>(() => TextFromRichTextBox(this.textCanvas)));
            this.current_note.ChangeTitleToParagraph();
            SaveFullNote();
        }

        string TextFromRichTextBox(RichTextBox rtb)
        {
            return new TextRange( rtb.Document.ContentStart,
                rtb.Document.ContentEnd).Text;
        }

        private void textCanvas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            /* This is where the image will be implemented
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
            */
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
                Globals.stickies[Globals.stickies.GetNoteIndex(this.current_note.Note_ID)] = this.current_note;

                //await Task.Factory.StartNew(() => LocalSave.SaveStickyNote(Globals.user.ID,, this.current_note.noteID)); //DEBUG: Change later to user ID)

                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() => LocalSave.SaveStickyNote(Globals.user.ID, this.current_note.Note_ID)));

                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                   new Action(() => manager.updateList()));

                if (Globals.user.AuthType != AuthType.Guest)
                {
                    if (this.current_note.isNew)
                    {
                        await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                            new Action(async () => await DAL.CreateNote(Globals.user.ID.ToString(), this.current_note)));
                        this.current_note.isNew = false;
                    }
                    else
                    {
                        if(this.current_note.hasUpdated)
                            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                                new Action(async () => await DAL.UpdateNote(Globals.user.ID.ToString(), this.current_note)));
                        this.current_note.hasUpdated = false;
                    }
                }

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
                Globals.stickies[Globals.stickies.GetNoteIndex(this.current_note.Note_ID)] = this.current_note;
                //Task.Factory.StartNew(() => LocalSave.SaveStickyNote(Globals.user.ID,, this.current_note.noteID)); //DEBUG: Change later to user ID)

                //TEST: add await if not working or uncomment code above, and delete this one, below
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() => LocalSave.SaveStickyNote(Globals.user.ID, this.current_note.Note_ID)));
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        public async Task CloseAndDeleteNote()
        {
            manager.DeleteNoteForm(this.current_note.Note_ID);
            Globals.stickies.RemoveAt(Globals.stickies.GetNoteIndex(this.current_note.Note_ID));
            manager.updateList();

            List<string> noteIDs = new List<string>();
            noteIDs.Add(this.current_note.Note_ID.ToString());
            await DAL.DeleteNotesFromUser(Globals.user.ID, noteIDs);

            LocalSave.DeleteNote(Globals.user.ID, this.current_note.Note_ID);
            this.Close();
        }

        public void CloseNote()
        {
            manager.DeleteNoteForm(this.current_note.Note_ID);
            this.Close();
        }

        private void savedIndicator_Click(object sender, RoutedEventArgs e)
        {
            saveWait.Stop();
            this.current_note.NoteText = (string)Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                new Func<string>(() => TextFromRichTextBox(this.textCanvas)));
            this.current_note.ChangeTitleToParagraph();
            this.current_note.hasUpdated = true;
            SaveFullNote();
        }

        private void noteWindow_LocationChanged(object sender, EventArgs e)
        {
            this.current_note.PosX = (int)this.Left;
            this.current_note.PosY = (int)this.Top;

            saveWait.Stop();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() => ChangeSavedState(State.NotSaved)));
            saveWait.Start();
        }

        public void LoadNoteProperties()
        {
            this.textCanvas.AppendText(this.current_note.NoteText); //Load Text

            //This is necessary to have, because if this code was on the Loaded event
            //the note would ignore the positions and size set, and move randomly across the screen
            if ((this.current_note.Width > 0 && this.current_note.Height > 0) &&
                (this.current_note.PosX > 0 && this.current_note.PosY > 0))
            {
                this.Width = this.current_note.Width;
                this.Height = this.current_note.Height;
                this.Left = this.current_note.PosX;
                this.Top = this.current_note.PosY;
            }

            isInitialRender = true;

            LockNote();
            if (this.current_note.IsClosed)
                CloseTheNote();

            isInitialRender = false;
        }

        private void noteWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if ((this.current_note.Width <= 0 || this.current_note.Height <= 0) ||
                (this.current_note.PosX <= 0 || this.current_note.PosY <= 0))
            {
                this.current_note.PosX = (int)this.Left;
                this.current_note.PosY = (int)this.Top;
                this.current_note.Width = (int)this.Width;
                this.current_note.Height = (int)this.Height;
            }

            saveWait.Start();
        }

        private void changeToGreen(object sender, RoutedEventArgs e)
        {
            this.current_note.NoteColor = NoteColors.GreenNote;
            this.current_note.TitleColor = NoteColors.GreenTitle;
            ChangeColor();
            ForceSave();
        }

        private void changeToPink(object sender, RoutedEventArgs e)
        {
            this.current_note.NoteColor = NoteColors.PinkNote;
            this.current_note.TitleColor = NoteColors.PinkTitle;
            ChangeColor();
            ForceSave();
        }

        private void changeToYellow(object sender, RoutedEventArgs e)
        {
            this.current_note.NoteColor = NoteColors.YellowNote;
            this.current_note.TitleColor = NoteColors.YellowTitle;
            ChangeColor();
            ForceSave();
        }

        private void changeToBlue(object sender, RoutedEventArgs e)
        {
            this.current_note.NoteColor = NoteColors.AquaNote;
            this.current_note.TitleColor = NoteColors.AquaTitle;
            ChangeColor();
            ForceSave();
        }

        private void changeToOrange(object sender, RoutedEventArgs e)
        {
            this.current_note.NoteColor = NoteColors.OrangeNote;
            this.current_note.TitleColor = NoteColors.OrangeTitle;
            ChangeColor();
            ForceSave();
        }

        private void MakeBold(object sender, RoutedEventArgs e)
        {
            this.textCanvas.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Bold);
            ForceSave();
        }

        private void MakeItalic(object sender, RoutedEventArgs e)
        {
            this.textCanvas.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Italic);
            ForceSave();
        }

        private void MakeNormal(object sender, RoutedEventArgs e)
        {
            this.textCanvas.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Normal);
            ForceSave();
        }

        private void MakeStyleNormal(object sender, RoutedEventArgs e)
        {
            this.textCanvas.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Normal);
            ForceSave();
        }

        private void ChangeSize4(object sender, RoutedEventArgs e)
        {
            ChangeSize(4);
        }

        private void ChangeSize8(object sender, RoutedEventArgs e)
        {
            ChangeSize(8);
        }

        private void ChangeSize12(object sender, RoutedEventArgs e)
        {
            ChangeSize(12);
        }

        private void ChangeSize16(object sender, RoutedEventArgs e)
        {
            ChangeSize(16);
        }

        private void ChangeSize20(object sender, RoutedEventArgs e)
        {
            ChangeSize(20);
        }

        public void ChangeColor()
        {
            textCanvas.Background = current_note.NoteColor;
            gripBar.Background = current_note.TitleColor;
        }

        public void ChangeSize(double size)
        {
            this.textCanvas.Selection.ApplyPropertyValue(Run.FontSizeProperty, size);
            ForceSave();
        }

        public void ForceSave()
        {
            saveWait.Stop();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() => ChangeSavedState(State.NotSaved)));
            this.current_note.hasUpdated = true;
            saveWait.Start();
        }

        private void AddSelectionDateTime(object sender, RoutedEventArgs e)
        {
            this.textCanvas.Selection.Text = DateTime.Now.ToString();
        }

        private void AddSelectionTimestamp(object sender, RoutedEventArgs e)
        {
            this.textCanvas.Selection.Text = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        }

        private void CreateURL(object sender, RoutedEventArgs e)
        {
            Paragraph paraSelection = new Paragraph();
            paraSelection.Margin = new Thickness(0); // remove indent between paragraphs

            string linkURL = textCanvas.Selection.Text;
            Regex reg = new Regex(@"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?"); //Regex to detect URL

            if (reg.IsMatch(linkURL))
            {
                Hyperlink link = new Hyperlink();
                link.IsEnabled = true;
                link.Inlines.Add(linkURL);
                link.NavigateUri = new Uri(linkURL);
                link.RequestNavigate += (newSender, args) => Process.Start(args.Uri.ToString()); //Click event for URL
                paraSelection.Inlines.Add(link);

                textCanvas.Document.Blocks.InsertBefore(textCanvas.CaretPosition.Paragraph, paraSelection); //Insert clickable URL
                textCanvas.Document.Blocks.Remove(textCanvas.CaretPosition.Paragraph); //Remove Text URL
            }
        }
    }
}