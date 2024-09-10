using Photon.Pun;
using Photon.Realtime;

public class DeleteRoomManager_MT : MonoBehaviourPunCallbacks
{
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        // 방에 남은 플레이어 수 확인
        if (PhotonNetwork.CurrentRoom.PlayerCount == 0)
        {
            // 방을 바로 삭제하기 위해 방의 상태를 변경
            PhotonNetwork.CurrentRoom.IsOpen = false; // 방을 열 수 없게 설정
            PhotonNetwork.CurrentRoom.IsVisible = false; // 방을 목록에서 숨김

            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
    }
}
