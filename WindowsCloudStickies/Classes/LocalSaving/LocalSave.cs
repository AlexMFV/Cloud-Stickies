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
            //-------------------------------DEBUG ONLY-----------------------------------
            userID = Guid.Parse("3878d481-54e2-40a1-95e7-af664a1fd140");

            string cacheDir = Path.Combine(Globals.AppData, "alexmfv_stickies"); //Change To Sticky Notes
            string userDir = Path.Combine(cacheDir + "\\" + userID.ToString());

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
                        note.noteID = Guid.Parse(obj.Property("noteID").Value.ToString());
                        note.noteText = obj.Property("noteText").Value.ToString();
                        note.noteTitle = obj.Property("noteTitle").Value.ToString();
                        note.noteColor = Parser.ToColor(obj.Property("noteColor").Value.ToString());
                        note.titleColor = Parser.ToColor(obj.Property("titleColor").Value.ToString());
                        note.dateCreated = DateTime.Parse(obj.Property("dateCreated").Value.ToString());
                        note.baseFont = obj.Property("baseFont").Value.ToString();
                        note.baseFontSize = obj.Property("baseFontSize").Value.ToString();
                        note.baseFontColor = obj.Property("baseFontColor").Value.ToString();
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
            //-------------DEBUG ONLY----------------
            userID = Guid.Parse("3878d481-54e2-40a1-95e7-af664a1fd140");

            string cacheDir = Path.Combine(Globals.AppData, "alexmfv_stickies");
            string userDir = Path.Combine(cacheDir + "\\" + userID.ToString());

            if (!Directory.Exists(cacheDir))
                Directory.CreateDirectory(cacheDir);

            if (!Directory.Exists(userDir))
                Directory.CreateDirectory(userDir);

            foreach (StickyNote note in Globals.stickies)
            {
                //From Stickies Collection To JSON To File
                using (StreamWriter file = File.CreateText(userDir + "\\" + note.noteID.ToString()))
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    JObject toAdd = new JObject();
                    toAdd.Add("noteID", note.noteID);
                    toAdd.Add("noteText", note.noteText);
                    toAdd.Add("noteTitle", note.noteTitle);
                    toAdd.Add("noteColor", ColorRGBToString(note.noteColor));
                    toAdd.Add("titleColor", ColorRGBToString(note.titleColor));
                    toAdd.Add("dateCreated", note.dateCreated);
                    toAdd.Add("baseFont", note.baseFont);
                    toAdd.Add("baseFontSize", note.baseFontSize);
                    toAdd.Add("baseFontColor", note.baseFontColor);
                    toAdd.WriteTo(writer);
                }
            }
        }

        public static void SaveStickyNote(Guid userID, Guid noteID)
        {
            //-------------DEBUG ONLY----------------
            userID = Guid.Parse("3878d481-54e2-40a1-95e7-af664a1fd140");

            string cacheDir = Path.Combine(Globals.AppData, "alexmfv_stickies");
            string userDir = Path.Combine(cacheDir + "\\" + userID.ToString());

            if (!Directory.Exists(cacheDir))
                Directory.CreateDirectory(cacheDir);

            if (!Directory.Exists(userDir))
                Directory.CreateDirectory(userDir);

            StickyNote note = Globals.stickies.GetNoteFromGUID(noteID);

            using (StreamWriter file = File.CreateText(userDir + "\\" + note.noteID.ToString()))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                JObject toAdd = new JObject();
                toAdd.Add("noteID", note.noteID);
                toAdd.Add("noteText", note.noteText);
                toAdd.Add("noteTitle", note.noteTitle);
                toAdd.Add("noteColor", ColorRGBToString(note.noteColor));
                toAdd.Add("titleColor", ColorRGBToString(note.titleColor));
                toAdd.Add("dateCreated", note.dateCreated);
                toAdd.Add("baseFont", note.baseFont);
                toAdd.Add("baseFontSize", note.baseFontSize);
                toAdd.Add("baseFontColor", note.baseFontColor);
                toAdd.WriteTo(writer);
            }
        }

        public static void DeleteAllNotes(Guid userID)
        {
            userID = Guid.Parse("3878d481-54e2-40a1-95e7-af664a1fd140");

            string cacheDir = Path.Combine(Globals.AppData, "alexmfv_stickies");
            string userDir = Path.Combine(cacheDir + "\\" + userID.ToString());

            foreach(string filePath in Directory.GetFiles(userDir).ToList())
                File.Delete(filePath);
        }

        public static void DeleteNote(Guid userID, Guid noteID)
        {
            userID = Guid.Parse("3878d481-54e2-40a1-95e7-af664a1fd140");

            string cacheDir = Path.Combine(Globals.AppData, "alexmfv_stickies");
            string userDir = Path.Combine(cacheDir + "\\" + userID.ToString());
            string filepath = Path.Combine(userDir + "\\" + noteID);

            if (File.Exists(filepath))
                File.Delete(filepath);
            else
                MessageBox.Show("There was a problem deleting the note!");
        }

        //public static void DeleteSelectedNotes(Guid userID, Guid noteIDs)
        //{

        //}

        //public static void LoadUserSettings(Guid userID)
        //{
        //    //-------------------------------DEBUG ONLY-----------------------------------
        //    userID = Guid.Parse("3878d481-54e2-40a1-95e7-af664a1fd140");

        //    string cacheDir = Path.Combine(Globals.AppData, "alexmfv_stickies"); //Change To Sticky Notes
        //    string userDir = Path.Combine(cacheDir + "\\" + userID.ToString());

        //    StickyNotes collection = new StickyNotes();

        //    //If the directory of the user exists
        //    if (Directory.Exists(userDir))
        //    {
        //        //Get all the files inside the directory
        //        foreach (string file in Directory.GetFiles(userDir))
        //        {
        //            JObject obj = JObject.Parse(File.ReadAllText(file));
        //            StickyNote note = new StickyNote();
        //            note.noteID = Guid.Parse(obj.Property("noteID").Value.ToString());
        //            note.noteText = obj.Property("noteText").Value.ToString();
        //            note.noteTitle = obj.Property("noteTitle").Value.ToString();
        //            note.noteColor = Parser.ToColor(obj.Property("noteColor").Value.ToString());
        //            note.titleColor = Parser.ToColor(obj.Property("titleColor").Value.ToString());
        //            note.dateCreated = DateTime.Parse(obj.Property("dateCreated").Value.ToString());
        //            note.baseFont = obj.Property("baseFont").Value.ToString();
        //            note.baseFontSize = obj.Property("baseFontSize").Value.ToString();
        //            note.baseFontColor = obj.Property("baseFontColor").Value.ToString();
        //            collection.Add(note);
        //        }
        //    }

        //    Globals.stickies = collection;
        //}

        //public static void SaveUserSettings(Guid userID)
        //{
        //    //-------------DEBUG ONLY----------------
        //    userID = Guid.Parse("3878d481-54e2-40a1-95e7-af664a1fd140");

        //    string cacheDir = Path.Combine(Globals.AppData, "alexmfv_stickies");
        //    string userDir = Path.Combine(cacheDir + "\\" + userID.ToString());

        //    if (!Directory.Exists(cacheDir))
        //        Directory.CreateDirectory(cacheDir);

        //    if (!Directory.Exists(userDir))
        //        Directory.CreateDirectory(userDir);

        //    foreach (StickyNote note in Globals.stickies)
        //    {
        //        //From Stickies Collection To JSON To File
        //        using (StreamWriter file = File.CreateText(userDir + "\\" + note.noteID.ToString()))
        //        using (JsonTextWriter writer = new JsonTextWriter(file))
        //        {
        //            JObject toAdd = new JObject();
        //            toAdd.Add("noteID", note.noteID);
        //            toAdd.Add("noteText", note.noteText);
        //            toAdd.Add("noteTitle", note.noteTitle);
        //            toAdd.Add("noteColor", ColorRGBToString(note.noteColor));
        //            toAdd.Add("titleColor", ColorRGBToString(note.titleColor));
        //            toAdd.Add("dateCreated", note.dateCreated);
        //            toAdd.Add("baseFont", note.baseFont);
        //            toAdd.Add("baseFontSize", note.baseFontSize);
        //            toAdd.Add("baseFontColor", note.baseFontColor);
        //            toAdd.WriteTo(writer);
        //        }
        //    }
        //}

        public static string ColorRGBToString(SolidColorBrush brush)
        {
            return brush.Color.R.ToString() + ":" + brush.Color.G.ToString() + ":" + brush.Color.B.ToString();
        }
    }
}
