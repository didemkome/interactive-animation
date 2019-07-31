using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script;
using UnityEngine.SceneManagement;

/* 
# Doğru Yanlis Cevap Sahneleri
*/

public class ControlUI : MonoBehaviour {
    bool TimerTick = false;
    // float Timer = -1.0f; // 2 saniye mobil ekranda doğru yada yanlış bildirimi görünür.
    public int maxConnection = 10,id=-1,
        totalAnimation=6,FactorAnimation=0;
    string SubjectAnimation;
    public string[] DataPackageArr = new string[10];
    public Question question; public User user;
    public Text QuestionText;    //TimeLeft;
    public Canvas QuestionAnswerCnv, subjectSelect,Answer, Menu;
    public Button choiceOne, choiceTwo,
        btnAnimal, btnFruit,btnColor; //topicButtons
    public Sprite TrueImg, WrongImg;
    public NetworkClient ServerDev, ScreenDev;
    StringMessage mobilMsg, ReceiveMsg;
    ConnectionConfig connectionConfig = new UnityEngine.Networking.ConnectionConfig();
    NetworkOperations networkOperations;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        //Screen.fullScreen = true;
        networkOperations = new NetworkOperations();

        ServerDev = new NetworkClient();
        ServerDev.RegisterHandler(MsgType.Disconnect, OnServerDisconnect);
        networkOperations.Connect(ServerDev, NetworkOperations.ServerIP, NetworkOperations.ConnectionPort);

        ScreenDev = new NetworkClient();
        ScreenDev.RegisterHandler(MsgType.Disconnect, OnScreenDisconnect);
        networkOperations.Connect(ScreenDev, NetworkOperations.DisplayIP, NetworkOperations.ConnectionPort);

