

using System;
using System.Collections.Generic;
using System.Linq;
using NETCONLib;


namespace WiFiHost
{
    
    public class ConnectSharingManager
    {
        protected INetSharingManager _NSManager;

        public ConnectSharingManager()
        {
            this.Init();
        }

        public void Init()
        {
            this._NSManager = new NetSharingManagerClass();
        }

        public void EnableIcs(Guid publicGuid, Guid privateGuid)
        {
            if (!this.SharingInstalled)
            {
                throw new Exception("Internet Connection Sharing NOT Installed");
            }

            var connections = this.Connections;

            SharingConnection publicConn = (from c in connections
                              where c.IsMatch(publicGuid)
                              select c).First();

            SharingConnection privateConn = (from c in connections
                                         where c.IsMatch(privateGuid)
                                        select c).First();

            this.DisableIcsOnAll();

            publicConn.EnableAsPublic();
            privateConn.EnableAsPrivate();
        }

        public void DisableIcsOnAll()
        {
            foreach (var conn in this.Connections)
            {
                if (conn.IsSupported)
                {
                    conn.DisableSharing();
                }
            }
        }

        private List<SharingConnection> _Connections = null;
        public List<SharingConnection> Connections
        {
            get
            {
                //if (this._Connections == null)
                //{
                //    this._Connections = new List<IcsConnection>();

                //    foreach (INetConnection conn in this._NSManager.EnumEveryConnection)
                //    {
                //        this._Connections.Add(new IcsConnection(this._NSManager, conn));
                //    }
                //}
                //return this._Connections;

                this._Connections = new List<SharingConnection>();

                foreach (INetConnection conn in this._NSManager.EnumEveryConnection)
                {
                    this._Connections.Add(new SharingConnection(this._NSManager, conn));
                }

                return this._Connections;
            }
        }

        public bool SharingInstalled
        {
            get
            {
                return this._NSManager.SharingInstalled;
            }
        }
    }
}
