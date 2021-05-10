using NETWORKLIST;

namespace WindowsCloudStickies
{
    public static class Network
    {
        private static INetworkListManager _networkListManager;

        public static bool HasInternetAccess()
        {
            _networkListManager = new NetworkListManager();
            return _networkListManager.IsConnectedToInternet;
        }
    }
}