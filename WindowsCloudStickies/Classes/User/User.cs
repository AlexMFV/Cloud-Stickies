using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsCloudStickies
{
    public class User
    {
        private Guid id { get; set; }
        private string username { get; set; }
        private AuthType authType { get; set; }
        private bool successfulAuth { get; set; }

        public User() { }

        /// <summary>
        /// Create a new instance of the logged in/registered user
        /// </summary>
        /// <param name="usr_id">Guid of the user</param>
        /// <param name="usr">Username of the user</param>
        /// <param name="type">Type of authentication used (Login, Guest, Register)</param>
        /// <param name="status">If the connection was successful or not</param>
        public User(string usr_id, string usr, AuthType type, bool status)
        {
            this.id = Guid.Parse(usr_id);
            this.username = usr;
            this.authType = type;
            this.successfulAuth = status;
        }

        /// <summary>
        /// Create a new instance for a Guest user
        /// </summary>
        /// <param name="success">If the connection was successful or not</param>
        public User(bool success)
        {
            this.id = Guid.Empty;
            this.username = "Guest";
            this.authType = AuthType.Guest;
            this.successfulAuth = success;
        }

        public Guid ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public AuthType AuthType
        {
            get { return this.authType; }
            set { this.authType = value; }
        }

        public bool SuccessfulAuth
        {
            get { return this.successfulAuth; }
            set { this.successfulAuth = value; }
        }
    }
}
