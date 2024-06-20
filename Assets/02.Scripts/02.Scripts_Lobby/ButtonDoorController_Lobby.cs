using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ButtonDoorController_Lobby : MonoBehaviour
{
    public GameObject door;
    public float farDistance = 0.5f;    // ��-�÷��̾� ���� �Ÿ��� ��������� �� ���� ���� ��
    public float openTimer = 1f;        // ���� ������ ���� �ɸ��� �ð�
    public float closeTimer = 0.5f;     // ���� ������ ���� �ɸ��� �ð�
    public float destX = 2.3f;          // �� ������ ������ ���� X ��
    private float firstX;               // �� ������ ������ ���� X ��
    private Vector3 doorPos;            // ó�� ���� ��ġ
    private GameObject player;
    private float DPdistance; // ��-�÷��̾� ���� �Ÿ�
    private float Ddistance;    // ���� ���� ������ (= ���� ������) �Ÿ�
    
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
        // ���⿡ �� �ݴ� �ڷ�ƾ ����
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