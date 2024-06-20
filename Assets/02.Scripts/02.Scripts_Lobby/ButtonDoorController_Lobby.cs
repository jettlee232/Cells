using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ButtonDoorController_Lobby : MonoBehaviour
{
    public GameObject door;
    public float farDistance = 0.5f;    // 문-플레이어 사이 거리가 어느정도일 때 문이 닫힐 지
    public float openTimer = 1f;        // 문이 열리는 데에 걸리는 시간
    public float closeTimer = 0.5f;     // 문이 닫히는 데에 걸리는 시간
    public float destX = 2.3f;          // 문 완전히 열렸을 때의 X 값
    private float firstX;               // 문 완전히 닫혔을 때의 X 값
    private Vector3 doorPos;            // 처음 문의 위치
    private GameObject player;
    private float DPdistance; // 문-플레이어 사이 거리
    private float Ddistance;    // 문이 열릴 때까지 (= 닫힐 때까지) 거리
    
    private void Start()
    {
        player = GameManager_Lobby.instance.GetPlayer();
        Ddistance = destX - door.transform.position.x;
        firstX = door.transform.position.x;
        doorPos = door.transform.position;
    }

    public void DoorOpen()
    {
        StartCoroutine(OpenCoroutine());
    }

    IEnumerator OpenCoroutine()
    {
        float timer = 0f;

        while (true)
        {
            if (destX - door.transform.position.x <= 0.01f) { door.transform.position = new Vector3(destX, door.transform.position.y, door.transform.position.z); break; }
            timer += Time.deltaTime;
            door.transform.position = new Vector3(Mathf.Lerp(firstX, destX, timer / openTimer), door.transform.position.y, door.transform.position.z);
            yield return null;
        }
        StartCoroutine(CalculateDistance());
    }

    IEnumerator CalculateDistance()
    {
        while (true)
        {
            DPdistance = (player.transform.position - doorPos).magnitude;
            if (DPdistance >= farDistance) { break; }
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
            if (door.transform.position.x - firstX <= 0.01f) { door.transform.position = new Vector3(firstX, door.transform.position.y, door.transform.position.z); yield break; }
            timer += Time.deltaTime;
            door.transform.position = new Vector3(Mathf.Lerp(destX, firstX, timer / closeTimer), door.transform.position.y, door.transform.position.z);
            yield return null;
        }
    }
}