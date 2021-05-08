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

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtUserR.Text == "" || txtPassR.Password == "" || txtPass2R.Password == "")
                    MessageBox.Show("Username and password cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    if (txtPassR.Password != txtPass2R.Password)
                        MessageBox.Show("Passwords must match! Please try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        string result = await DAL.CreateUser(txtUserR.Text, txtPassR.Password);

                        switch (result)
                        {
                            case "OK": MessageBox.Show("User created successfully!");
                                UserSuccessfullyAuthenticated = "Auth";
                                CloseLogin(); break;
                            case "ERROR": MessageBox.Show("There was an error creating the account, please try again later!"); break;
                            case "EXISTS": MessageBox.Show("This username already exists, please choose another one!"); break;
                            case "EX": MessageBox.Show("An internal server error occured, please try again later!"); break;
                        }
                    }
                }
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
                if(txtUserL.Text == "" || txtPassL.Password == "")
                    MessageBox.Show("Username and password cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    if (await DAL.CheckUserLogin(txtUserL.Text, txtPassL.Password))
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
