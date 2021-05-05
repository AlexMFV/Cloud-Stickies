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

namespace WindowsCloudStickies
{
    /// <summary>
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : Window
    {
        public string UserSuccessfullyAuthenticated { get; private set; }

        public Account()
        {
            InitializeComponent();
            //Check if user is logged in, and the login hasn't expired yet
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserSuccessfullyAuthenticated = "Auth";
                CloseLogin();
            }
            catch(Exception ex)
            {
                Messager.Process(ex);
            }
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(txtUserL.Text == "" || txtPassL.Text == "")
                    MessageBox.Show("Username and password cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    if (await DAL.CheckUserLogin(txtUserL.Text, txtPassL.Text))
                    {
                        UserSuccessfullyAuthenticated = "Auth";
                        CloseLogin();
                    }
                    else
                    {
                        MessageBox.Show("The credentials are not valid!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Messager.Process(ex);
            }
        }

        private void lblGuest_MouseEnter(object sender, MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.White);
            lblGuest.Foreground = brush;
            this.Cursor = Cursors.Hand;
        }

        private void lblGuest_MouseLeave(object sender, MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.CornflowerBlue);
            lblGuest.Foreground = brush;
            this.Cursor = Cursors.Arrow;
        }

        private void lblGuest_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UserSuccessfullyAuthenticated = "Guest";
            CloseLogin();
        }

        private void CloseLogin()
        {
            WindowManager.OpenManager(this);
        }
    }
}
