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

        public static async Task<bool> CheckUserLogin(string user, string pass)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("user", Encrypt.ComputeHash(user));
            parameters.Add("pass", Encrypt.ComputeHash(pass));
            bool response = Convert.ToBoolean(await API.FetchPOST(RequestType.POST, ReadType.String, "/api/login", parameters));
            return response;
        }

        #endregion

        #region Register

        //public static bool CreateUser(string user, string pass)
        //{
        //
        //}

        #endregion

        #region Misc

        public static async Task<bool> CheckUserExists(string user)
        {
            bool response = (bool)await API.Fetch(RequestType.GET, ReadType.String, ("/api/user/" + user));
            return response;
        }

        #endregion

        #endregion

        #region Common

        //COMMON Functions

        #endregion
    }
}
