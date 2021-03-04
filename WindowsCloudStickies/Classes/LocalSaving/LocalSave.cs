using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsCloudStickies
{
    class LocalSave
    {
        //Save all notes
        //Save individual note
        //Read from files
        //Write to files
        //Convert from FileRead to Object
        //Convert from Object to JSON

        //Get the %appdata% dir
        static string appdata_dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        //public static StickyNote LoadAccounts()
        //{
        //    string cacheDir = Path.Combine(appdata_dir, "alexmfv");
        //    Accounts collection = new Accounts();

        //    if (File.Exists(cacheDir + "\\steam_accounts.json"))
        //    {
        //        if (new FileInfo(cacheDir + "\\steam_accounts.json").Length > 0)
        //        {
        //            JArray arr = JArray.Parse(File.ReadAllText(cacheDir + "\\steam_accounts.json"));
        //            foreach (JObject obj in arr)
        //            {
        //                Account acc = Parser.ParseAccount(obj.Property("SteamID").Value.ToString());
        //                collection.Add(acc);
        //            }
        //        }
        //    }

        //    return collection;
        //}

        //public static void SaveAccounts(Accounts collection)
        //{
        //    string cacheDir = Path.Combine(appdata_dir, "alexmfv");

        //    if (!Directory.Exists(cacheDir))
        //        Directory.CreateDirectory(cacheDir);

        //    //From Accounts Collection To JSON To File
        //    using (StreamWriter file = File.CreateText(cacheDir + "\\steam_accounts.json"))
        //    using (JsonTextWriter writer = new JsonTextWriter(file))
        //    {
        //        JArray arr = new JArray();

        //        foreach (Account acc in collection)
        //        {
        //            JObject toAdd = new JObject();
        //            toAdd.Add("SteamID", acc.steamID);
        //            toAdd.Add("Name", acc.personaName);
        //            arr.Add(toAdd);
        //        }

        //        arr.WriteTo(writer);
        //    }
        //}
    }
}
