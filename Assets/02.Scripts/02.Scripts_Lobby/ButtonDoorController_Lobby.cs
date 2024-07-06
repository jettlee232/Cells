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
    public float DPFarDistance = 0.5f;    // ��-�÷��̾� ���� �Ÿ��� ��������� �� ���� ���� ��
    public float openTimer = 1f;        // ���� ������ ���� �ɸ��� �ð�
    public float closeTimer = 0.5f;     // ���� ������ ���� �ɸ��� �ð�
    public float DdestX = 2.3f;          // �� ������ ������ ���� X ��
    private float DfirstX;               // �� ������ ������ ���� X ��
    private Vector3 doorPos;            // ó�� ���� ��ġ

    [Header("Subtitle Script")]
    public float BPCloseDistance = 2f;       // ��ư-�÷��̾� ���� �Ÿ��� ��������� �� �ڸ��� �� ��

    private GameObject player;
    private float DPdistance; // ��-�÷��̾� ���� �Ÿ�
    private float Ddistance;    // ���� ���� ������ (= ���� ������) �Ÿ�
    private Vector3 BPdistance;   // ��ư-�÷��̾� ���� ����
    
    private void Start()
    {
        player = GameManager_Lobby.instance.GetPlayer();
        Ddistance = DdestX - door.transform.position.x;
        DfirstX = door.transform.position.x;
        doorPos = door.transform.position;
        StartCoroutine(ShowButtonScript());

        UIManager_Lobby.instance.ShowPressTutorial();
    }

    #region ��ư-�� �Լ�
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
        // ���⿡ �� �ݴ� �ڷ�ƾ ����
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

    #region ��ư-�÷��̾� �ڸ� �Լ�

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
        UIManager_Lobby.instance.ChangeQuest("��ư�� ��������!");
        UIManager_Lobby.instance.ShowPressTutorial();
    }

    public void HideButtonScript()
    {
        UIManager_Lobby.instance.HidePressTutorial();
        UIManager_Lobby.instance.HideQuest();
    }

    #endregion
}