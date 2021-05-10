using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WindowsCloudStickies
{
    public static class LocalSave
    {
        //Save individual note
        //Read from files
        //Write to files
        //Convert from FileRead to Object
        //Convert from Object to JSON
        //Delete All Notes

        public static void LoadStickyNotes(Guid userID)
        {
            bool isCorrupted = false;

            string cacheDir = Path.Combine(Globals.AppData, "alexmfv_stickies"); //Change To Sticky Notes
            string userDir;

            if (userID != Guid.Empty)
                userDir = Path.Combine(cacheDir + "\\" + userID.ToString());
            else
                userDir = Path.Combine(cacheDir + "\\Guest");

            StickyNotes collection = new StickyNotes();

            //If the directory of the user exists
            if (Directory.Exists(userDir))
            {
                //Get all the files inside the directory
                foreach (string file in Directory.GetFiles(userDir))
                {
                    string content = File.ReadAllText(file);
                    if (content != "")
                    {
                        JObject obj = JObject.Parse(content);
                        StickyNote note = new StickyNote();
                        note = LoadNoteProperties(note, obj);
                        collection.Add(note);
                    }
                    else
                    {
                        isCorrupted = true;
                        File.Delete(file);
                    }
                }
            }

            if(isCorrupted)
                MessageBox.Show("One or more files we're corrupted and have been deleted!",
                    "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            Globals.stickies = collection;
        }

        public static void SaveAllStickyNotes(Guid userID)
        {
            string cacheDir = Path.Combine(Globals.AppData, "alexmfv_stickies");

            string userDir;
            if(userID != Guid.Empty)
                userDir = Path.Combine(cacheDir + "\\" + userID.ToString());
            else
                userDir = Path.Combine(cacheDir + "\\Guest");

            if (!Directory.Exists(cacheDir))
                Directory.CreateDirectory(cacheDir);

            if (!Directory.Exists(userDir))
                Directory.CreateDirectory(userDir);

            foreach (StickyNote note in Globals.stickies)
            {
                //From Stickies Collection To JSON To File
                using (StreamWriter file = File.CreateText(userDir + "\\" + note.NoteID.ToString()))
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    JObject toAdd = new JObject();
                    toAdd = AddNoteProperties(note, toAdd);
                    toAdd.WriteTo(writer);
                }
            }
        }

        public static void SaveStickyNote(Guid userID, Guid noteID)
        {
            string cacheDir = Path.Combine(Globals.AppData, "alexmfv_stickies");

            string userDir;
            if (userID != Guid.Empty)
                userDir = Path.Combine(cacheDir + "\\" + userID.ToString());
            else
                userDir = Path.Combine(cacheDir + "\\Guest");

            if (!Directory.Exists(cacheDir))
                Directory.CreateDirectory(cacheDir);

            if (!Directory.Exists(userDir))
                Directory.CreateDirectory(userDir);

            StickyNote note = Globals.stickies.GetNoteFromGUID(noteID);

            using (StreamWriter file = File.CreateText(userDir + "\\" + note.NoteID.ToString()))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                JObject toAdd = new JObject();
                toAdd = AddNoteProperties(note, toAdd);
                toAdd.WriteTo(writer);
            }
        }

        private static JObject AddNoteProperties(StickyNote note, JObject toAdd)
        {
            toAdd.Add("noteID", note.NoteID);
            toAdd.Add("noteText", note.NoteText);
            toAdd.Add("noteTitle", note.NoteTitle);
            toAdd.Add("noteColor", ColorRGBToString(note.NoteColor));
            toAdd.Add("titleColor", ColorRGBToString(note.TitleColor));
            toAdd.Add("dateCreated", note.DateCreated);
            toAdd.Add("baseFont", note.BaseFont);
            toAdd.Add("baseFontSize", note.BaseFontSize);
            toAdd.Add("baseFontColor", note.BaseFontColor);
            toAdd.Add("x", note.X);
            toAdd.Add("y", note.Y);
            toAdd.Add("width", note.Width);
            toAdd.Add("height", note.Height);
            return toAdd;
        }

        private static StickyNote LoadNoteProperties(StickyNote note, JObject toLoad)
        {
            note.NoteID = Guid.Parse(toLoad.Property("noteID").Value.ToString());
            note.NoteText = toLoad.Property("noteText").Value.ToString();
            note.NoteTitle = toLoad.Property("noteTitle").Value.ToString();
            note.NoteColor = Parser.ToColor(toLoad.Property("noteColor").Value.ToString());
            note.TitleColor = Parser.ToColor(toLoad.Property("titleColor").Value.ToString());
            note.DateCreated = DateTime.Parse(toLoad.Property("dateCreated").Value.ToString());
            note.BaseFont = toLoad.Property("baseFont").Value.ToString();
            note.BaseFontSize = int.Parse(toLoad.Property("baseFontSize").Value.ToString());
            note.BaseFontColor = toLoad.Property("baseFontColor").Value.ToString();
            note.X = int.Parse(toLoad.Property("x").Value.ToString());
            note.Y = int.Parse(toLoad.Property("y").Value.ToString());
            note.Width = int.Parse(toLoad.Property("width").Value.ToString());
            note.Height = int.Parse(toLoad.Property("height").Value.ToString());
            note.isNew = false;
            note.hasUpdated = false;
            return note;
        }

        public static void DeleteAllNotes(Guid userID)
        {
            string cacheDir = Path.Combine(Globals.AppData, "alexmfv_stickies");

            string userDir;
            if (userID != Guid.Empty)
                userDir = Path.Combine(cacheDir + "\\" + userID.ToString());
            else
                userDir = Path.Combine(cacheDir + "\\Guest");

            foreach (string filePath in Directory.GetFiles(userDir).ToList())
                File.Delete(filePath);
        }

        public static void DeleteNote(Guid userID, Guid noteID)
        {
            string cacheDir = Path.Combine(Globals.AppData, "alexmfv_stickies");

            string userDir;
            if (userID != Guid.Empty)
                userDir = Path.Combine(cacheDir + "\\" + userID.ToString());
            else
                userDir = Path.Combine(cacheDir + "\\Guest");

            string filepath = Path.Combine(userDir + "\\" + noteID);

            if (File.Exists(filepath))
                File.Delete(filepath);
            else
                MessageBox.Show("There was a problem deleting the note!");
        }

        public static void CreateCookieFile(string cookie, string user, string pass, string username)
        {
            string cacheDir = Path.Combine(Globals.AppData, "alexmfv_stickies", "config");

            if (!Directory.Exists(cacheDir))
                Directory.CreateDirectory(cacheDir);

            //From Accounts Collection To JSON To File
            using (StreamWriter file = File.CreateText(cacheDir + "\\cookie"))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                JObject obj = new JObject();
                obj.Add("cookie", cookie);
                obj.Add("usr", user);
                obj.Add("pwd", pass);
                obj.Add("username", username);

                obj.WriteTo(writer);
            }
        }

        public static bool CheckCookieFile()
        {
            string cacheDir = Path.Combine(Globals.AppData, "alexmfv_stickies", "config");
            if (File.Exists(cacheDir + "\\cookie"))
                return true;
            return false;
        }

        public static Tuple<string, string, string, string> GetCookieFile()
        {
            string cacheDir = Path.Combine(Globals.AppData, "alexmfv_stickies", "config");
            Tuple<string, string, string, string> values = null;

            if (new FileInfo(cacheDir + "\\cookie").Length > 0)
            {
                JObject obj = JObject.Parse(File.ReadAllText(cacheDir + "\\cookie"));

                values = new Tuple<string, string, string, string>(obj.Property("cookie").Value.ToString(),
                    obj.Property("usr").Value.ToString(), obj.Property("pwd").Value.ToString(), obj.Property("username").Value.ToString());
            }

            return values;
        }

        public static void DeleteCookieFile()
        {
            string cacheDir = Path.Combine(Globals.AppData, "alexmfv_stickies", "config", "cookie");

            if (File.Exists(cacheDir))
                File.Delete(cacheDir);
        }

        //TODO:
        //Save User Settings
        //Load User Settings

        public static string ColorRGBToString(SolidColorBrush brush)
        {
            return brush.Color.R.ToString() + ":" + brush.Color.G.ToString() + ":" + brush.Color.B.ToString();
        }
    }
}
