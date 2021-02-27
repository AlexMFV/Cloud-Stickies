﻿using System;
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

namespace WindowsCloudStickies
{
    /// <summary>
    /// Interaction logic for Note.xaml
    /// </summary>
    public partial class Note : Window
    {
        bool isLocked = false; //Pass this to the Note Class later
        bool isClosed = false;

        double h = 0;


        public Note()
        {
            InitializeComponent();
            randomizeColor();
            this.ShowInTaskbar = false;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //Ask user if sure to delete
            //Shift + Click on close button (Force delete without asking)
            //Delete note if not locked
            //If note is locked requires Shift + Click to delete
            MessageBox.Show("Are you sure you want to delete the note?", "Delete Note", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            //Delete the note
            //Delete note from Database
            this.Close();
        }

        private void gripBar_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left && !isLocked)
                this.DragMove();
        }
        void randomizeColor()
        {
            Random r = new Random();
            int value = r.Next(0, 4);

            switch (value)
            {
                case 0: changeColor("GreenNote", "GreenTitle"); break;
                case 1: changeColor("PinkNote", "PinkTitle"); break;
                case 2: changeColor("AquaNote", "AquaTitle"); break;
                case 3: changeColor("OrangeNote", "OrangeTitle"); break;
                default: break;
            }
        }

        void changeColor(string bg, string topbar)
        {
            noteBackground.Background = (SolidColorBrush)Application.Current.Resources[bg];
            gripBar.Background = (SolidColorBrush)Application.Current.Resources[topbar];
        }

        private void btnLockNote_Click(object sender, RoutedEventArgs e)
        {
            isLocked = !isLocked;
            if (isLocked)
            {
                btnLockNote.Background = (SolidColorBrush)Application.Current.Resources["ButtonSelected"]; //Set button Toggle color
                this.ResizeMode = ResizeMode.NoResize;
            }
            else
            {
                btnLockNote.Background = (SolidColorBrush)Application.Current.Resources["ButtonUnselected"];
                this.ResizeMode = ResizeMode.CanResizeWithGrip;
            }
        }

        private void noteWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.Height < 15 && !isClosed)
                this.Height = 30;

            if (this.Width <= 100)
                this.Width = 100;
        }

        private void gripBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && !isClosed)
            {
                isClosed = !isClosed;
                h = this.Height;
                this.Height = 14;
                this.ResizeMode = ResizeMode.NoResize;
                return;
            }

            if(e.ClickCount == 2 && isClosed)
            {
                isClosed = !isClosed;
                this.Height = h;
                this.ResizeMode = ResizeMode.CanResizeWithGrip;
                return;
            }
        }

        private void btnHideNote_Click(object sender, RoutedEventArgs e)
        {
            //Save Note, position, color and size
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
    }
}
