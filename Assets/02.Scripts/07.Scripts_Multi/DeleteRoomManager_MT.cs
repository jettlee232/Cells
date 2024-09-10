using Photon.Pun;
using Photon.Realtime;

public class DeleteRoomManager_MT : MonoBehaviourPunCallbacks
{
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        // �濡 ���� �÷��̾� �� Ȯ��
        if (PhotonNetwork.CurrentRoom.PlayerCount == 0)
        {
            // ���� �ٷ� �����ϱ� ���� ���� ���¸� ����
            PhotonNetwork.CurrentRoom.IsOpen = false; // ���� �� �� ���� ����
            PhotonNetwork.CurrentRoom.IsVisible = false; // ���� ��Ͽ��� ����

            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
    }
}
