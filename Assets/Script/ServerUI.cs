using System;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using Assets.Script;
using Newtonsoft.Json.Linq;
public class ServerUI : MonoBehaviour {

    int maxConnection = 10, /* hostCount=0,hostCountTrig=0,*/
        totalAnimation = 6,
        FactorAnimation; // Hayvanlar = 0 , Renkler = 6, Meyveler = 12..
    User user; Question question;
    ConnectionConfig connectionConfig;
    public NetworkClient Display, Control;
    public InputField txtReceiveMsg;
    NetworkOperations networkOperations;
    string QuestionMessage, AnimationMessage;
    string[] ReceiveMsgArr = new string[10];

    // Use this for initialization
    void Start() {
        
        networkOperations = new NetworkOperations();
        Display = new NetworkClient();
        Control = new NetworkClient();
        networkOperations.Connect(Display, NetworkOperations.DisplayIP, NetworkOperations.ConnectionPort); // Connect(Screen, ScreenIP, ConnectionPort);
        networkOperations.Connect(Control, NetworkOperations.ControlIP, NetworkOperations.ConnectionPort); // Connect(Control, ControlIP, ConnectionPort);
        Display.RegisterHandler(MsgType.Disconnect, OnScreenDisconnect);
        Display.RegisterHandler(MsgType.Error, OnScreenError);
        Control.RegisterHandler(MsgType.Disconnect, OnControlDisconnect);
        Control.RegisterHandler(MsgType.Error, OnControlError);
        connectionConfig = new ConnectionConfig();
        ListenServer(); // Dinlemeye başla
        user = new User();
        question = new Question();
        txtReceiveMsg.enabled = true;
        //Network.InitializeServer(10, 25000, false);
    }
    protected virtual void OnScreenDisconnect(NetworkMessage netMsg)
    {
        networkOperations.Connect(Display, NetworkOperations.DisplayIP, NetworkOperations.ConnectionPort);
        Debug.Log(netMsg.ToString());
    }

    protected virtual void OnScreenError(NetworkMessage netMsg)
    {
        networkOperations.Connect(Display, NetworkOperations.DisplayIP, NetworkOperations.ConnectionPort);
    }

    protected virtual void OnControlDisconnect(NetworkMessage netMsg)
    {
        networkOperations.Connect(Control, NetworkOperations.ControlIP, NetworkOperations.ConnectionPort);
    }

    protected virtual void OnControlError(NetworkMessage netMsg)
    {
        networkOperations.Connect(Control, NetworkOperations.ControlIP, NetworkOperations.ConnectionPort);
    }
    void Update() {
    }
    void OnGUI()
    {
        if (!Display.isConnected)
        {
            if (GUI.Button(new Rect(50, 50, 250, 100), "Connect"))
            {
                networkOperations.Connect(Display, NetworkOperations.DisplayIP, NetworkOperations.ConnectionPort);
            }
        }

        if (!Control.isConnected)
        {
            if (GUI.Button(new Rect(50, 175, 250, 100), "Connect"))
            {
                networkOperations.Connect(Control, NetworkOperations.ControlIP, NetworkOperations.ConnectionPort);
            }
        }
    }

    //void Connect(NetworkClient ConnectDev, String ConnDevIP, int ConnPort)
    //{
    //    //hostCount++;
    //    //if (hostCount < 4)
    //    //{
    //    ConnectDev.Connect(ConnDevIP, ConnPort);
    //    //}
    //    //else
    //    //{
    //    //    ConnectDev.ResetConnectionStats();
    //    //    //NetworkServer.Reset();
    //    //    if (hostCount == 6)
    //    //    {
    //    //        hostCount = 0;
    //    //    }
    //    //    Debug.Log("NetworkReset");
    //    //}
    //}

