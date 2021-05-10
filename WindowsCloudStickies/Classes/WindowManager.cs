using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsCloudStickies
{
    public static class WindowManager
    {
        public static void OpenManager(Account toClose)
        {
            NoteManager manager = new NoteManager();
            toClose.Close();
            manager.Show();
        }

        public async static Task<bool> OpenLogin(NoteManager toClose)
        {
            bool result = Messager.Logout();

            if (result)
            {
                if (LocalSave.CheckCookieFile())
                {
                    Tuple<string, string, string> values = LocalSave.GetCookieFile();
                    LocalSave.DeleteCookieFile();
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
