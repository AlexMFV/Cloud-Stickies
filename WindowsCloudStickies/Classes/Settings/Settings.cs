using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsCloudStickies
{
    public class Settings
    {
        Guid userID;
        string userName;
        DateTime dateCreated;

        public Settings() { }

        public Settings(Guid uID, string username, DateTime date)
        {
            this.userID = uID;
            this.userName = username;
            this.dateCreated = date;
        }

        public void CreateNewUser(string username = "guest")
        {
            this.userID = Guid.NewGuid();
            this.userName = VerifyUsername(username);
            this.dateCreated = DateTime.Now;
        }

        public string VerifyUsername(string userName)
        {
            //Check if the user with the current username exists
            //If the username already exists, then add a Count+1 to the new username

            return "";
        }
    }
}
