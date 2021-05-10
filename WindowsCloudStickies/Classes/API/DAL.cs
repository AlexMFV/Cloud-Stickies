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

        #region Login

        public static async Task<bool> CheckUserLogin(string user, string pass)
        {
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("user", user);
                parameters.Add("pass", pass);
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

        #region Notes

        public static async Task<bool> CreateNote(string user, StickyNote note)
        {
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("conID", Guid.NewGuid().ToString());
                parameters.Add("userID", user);
                parameters.Add("note_id", note.Note_ID.ToString());
                parameters.Add("noteText", note.NoteText);
                parameters.Add("noteTitle", note.NoteTitle);
                parameters.Add("noteColor", note.NoteColorHex);
                parameters.Add("titleColor", note.TitleColorHex);
                parameters.Add("dateCreated", note.DateCreated.ToString("yyyy-MM-dd HH:mm:ss"));
                parameters.Add("baseFont", note.BaseFont);
                parameters.Add("baseFontSize", note.BaseFontSize.ToString());
                parameters.Add("baseFontColor", note.BaseFontColor);
                parameters.Add("posX", note.PosX.ToString());
                parameters.Add("posY", note.PosY.ToString());
                parameters.Add("width", note.Width.ToString());
                parameters.Add("height", note.Height.ToString());
                parameters.Add("isClosed", note.IsClosed ? "1" : "0");
                parameters.Add("isLocked", note.IsLocked ? "1" : "0");

                bool response = Convert.ToBoolean(await API.Fetch(RequestType.POST, "/api/note/create", parameters));
                return response;
            }
            catch (Exception ex)
            {
                Messager.Process(ex);
                return false;
            }
        }

        public static async Task<bool> UpdateNote(string user, StickyNote note)
        {
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("note_id", note.Note_ID.ToString());
                parameters.Add("noteText", note.NoteText);
                parameters.Add("noteTitle", note.NoteTitle);
                parameters.Add("noteColor", note.NoteColorHex);
                parameters.Add("titleColor", note.TitleColorHex);
                parameters.Add("baseFont", note.BaseFont);
                parameters.Add("baseFontSize", note.BaseFontSize.ToString());
                parameters.Add("baseFontColor", note.BaseFontColor);
                parameters.Add("posX", note.PosX.ToString());
                parameters.Add("posY", note.PosY.ToString());
                parameters.Add("width", note.Width.ToString());
                parameters.Add("height", note.Height.ToString());
                parameters.Add("isClosed", note.IsClosed ? "1" : "0");
                parameters.Add("isLocked", note.IsLocked ? "1" : "0");

                bool response = Convert.ToBoolean(await API.Fetch(RequestType.POST, "/api/note/update", parameters));
                return response;
            }
            catch (Exception ex)
            {
                Messager.Process(ex);
                return false;
            }
        }

        public static async Task<bool> GetNotesFromUser(Guid id)
        {
            try
            {
                object response = await API.Fetch(RequestType.GET, "/api/note/get/" + id.ToString());
                List<StickyNote> notes = JsonConvert.DeserializeObject<List<StickyNote>>((string)response);
                StickyNotes stickies = new StickyNotes();

                foreach(StickyNote note in notes)
                {
                    note.BaseFont = note.BaseFont == "null" ? "" : note.BaseFont;
                    note.BaseFontColor = note.BaseFontColor == "null" ? "" : note.BaseFontColor;
                    stickies.Add(note);
                }

                Globals.stickies = stickies;
                return true;
            }
            catch (Exception ex)
            {
                Messager.Process(ex);
                return false;
            }
        }

        #endregion

        #region Cookies

        public static async Task<string> CreateCookie(string cookie, string encryptedUser)
        {
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("id", Guid.NewGuid().ToString());
                parameters.Add("userID", encryptedUser);
                parameters.Add("cookieID", cookie);
                parameters.Add("expire", DateTime.Now.AddDays(7).ToString("yyyy-MM-dd HH:mm:ss"));

                string response = (string)await API.Fetch(RequestType.POST, "/api/cookie/create", parameters);
                return response;
            }
            catch (Exception ex)
            {
                Messager.Process(ex);
                return null;
            }
        }

        public static async Task<bool> CheckCookie(string userID, string cookie)
        {
            try
            {
                bool response = Convert.ToBoolean(await API.Fetch(RequestType.GET, "/api/cookie/check/" + userID + "/" + cookie));
                return response;
            }
            catch (Exception ex)
            {
                Messager.Process(ex);
                return false;
            }
        }

        public static async Task<bool> DeleteCookie(string cookie, string userID)
        {
            try
            {
                bool response = Convert.ToBoolean(await API.Fetch(RequestType.DELETE, "/api/cookie/delete/" + userID + "/" + cookie));
                return response;
            }
            catch (Exception ex)
            {
                Messager.Process(ex);
                return false;
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
                string response = (string)await API.Fetch(RequestType.GET, "/api/getUserID/" + user);
                return response;
            }
            catch (Exception ex)
            {
                Messager.Process(ex);
                return null;
            }
        }

        #endregion

        #region Common

        //COMMON Functions

        #endregion
    }
}
