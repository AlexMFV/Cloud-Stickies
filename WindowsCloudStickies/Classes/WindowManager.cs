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

        public static void OpenLogin(NoteManager toClose)
        {
            Account login = new Account();
            bool result = Messager.Logout();

            if (result)
            {
                toClose.isLogout = true;
                Globals.user = null;
                toClose.Close();
                login.Show();
            }
        }
    }
}
