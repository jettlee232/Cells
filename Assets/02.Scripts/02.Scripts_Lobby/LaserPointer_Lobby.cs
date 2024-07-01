using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class LaserPointer_Lobby : MonoBehaviour
{
    private BNG.UIPointer uiPointer; // ������ ������Ʈ
    private bool isButtonPressed = false; // ������ A��ư�� �������� �� ��������
    public float maxDistance;

    private GameObject obj = null;
    private int descObjLayer;
    private GameObject player = null;
    private Camera mainCam = null;
    private GameObject NPC = null;
    private bool isDialogue = false;

    UnityEngine.XR.InputDevice right; // ������ ��Ʈ�ѷ� ���¸� �޴� ����

    void Start()
    {
        uiPointer = gameObject.GetComponent<BNG.UIPointer>(); // UI ������Ʈ �ޱ�
        obj = null;
        descObjLayer = 1 << LayerMask.NameToLayer("DescObj");
        player = GameManager_Lobby.instance.GetPlayer();
        mainCam = GameManager_Lobby.instance.GetPlayerCam().GetComponent<Camera>();
        NPC = GameManager_Lobby.instance.GetNPC();
    }

    void Update()
    {
        // �� bool�� �����鿡 Ʈ���� ��ư�� A��ư�� �������� �� �������� �ǽð����� �ޱ�        
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);

        if (isButtonPressed) // ��ư�� ������ �ִٸ�
        {
            uiPointer.HidePointerIfNoObjectsFound = false; // ������ ���̰� �ϱ�
            CheckRay(transform.position, transform.forward, maxDistance); // ���� �������� ���� ������Ʈ�� ���� �˻��ϱ�
        }
        else { uiPointer.HidePointerIfNoObjectsFound = true; }

        if (obj != null && obj.gameObject.layer == LayerMask.NameToLayer("DescObj")) // ���� ����â�� ������� ���¶��
        {
            if (!CheckSight()) { DestroyDescription(); }
        }
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction.normalized);

        if (Physics.Raycast(ray, out RaycastHit rayHit, length))
        {
            if (rayHit.collider.gameObject.CompareTag("Interactable"))
            {
                if (obj != rayHit.collider.gameObject)
                {
                    obj = rayHit.collider.gameObject;
                    obj.transform.GetChild(0).GetComponent<TextObject_Lobby>().ShowText();
                }
            }
            else if (rayHit.collider.gameObject.CompareTag("NPC"))
            {
                if (GameManager_Lobby.instance.firstEnd)
                {
                    if (GameManager_Lobby.instance.secondCon)
                    {
                        // �ι�° ��ȭ ���� ����
                        NPC.GetComponent<SelectDialogue_Lobby>().ActivateDST2();
                    }
                    else { return; }
                }
                else { NPC.GetComponent<SelectDialogue_Lobby>().ActivateDST1(); }
            }
        }
    }

    public void DestroyDescription() // 3D Text �г� ���ֱ�
    {
        if (obj == null) return;
        obj.transform.GetChild(0).GetComponent<TextObject_Lobby>().HideText();
        obj = null;
    }

    private bool CheckSight()
    {
        Vector3 viewportPos = Vector3.one;
        if (obj == null || mainCam == null || obj.transform == null) { DestroyDescription(); }
        else { viewportPos = mainCam.WorldToViewportPoint(obj.transform.position); }

        bool isInView = viewportPos.z > 0f && (viewportPos.x >= 0f && viewportPos.x <= 1f) && (viewportPos.y >= 0f && viewportPos.y <= 1f);

        return isInView;
    }
}