        //Network.InitializeServer(10, 25000, true);
        ListenServer();
        question = new Question();
        user = new User();
        user.animation.ID = -1; user.animation.Subject = "tanimsiz"; user.Answer = false;
        subjectSelect.enabled = false;
        QuestionAnswerCnv.enabled = false;
        Menu.enabled = true;
    }

    protected virtual void OnServerDisconnect(NetworkMessage netMsg)
    {
        networkOperations.Connect(ServerDev, NetworkOperations.ServerIP, NetworkOperations.ConnectionPort);
    }
    protected virtual void OnServerError(NetworkMessage netMsg)
    {
        networkOperations.Connect(ServerDev, NetworkOperations.ServerIP, NetworkOperations.ConnectionPort);
    }
    protected virtual void OnScreenDisconnect(NetworkMessage netMsg)
    {
        networkOperations.Connect(ScreenDev, NetworkOperations.DisplayIP, NetworkOperations.ConnectionPort);
    }
    void Update(){
        //Timer -= Time.deltaTime;
        //if (Convert.ToInt32(Timer) > 0)
        //{
        //    TimeLeft.text = "Kalan Süre : " + Convert.ToInt32(Timer).ToString();
        //    Debug.Log(Convert.ToInt32(Timer).ToString());
        //}
        //else if (Convert.ToInt32(Timer)==0)
        //{
        //    user.Answer = false;
        //    networkOperations.SendMessage(ServerDev, user.animation.Subject + "|" + user.Answer + "|" + user.animation.ID); // BilgiPaketi(); 
        //}
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu.enabled = true;
        }
    }
    void OnGUI()
    {
        if (!ServerDev.isConnected)
        {
            if (GUI.Button(new Rect(50, 50, 200, 70), "Connect"))
            {
                networkOperations.Connect(ServerDev, NetworkOperations.ServerIP, NetworkOperations.ConnectionPort);
            }
        }

        if (!ScreenDev.isConnected)
        {
            if (GUI.Button(new Rect(50, 150, 200, 70), "Connect"))
            {
                networkOperations.Connect(ScreenDev, NetworkOperations.DisplayIP, NetworkOperations.ConnectionPort);
            }
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
    public void ListenServer()
    {
        NetworkServer.Listen(NetworkOperations.ListenPort);
        NetworkServer.RegisterHandler(888, ListenServerMessage);
        NetworkServer.Configure(connectionConfig, maxConnection);
    }
    public void ListenServerMessage(NetworkMessage message)
    {
        StringMessage DataPackage = new StringMessage();
        DataPackage.value = message.ReadMessage<StringMessage>().value;
        try
        {
            DataPackageArr = DataPackage.value.Split('|');
        }
        finally
        {
            if (DataPackageArr[0] == "subjectSelect")
            {
                QuestionAnswerCnv.enabled = false;
                subjectSelect.enabled = true;
            }
            else
            {
                question.QuestionText = DataPackageArr[0];
                question.TrueAnswer = DataPackageArr[1];
                question.TrueAnswerImageURL = DataPackageArr[2];
                question.WrongAnswer = DataPackageArr[3];
                question.WrongAnswerImageURL = DataPackageArr[4];
                user.animation.ID = Convert.ToInt32(DataPackageArr[5]);
                user.animation.Subject = DataPackageArr[6];
                //Timer = 30.0f;
                UpdateFactorAnimation();
                FillInterface();
            }
        }
    }
    void FillInterface()
    {
        QuestionText.text = question.QuestionText;
        SetButtonText(choiceOne, choiceTwo, question.TrueAnswer, question.WrongAnswer, question.TrueAnswerImageURL, question.WrongAnswerImageURL);
    }
    void SetButtonText(Button choiceOne, Button choiceTwo, string TrueAnswer, string WrongAnswer,
        string TrueAnswerImageURL, string WrongAnswerImageURL)
    {
        int a;
        System.Random rnd = new System.Random();
        a = rnd.Next(0, 2);
        if (a == 0)
        {
            choiceOne.GetComponentInChildren<Text>().text = TrueAnswer;
            choiceTwo.GetComponentInChildren<Text>().text = WrongAnswer;
            StartCoroutine(setImage(TrueAnswerImageURL, choiceOne)); 
            StartCoroutine(setImage(WrongAnswerImageURL, choiceTwo)); 
        }
        else
        {
            choiceOne.GetComponentInChildren<Text>().text = WrongAnswer;
            choiceTwo.GetComponentInChildren<Text>().text = TrueAnswer;
            StartCoroutine(setImage(WrongAnswerImageURL, choiceOne));
            StartCoroutine(setImage(TrueAnswerImageURL, choiceTwo));
        }
    }
    bool AnswerCheck(Button SelectedButton)
    {
        if (SelectedButton.GetComponentInChildren<Text>().text == question.TrueAnswer)
        {
            user.Answer = true;
        }
        else
        {
            user.Answer = false;
        }
        return user.Answer;
    }

    //public void BilgiPaketi()
    //{
    //    StringMessage MbltoSrvrMsg = new StringMessage();
    //    MbltoSrvrMsg.value = user.animation.Subject + "|" + user.Answer + "|" + user.animation.ID;
    //    ServerDev.Send(888, MbltoSrvrMsg);
    //}
    
    public void ButtonchoiceOne()
    {
        if(AnswerCheck(choiceOne) && user.animation.ID>((totalAnimation + FactorAnimation) - 2))
        {
            choiceOne.image.sprite = TrueImg;
            networkOperations.SendMessage(ServerDev, "appisstarting");
            user.animation.ID = -1; user.animation.Subject = "tanimsiz"; user.Answer = false;  // Eğer son 2 soruda doğru yaparsa başa intro'ya döner
        }
        else if (AnswerCheck(choiceOne))
        {
            choiceOne.image.sprite = TrueImg;
            networkOperations.SendMessage(ServerDev, user.animation.Subject + "|" + user.Answer + "|" + user.animation.ID); // Son 2 soru haricinde doğru yapılan blok
        }
        else
        {
            choiceOne.image.sprite = WrongImg;
            networkOperations.SendMessage(ServerDev, user.animation.Subject + "|" + user.Answer + "|" + user.animation.ID); //BilgiPaketi(); // Sourya Yanlış cevap verilen blok
        }
        choiceOne.image.sprite = null;
    }
    public void ButtonchoiceTwo()
    {
        if (AnswerCheck(choiceTwo) && user.animation.ID > ((totalAnimation + FactorAnimation) - 2))
        {
            choiceTwo.image.sprite = TrueImg;
            networkOperations.SendMessage(ServerDev, "appisstarting");
            user.animation.ID = -1; user.animation.Subject = "tanimsiz"; user.Answer = false;
        }
        else if (AnswerCheck(choiceTwo))
        {
            choiceTwo.image.sprite = TrueImg;
            networkOperations.SendMessage(ServerDev, user.animation.Subject + "|" + user.Answer + "|" + user.animation.ID);
        }
        else
        {
            choiceTwo.image.sprite = WrongImg;
            networkOperations.SendMessage(ServerDev, user.animation.Subject + "|" + user.Answer + "|" + user.animation.ID);
        }
        choiceOne.image.sprite = null;
    }
    IEnumerator setImage(string url, Button imageButton)
    {
        //logMessage = "State: SetImage";
        // Start a download of the given URL
        var www = new WWW(url);
        // wait until the download is done
        yield return www;
        // Create a texture in DXT1 format
        Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
        // assign the downloaded image to sprite
        www.LoadImageIntoTexture(texture);
        Rect rec = new Rect(0, 0, texture.width, texture.height);
        Sprite spriteToUse = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
        imageButton.image.sprite = spriteToUse;
        www.Dispose();
        www = null;
    }

    // Konu Secim //
    public void SendTopicFruit()
    {
        user.animation.Subject = "Meyveler";
        networkOperations.SendMessage(ServerDev, user.animation.Subject + "|" + user.Answer + "|" + user.animation.ID);
        TopicFalseQuestActive();
    }

    public void SendTopicAnimal()
    {
        user.animation.Subject = "Hayvanlar";
        networkOperations.SendMessage(ServerDev, user.animation.Subject + "|" + user.Answer + "|" + user.animation.ID);
        TopicFalseQuestActive();
    }

    public void SendTopicColor()
    {
        user.animation.Subject = "Renkler";
        networkOperations.SendMessage(ServerDev, user.animation.Subject + "|" + user.Answer + "|" + user.animation.ID);
        TopicFalseQuestActive();
    }

    void TopicFalseQuestActive()
    {
        subjectSelect.enabled = false;
        QuestionAnswerCnv.enabled = true;
        TimerTick = true;
    }

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

    public void StartButton()
    {
        if (ScreenDev.isConnected)
        {
            networkOperations.SendMessage(ScreenDev, "AppStart");
            Menu.enabled = false;
        }
    }

    public void ExitButton()
    {
        if (ScreenDev.isConnected)
        {
            networkOperations.SendMessage(ScreenDev, "AppQuit");
            Menu.enabled = false;
        }
        Application.Quit();
    }

    //void Connect(NetworkClient ConnectDev ,String ConnDevIP, int ConnPort)
    //{
    //    ConnectDev.Connect(ConnDevIP, ConnPort);
    //}
    //void Disconnect()
    //{
    //    ServerDev.Disconnect();
    //}
}
