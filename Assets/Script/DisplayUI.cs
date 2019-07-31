using System;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script;

public class DisplayUI : MonoBehaviour {
    
    string[] AnimationMsgArr;
    NetworkClient ServerDev;
    ConnectionConfig connectionConfig = new UnityEngine.Networking.ConnectionConfig();
    //public InputField txtReceiveMsg;
    int maxConnection = 10;
    StringMessage ReceiveMsg;
    //bool NetworkState = false;
    //VideoController videoController;
    public Canvas VideoCnv, MenuCnv;
    NetworkOperations networkOperations = new NetworkOperations();
    string ServerIP = NetworkOperations.ServerIP;
    void Start()
    {
        ListenServer();
        ServerDev = new NetworkClient();
        networkOperations.Connect(ServerDev, NetworkOperations.ServerIP, NetworkOperations.ConnectionPort);
        ServerDev.RegisterHandler(MsgType.Disconnect, OnServerDisconnect);
        ServerDev.RegisterHandler(MsgType.Error, OnServerError);
        //SceneManager.LoadScene(SceneManager.GetSceneByName());
        //txtReceiveMsg.text = "Listen25000";
        //VideoCnv.enabled = false;
        //MenuCnv.enabled = true;
    }

    protected virtual void OnServerDisconnect(NetworkMessage netMsg)
    {
        networkOperations.Connect(ServerDev, NetworkOperations.ServerIP, NetworkOperations.ConnectionPort);
    }

    protected virtual void OnServerError(NetworkMessage netMsg)
    {
        networkOperations.Connect(ServerDev, NetworkOperations.ServerIP, NetworkOperations.ConnectionPort);
    }
    // Update is called once per frame
    void Update(){
    }

    void OnGUI()
    {
        //if (!ServerDev.isConnected)
        //{
        //    if (GUI.Button(new Rect(50, 50, 250, 100), "Connect"))
        //    {
        //        Connect(ServerIP, ListenPort);
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    VideoCnv.enabled = false;
        //    MenuCnv.enabled = true;
        //}
    }
    void ListenServer()
    {
        NetworkServer.Listen(NetworkOperations.ListenPort);
        NetworkServer.RegisterHandler(888, ListenServerMessage);
        NetworkServer.Configure(connectionConfig, maxConnection);
    }
    private void ListenServerMessage(NetworkMessage message)
    {
        ReceiveMsg = new StringMessage();
        ReceiveMsg.value = message.ReadMessage<StringMessage>().value;
        try
        {
            AnimationMsgArr = ReceiveMsg.value.Split('|');
        }
        finally
        {
            if (AnimationMsgArr[0] == "AppStart")
            {
                networkOperations.SendMessage(ServerDev, "appisstarting");
                VideoCnv.enabled = true;
                MenuCnv.enabled = false;
            }
            else if (AnimationMsgArr[0] == "AppQuit")
            {
                Application.Quit();
            }
            else
            {
                VideoPlayer.VideoURL = AnimationMsgArr[0];
            }
            //txtReceiveMsg.text = AnimationMsgArr[0];
        }
    }
    
    //void Connect(NetworkClient networkClient, String ConnDevIP, int ConnPort)
    //{
    //    networkClient.Connect(ConnDevIP, ConnPort);
    //}

    //public void SendMessageToServer()
    //{
    //    if (ServerDev.isConnected)
    //    {
    //        StringMessage SendMsg = new StringMessage();
    //        Debug.Log("SendMsgToScreen");
    //        SendMsg.value = "appisstarting";
    //        ServerDev.Send(888, SendMsg);
    //    }
    //    VideoCnv.enabled = true;
    //    MenuCnv.enabled = false;
    //}

    public void ExitButton()
    {
        Application.Quit();
    }
}
