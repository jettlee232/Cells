using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class LaserPointer_Lobby : MonoBehaviour
{
    public BNG.UIPointer uiPointer; // ������ ������Ʈ
    public bool isTriggerPressed = false; // Ʈ���Ű� �������� �� ��������
    public bool isButtonPressed = false; // ������ A��ư�� �������� �� ��������
    public float maxDistance;

    public GameObject obj = null;
    private GameObject descPanel = null;
    private int descObjLayer;
    private GameObject player = null;
    private Camera mainCam = null;
    private GameObject NPC = null;
    private bool isDialogue = false;

    UnityEngine.XR.InputDevice right; // ������ ��Ʈ�ѷ� ���¸� �޴� ����

    void Start()
    {
        uiPointer = gameObject.GetComponent<BNG.UIPointer>(); // UI ������Ʈ �ޱ�
        descPanel = UIManager_Lobby.instance.GetDesc();
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
        right.TryGetFeatureValue(CommonUsages.triggerButton, out isTriggerPressed);
        right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);

        if (isTriggerPressed) // Ʈ���Ű� ������ �ִٸ�
        {
            uiPointer.HidePointerIfNoObjectsFound = false; // ������ ���̰� �ϱ�
            CheckRay(transform.position, transform.forward, 10f); // ���� �������� ���� ������Ʈ�� ���� �˻��ϱ�
        }
        else { uiPointer.HidePointerIfNoObjectsFound = true; }

        if (descPanel.activeSelf) // ���� ����â�� ������� ���¶��
        {
            if (!CheckSight()) { DestroyDescription(); }
        }
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction.normalized);

        if (Physics.Raycast(ray, out RaycastHit rayHit, length))
        {
            if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("DescObj"))
            {
                if (obj != rayHit.collider.gameObject)
                {
                    obj = rayHit.collider.gameObject;
                    InstantiatePanel(obj);
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
                    else { NPC.GetComponent<SelectDialogue_Lobby>().ActivateDST3(); }
                }
                else { NPC.GetComponent<SelectDialogue_Lobby>().ActivateDST1(); }
            }
        }
    }

    public void InstantiatePanel(GameObject go)
    {
        if (descPanel.activeSelf) { DestroyDescription(); }

        UIManager_Lobby.instance.OnDesc();
        descPanel.GetComponent<RectTransform>().localScale = Vector3.one * 0.00125f;
        MakeDescription(go);
    }

    public void DestroyDescription() // �г� ���ֱ�
    {
        descPanel.SetActive(false);
        obj = null;
    }

    public void MakeDescription(GameObject go) // ���� ������Ʈ�� �̸��� ������ ���� ����â �ؽ�Ʈ�� �����ϱ�
    {
        descPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = go.GetComponent<DescObj_Lobby>().GetName();
    }

    private bool CheckSight()
    {
        Vector3 viewportPos = Vector3.one;
        if (obj == null || mainCam == null || obj.transform == null) { DestroyDescription(); }
        else { mainCam.WorldToViewportPoint(obj.transform.position); }

        bool isInView = viewportPos.z > 0f && (viewportPos.x > 0f && viewportPos.x < 1f) && (viewportPos.y > 0f && viewportPos.y < 1f);

        return isInView;
    }

    public void IsTalking() { isDialogue = true; }
}
