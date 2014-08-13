using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WiFiHost
{
    public class WifiHost
    {

        private string ssid { set; get; }
        private string password { set; get; }
        SharingConnection sharingConnection { set; get; }
        public int maxClients { set; get; }
        public bool isHost { set; get; }
        ConnectSharingManager csManager ;
        WlanManager wlanManager ;

        public WifiHost()
        {   
            csManager=new ConnectSharingManager();
            wlanManager=new WlanManager();
            isHost = false;
            
        }
        /// <summary>
        /// 获取SharingConnection集合
        /// </summary>
        /// <returns></returns>
        public List<SharingConnection> GetSharingConnections()
        {
            return this.csManager.Connections;
        }

        /// <summary>
        /// WifiHost参数初始化
        /// </summary>
        /// <param name="ssid"></param>
        /// <param name="password"></param>
        /// <param name="maxClients">最大客户端数</param>
        /// <param name="sharingConnection">共享连接</param>
        public void Init(string ssid, string password, int maxClients, SharingConnection sharingConnection)
        {
            this.ssid = ssid;
            this.password = password;
            this.sharingConnection = sharingConnection;
            this.maxClients = maxClients;
            string error = ValidateFields();
            if (error != string.Empty)
            {
                throw new Exception(error);
            }
        }

        public bool Start()
        {   
            
            try
            {   
                Stop();
                wlanManager.SetConnectionSettings(ssid, 32);
                wlanManager.SetSecondaryKey(password);
                wlanManager.StartHostedNetwork();

                var privateConnectionGuid = wlanManager.HostedNetworkInterfaceGuid;

                csManager.EnableIcs(sharingConnection.Guid, privateConnectionGuid);
                isHost = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Stop()
        {
            try
            {
                if (this.csManager.SharingInstalled)
                {
                    this.csManager.DisableIcsOnAll();
                }

                this.wlanManager.StopHostedNetwork();
                isHost = false;
                return true;
            }
            catch
            {
                return false;
            }
        }


        private string ValidateFields()
        {
            if (ssid.Length <= 0)
            {
                return ErrorResource.SSIDRequiredError;
            }

            if (ssid.Length > 32)
            {
                return ErrorResource.SSIDTooLongError;
            }

            if (password.Length < 8)
            {
                return ErrorResource.PasswordTooShortError;
            }

            if (password.Length > 64)
            {
                return ErrorResource.PasswordTooLongError;
            }
            return string.Empty;
        }
    }
}
