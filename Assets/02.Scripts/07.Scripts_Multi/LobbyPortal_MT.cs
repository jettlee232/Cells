using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyPortal_MT : MonoBehaviourPunCallbacks
{
    public string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<FadeInOut_MT>().FadeOut();
            Invoke("LeaveGame", 0.75f);
        }
    }

    public void LeaveGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false; // ���� �� �� ���� ����
            PhotonNetwork.CurrentRoom.IsVisible = false; // ���� ��Ͽ��� ����
            PhotonNetwork.LeaveRoom();
        }
        else { PhotonNetwork.LeaveRoom(); }
        /*
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            PhotonNetwork.Disconnect();
        }
        */
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene(sceneName);
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        SceneManager.LoadScene(sceneName);
    }
}