    //public void SendMessage(NetworkClient SendDevice, string Message)
    //{
    //    if (SendDevice.isConnected)
    //    {
    //        StringMessage SendMsg = new StringMessage();
    //        Debug.Log("SendMessage(" + Message + ")..");
    //        SendMsg.value = Message;
    //        SendDevice.Send(888, SendMsg);
    //    }
    //}
    void ListenServer()
    {
        NetworkServer.Listen(NetworkOperations.ListenPort);
        NetworkServer.RegisterHandler(888, ListenServerMessage);
        NetworkServer.Configure(connectionConfig, maxConnection);
    }
    private void ListenServerMessage(NetworkMessage message)
    {
        StringMessage ReceiveMsg = new StringMessage();
        ReceiveMsg.value = message.ReadMessage<StringMessage>().value;
        try
        {
            ReceiveMsgArr = ReceiveMsg.value.Split('|');
        }
        finally
        {
            if (ReceiveMsgArr[0] == "appisstarting")
            {
                networkOperations.SendMessage(Display, "http://" + NetworkOperations.ServerIP + "/"+ NetworkOperations.ProjectFolderName  + "/introAnimation.mp4");
                networkOperations.SendMessage(Control, "subjectSelect");
            }
            else if (ReceiveMsgArr[1] == "False" && ReceiveMsgArr[2] == "-1")
            {
                user.animation.Subject = ReceiveMsgArr[0]; 
                UpdateFactorAnimation();
                user.Answer = Convert.ToBoolean(ReceiveMsgArr[1]);
                user.animation.ID = Convert.ToInt32(ReceiveMsgArr[2]);
                
                GetDataFromJSON(FactorAnimation + 1, "SELF");
                networkOperations.SendMessage(Display, "http://" + NetworkOperations.ServerIP + "/"+ NetworkOperations.ProjectFolderName + "/Video/" + ReceiveMsgArr[0] + "Sokagi/" + ReceiveMsgArr[0] + "Sokagi_1.mp4");
                networkOperations.SendMessage(Control, QuestionMessage);
            }
            else if (ReceiveMsgArr[1] == "True")
            {
                GetDataFromJSON(Convert.ToInt32(ReceiveMsgArr[2]), "TAID"); // TrueAnswer ID
                networkOperations.SendMessage(Display, AnimationMessage);
                networkOperations.SendMessage(Control, QuestionMessage);
            }
            else if (ReceiveMsgArr[1] == "False")
            {
                GetDataFromJSON(Convert.ToInt32(ReceiveMsgArr[2]), "WAID"); // WrongAnswer ID
                networkOperations.SendMessage(Display, AnimationMessage);
                networkOperations.SendMessage(Control, QuestionMessage);
            }
            Debug.Log(ReceiveMsgArr[0]);
        }
    }

    void UpdateFactorAnimation()
    {
        switch (user.animation.Subject)
        {
            case "Hayvanlar":
                FactorAnimation = 0;
                break;

            case "Renkler":
                FactorAnimation = 6;
                break;

            case "Meyveler":
                FactorAnimation = 12;
                break;

            default:
                FactorAnimation = -1;
                break;
        }
    }

    public void GetDataFromJSON(int ID, string AnswerState)
    {
        string url;
        if ((ID - 1) % totalAnimation == 0)
        {
            url = "http://" + NetworkOperations.ServerIP + "/"+ NetworkOperations.ProjectFolderName + "/Animasyon.php?" + AnswerState + "=" + ID;
        }
        else
        {
            url = "http://" + NetworkOperations.ServerIP + "/" + NetworkOperations.ProjectFolderName + "/Animasyon.php?" + AnswerState + "=" + ID;
        }

        WebClient webClient = new WebClient();
        webClient.Encoding = System.Text.Encoding.UTF8;
        string json = webClient.DownloadString(url);
        Debug.Log(json);
        Debug.Log(url);
        JContainer jObject = JArray.Parse(json);
        question.QuestionText = (string)jObject[0]["SoruMetni"];
        question.TrueAnswer = (string)jObject[0]["DogruCevap"];
        question.TrueAnswerImageURL = (string)jObject[0]["DogruCevapGorselAdres"];
        question.WrongAnswer = (string)jObject[0]["YanlisCevap"];
        question.WrongAnswerImageURL = (string)jObject[0]["YanlisCevapGorselAdres"];
        user.animation.ID = (int)jObject[0]["AnimasyonID"];
        user.animation.Subject = (string)jObject[0]["Konu"];
        user.animation.Url = (string)jObject[0]["Url"];
        QuestionMessage = question.QuestionText + "|" + question.TrueAnswer + "|" + question.TrueAnswerImageURL + "|"
            + question.WrongAnswer + "|" + question.WrongAnswerImageURL + "|" + user.animation.ID + "|" + user.animation.Subject;
        AnimationMessage = user.animation.Url;
    }
}