using Newtonsoft.Json;
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
using Newtonsoft.Json;

namespace WindowsCloudStickies
{
    /// <summary>
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : Window
    {
        bool isInternal = false;

        public Account()
        {
            InitializeComponent();
            CheckCookie();
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Network.HasInternetAccess())
                    throw new NoInternetConnection();

                if (txtUserR.Text == "" || txtPassR.Password == "" || txtPass2R.Password == "")
                    MessageBox.Show("Username and password cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    if (txtPassR.Password != txtPass2R.Password)
                        MessageBox.Show("Passwords must match! Please try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        //TODO: Needs to check DB connection first, and if successful proceed

                        string result = await DAL.CreateUser(txtUserR.Text, txtPassR.Password);

                        switch (result)
                        {
                            case "OK": MessageBox.Show("User created successfully!");
                                string id = JsonConvert.DeserializeObject<string>(await DAL.GetUserID(Encrypt.ComputeHash(txtUserL.Text)));
                                Globals.user = new User(id, txtUserL.Text, AuthType.Login, true);
                                //LocalSave.CreateCookieFile(Guid.NewGuid().ToString(), Encrypt.ComputeHash(txtUserR.Text), Encrypt.ComputeHash(txtPassR.Password));
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
                if (!Network.HasInternetAccess())
                    throw new NoInternetConnection();

                if (txtUserL.Text == "" || txtPassL.Password == "")
                    MessageBox.Show("Username and password cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    //TODO: Needs to check DB connection first, and if successful proceed

                    string userID = Encrypt.ComputeHash(txtUserL.Text);
                    string pass = Encrypt.ComputeHash(txtPassL.Password);
                    if (await DAL.CheckUserLogin(userID, pass))
                    {
                        if (chkRemember.IsChecked == true)
                        {
                            Guid cookieID = Guid.NewGuid();
                            LocalSave.CreateCookieFile(cookieID.ToString(), userID, pass, txtUserL.Text);
                            await DAL.CreateCookie(cookieID.ToString(), userID);
                        }

                        string id = JsonConvert.DeserializeObject<string>(await DAL.GetUserID(Encrypt.ComputeHash(txtUserL.Text)));
                        Globals.user = new User(id, txtUserL.Text, AuthType.Login, true);
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
            Globals.user = new User(true);
            CloseLogin();
        }

        private void CloseLogin()
        {
            isInternal = true;
            WindowManager.OpenManager(this);
        }

        private async void CheckCookie()
        {
            if (LocalSave.CheckCookieFile()) //Checks if there is a cookie stored (locally)
            {
                Tuple<string, string, string, string> session = LocalSave.GetCookieFile();
                if (session is null)
                    Messager.CookieError();
                else
                {
                    if (await DAL.CheckCookie(session.Item2, session.Item1))
                    {
                        if (await DAL.CheckUserLogin(session.Item2, session.Item3))
                        {
                            string id = JsonConvert.DeserializeObject<string>(await DAL.GetUserID(session.Item2));
                            Globals.user = new User(id, session.Item4, AuthType.Login, true);
                            CloseLogin();
                        }
                    }
                    else
                    {
                        //We can just assume that there is no cookie on the DB, because if we had a valid cookie
                        //This code wouldn't be run.
                        LocalSave.DeleteCookieFile();
                        MessageBox.Show("You session has expired, please login again!", "Session Expired", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!isInternal)
                Application.Current.Shutdown();
        }
    }
}
