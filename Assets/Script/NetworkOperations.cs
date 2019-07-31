using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

namespace Assets.Script
{
    public class NetworkOperations
    {
        public static string ControlIP = "172.20.10.2",
            ServerIP = "172.20.10.4",
            DisplayIP = "172.20.10.3",
            ProjectFolderName = "EtkilesimliAnimasyon";
    
        public static int ConnectionPort = 25000, ListenPort = 25000;

        public void Connect(NetworkClient ConnectDev, String ConnDevIP, int ConnPort)
        {
            ConnectDev.Connect(ConnDevIP, ConnPort);
        }

        public void Disconnect(NetworkClient ConnectDev)
        {
            ConnectDev.Disconnect();
        }

        public void SendMessage(NetworkClient SenderDevice, string Message)
        {
            if (SenderDevice.isConnected)
            {
                StringMessage SendMsg = new StringMessage();
                Debug.Log("SendMsgToScreen" + Message);
                SendMsg.value = Message;
                SenderDevice.Send(888, SendMsg);
            }
        }
    }
}
