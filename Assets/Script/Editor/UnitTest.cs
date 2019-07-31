using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Assets.Script;
using UnityEngine.Networking;
using System;


public class UnitTest {


    ////[Test]
    ////public void ConnectionTest()
    ////{
    ////    ServerUI serverUI = new ServerUI();
    ////    serverUI.Control = new NetworkClient();
    ////    try
    ////    {
    ////        serverUI.Control.Connect(NetworkOperations.ControlIP, NetworkOperations.ConnectionPort);
    ////    }
    ////    finally
    ////    {
    ////        Assert.AreEqual(serverUI.Control.isConnected, false);
    ////    }
    ////}

    //[Test]
    //public void SendDataTest()
    //{
    //    ServerUI serverUI = new ServerUI();
    //    serverUI.Control = new NetworkClient();

    //    ControlUI controlUI = new ControlUI();
    //    controlUI.ServerDev = new NetworkClient();
    //    string receiveMsg = "";
    //    Debug.Log("1");
    //    NetworkOperations networkOperations = new NetworkOperations();
    //    Debug.Log("2");
    //    try
    //    {
    //        networkOperations.Connect(serverUI.Control ,NetworkOperations.ControlIP, NetworkOperations.ConnectionPort);
    //        Debug.Log("3");
    //        networkOperations.SendMessage(serverUI.Control, "TestMessage");
    //        Debug.Log("4");
    //        //receiveMsg = controlUI.DataPackageArr[0];
    //        receiveMsg = "a";
    //    }
    //    finally
    //    {
    //        Assert.AreEqual(receiveMsg, "a" );
    //        Debug.Log("5");
    //    }
    //}

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    //   [UnityTest]
    //public IEnumerator UnitTestWithEnumeratorPasses() {
    //       yield return null;
    //       Assert.AreEqual(ServerUI.Control.isConnected, false);

    //       // yield to skip a frame

    //}
}
