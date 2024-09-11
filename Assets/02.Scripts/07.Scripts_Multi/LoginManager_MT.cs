using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LoginManager_MT : MonoBehaviourPunCallbacks
{
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
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LoadLevel("07_Multi");
    }
    #endregion
}