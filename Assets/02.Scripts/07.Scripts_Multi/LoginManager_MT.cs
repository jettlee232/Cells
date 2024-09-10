using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LoginManager_MT : MonoBehaviourPunCallbacks
{
    //public TMP_InputField PlayerName_InputField;

    public float checkInterval = 0.1f; // 체크 간격

    IEnumerator CheckConnect()
    {
        float timeout = 10f; // 타임아웃 시간 (초)
        float elapsed = 0f;

        while (!PhotonNetwork.IsConnectedAndReady)
        {
            yield return new WaitForSeconds(checkInterval);
            elapsed += checkInterval;

            if (elapsed >= timeout) { yield break; }
        }

        // 네트워크 연결 완료 후 씬 로드
        PhotonNetwork.LoadLevel("07_Multi");
    }
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
        StartCoroutine(CheckConnect());
    }
    #endregion

    #region Photon Callback Methods
    //public override void OnConnectedToMaster() { PhotonNetwork.LoadLevel("07_Multi"); }
    #endregion
}
