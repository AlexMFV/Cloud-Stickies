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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindowsCloudStickies
{
    /// <summary>
    /// Interaction logic for NoteManager.xaml
    /// </summary>
    public partial class NoteManager : Window
    {
        List<Note> notes = new List<Note>();

        public NoteManager()
        {
            InitializeComponent();            
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Note n = new Note();
            notes.Add(n);
            n.Show();
        }

        private void btnRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            foreach(Note n in notes)
            {
                n.Close();
            }
        }
    }
}
