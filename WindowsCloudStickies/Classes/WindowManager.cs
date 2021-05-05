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
            NoteManager manager = new NoteManager(toClose.UserSuccessfullyAuthenticated);
            toClose.Close();
            manager.Show();
        }

        public static void OpenLogin(NoteManager toClose)
        {
            Account login = new Account();
            toClose.Close();
            login.Show();
        }
    }
}
