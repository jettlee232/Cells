using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ButtonDoorController_Lobby : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject door;
    public GameObject button;

    [Header("Door Script")]
    public float DPFarDistance = 0.5f;    // 문-플레이어 사이 거리가 어느정도일 때 문이 닫힐 지
    public float openTimer = 1f;        // 문이 열리는 데에 걸리는 시간
    public float closeTimer = 0.5f;     // 문이 닫히는 데에 걸리는 시간
    public float DdestX = 2.3f;          // 문 완전히 열렸을 때의 X 값
    private float DfirstX;               // 문 완전히 닫혔을 때의 X 값
    private Vector3 doorPos;            // 처음 문의 위치

    [Header("Subtitle Script")]
    public float BPCloseDistance = 2f;       // 버튼-플레이어 사이 거리가 어느정도일 때 자막이 뜰 지

    private GameObject player;
    private float DPdistance; // 문-플레이어 사이 거리
    private float Ddistance;    // 문이 열릴 때까지 (= 닫힐 때까지) 거리
    private Vector3 BPdistance;   // 버튼-플레이어 사이 벡터
    
    private void Start()
    {
        player = GameManager_Lobby.instance.GetPlayer();
        Ddistance = DdestX - door.transform.position.x;
        DfirstX = door.transform.position.x;
        doorPos = door.transform.position;
        StartCoroutine(ShowButtonScript());

        UIManager_Lobby.instance.ShowPressTutorial();
    }

    #region 버튼-문 함수
    public void DoorOpen()
    {
        StartCoroutine(OpenCoroutine());
    }

    IEnumerator OpenCoroutine()
    {
        float timer = 0f;

        while (true)
        {
            if (DdestX - door.transform.position.x <= 0.01f) { door.transform.position = new Vector3(DdestX, door.transform.position.y, door.transform.position.z); break; }
            timer += Time.deltaTime;
            door.transform.position = new Vector3(Mathf.Lerp(DfirstX, DdestX, timer / openTimer), door.transform.position.y, door.transform.position.z);
            yield return null;
        }
        StartCoroutine(CalculateDistance());
    }

    IEnumerator CalculateDistance()
    {
        while (true)
        {
            DPdistance = (player.transform.position - doorPos).magnitude;
            if (DPdistance >= DPFarDistance) { break; }
            yield return new WaitForSeconds(0.2f);
        }
        // 여기에 문 닫는 코루틴 시작
        StartCoroutine(CloseCoroutine());
    }

    IEnumerator CloseCoroutine()
    {
        float timer = 0f;

        while (true)
        {
            if (door.transform.position.x - DfirstX <= 0.01f) { door.transform.position = new Vector3(DfirstX, door.transform.position.y, door.transform.position.z); yield break; }
            timer += Time.deltaTime;
            door.transform.position = new Vector3(Mathf.Lerp(DdestX, DfirstX, timer / closeTimer), door.transform.position.y, door.transform.position.z);
            yield return null;
        }
    }
    #endregion

    #region 버튼-플레이어 자막 함수

    IEnumerator ShowButtonScript()
    {
        while (true)
        {
            BPdistance = button.transform.position - player.transform.position;
            if (BPdistance.z < 0 && BPdistance.magnitude <= BPCloseDistance) { break; }
            yield return null;
        }
        ChangeScript();
    }

    private void ChangeScript()
    {
        UIManager_Lobby.instance.ChangeQuest("버튼을 눌러보자!");
        UIManager_Lobby.instance.ShowPressTutorial();
    }

    public void HideButtonScript()
    {
        UIManager_Lobby.instance.HidePressTutorial();
        UIManager_Lobby.instance.HideQuest();
    }

    #endregion
}