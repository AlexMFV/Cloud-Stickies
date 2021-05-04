using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WindowsCloudStickies
{
    public static class DAL
    {

        #region User Account

        #region Login

        //public static async Task<string> CheckUserLogin(string user, string pass)
        //{
        //
        //}

        #endregion

        #region Register

        //public static bool CreateUser(string user, string pass)
        //{
        //
        //}

        #endregion

        #region Misc

        public static async Task<string> CheckUserExists(string user)
        {
            string response = "";
            response = await API.Fetch(RequestType.GET, ("/api/user/" + user));
            return response;
        }

        #endregion

        #endregion
    }
}
