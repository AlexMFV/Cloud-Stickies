using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Newtonsoft.Json;

namespace WindowsCloudStickies
{
    public static class DAL
    {

        #region User Account

        #region Login

        public static async Task<bool> CheckUserLogin(string user, string pass)
        {
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("user", Encrypt.ComputeHash(user));
                parameters.Add("pass", Encrypt.ComputeHash(pass));
                bool response = Convert.ToBoolean(await API.Fetch(RequestType.POST, "/api/login", parameters));
                return response;
            }
            catch(Exception ex)
            {
                Messager.Process(ex);
                return false;
            }
        }

        #endregion

        #region Register

        public static async Task<string> CreateUser(string user, string pass)
        {
            try
            {
                //Check if user exists
                if (!await CheckUserExists(Encrypt.ComputeHash(user)))
                {
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("id", Guid.NewGuid().ToString());
                    parameters.Add("user", Encrypt.ComputeHash(user));
                    parameters.Add("pass", Encrypt.ComputeHash(pass));
                    bool response = Convert.ToBoolean(await API.Fetch(RequestType.POST, "/api/register", parameters));

                    if (response)
                        return "OK";
                    else
                        return "ERROR";
                }
                return "EXISTS";
            }
            catch (Exception ex)
            {
                Messager.Process(ex);
                return "EX";
            }
        }

        #endregion

        #region Misc

        public static async Task<bool> CheckUserExists(string user)
        {
            try
            {
                bool response = Convert.ToBoolean(await API.Fetch(RequestType.GET, "/api/user/" + user));
                return response;
            }
            catch (Exception ex)
            {
                Messager.Process(ex);
                return false;
            }
        }

        public static async Task<string> GetUserID(string user)
        {
            try
            {                
                return (string)(await API.Fetch(RequestType.GET, "/api/getUserID/" + Encrypt.ComputeHash(user)));
            }
            catch (Exception ex)
            {
                Messager.Process(ex);
                return null;
            }
        }
         
        #endregion

        #endregion

        #region Common

        //COMMON Functions

        #endregion
    }
}
