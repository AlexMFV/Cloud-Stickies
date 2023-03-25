using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WindowsCloudStickies
{
    public static class WindowManager
    {
        public static void OpenManager(Account toClose)
        {
            try
            {
                NoteManager manager = new NoteManager();
                toClose.Close();
                manager.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public async static Task<bool> OpenLogin(NoteManager toClose)
        {
            bool result = Messager.Logout();

            if (result)
            {
                if (LocalSave.CheckCookieFile())
                {
                    Tuple<string, string, string, string> values = LocalSave.GetCookieFile();
                    LocalSave.DeleteCookieFile();

                    //TODO: Needs to check DB connection first, and if successful and logged in, proceed
                    await DAL.DeleteCookie(values.Item1, values.Item2); //From the database
                }
                toClose.isLogout = true;
                Globals.user = null;
                toClose.Close();
                Account login = new Account();
                login.Show();
                return true;
            }
            return false;
        }
    }
}
