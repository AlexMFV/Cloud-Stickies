using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WindowsCloudStickies
{
    [Serializable()]
    public class NoInternetConnection : Exception
    {
        public NoInternetConnection() : base(String.Format("You don't have an active internet connection." +
            " Please connect to the internet and try again!"))
        { }
    }
}