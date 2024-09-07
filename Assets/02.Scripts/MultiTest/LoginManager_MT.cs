using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LoginManager_MT : MonoBehaviourPunCallbacks
{
    //public TMP_InputField PlayerName_InputField;

    #region UNITY Methods
    #endregion

    #region UI Callback Methods
    public void ConnectToPhotonServer()
    {
        //if (PlayerName_InputField != null)
        //{
        //    PhotonNetwork.NickName = PlayerName_InputField.text;
        //    PhotonNetwork.ConnectUsingSettings();
        //}
        PhotonNetwork.ConnectUsingSettings();
    }
    #endregion

    #region Photon Callback Methods
    public override void OnConnected() { Debug.Log("OnConnected is called. The server is available."); }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the Master Server with player name: " + PhotonNetwork.NickName);
        PhotonNetwork.LoadLevel("MultiTest2");
    }
    #endregion
}
