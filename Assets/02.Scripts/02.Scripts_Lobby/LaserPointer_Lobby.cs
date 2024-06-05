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

    UnityEngine.XR.InputDevice right; // ������ ��Ʈ�ѷ� ���¸� �޴� ����

    void Start()
    {
        uiPointer = gameObject.GetComponent<BNG.UIPointer>(); // UI ������Ʈ �ޱ�
        descPanel = UIManager_Lobby.instance.GetDesc();
        obj = null;
        descObjLayer = 1 << LayerMask.NameToLayer("DescObj");
        player = GameManager_Lobby.instance.GetPlayer();
        mainCam = GameManager_Lobby.instance.GetPlayerCam().GetComponent<Camera>();
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
            FollowingDescription(UIManager_Lobby.instance.GetDesc()); // ���� ������� ����â�� �� �ü��� ������� �ϱ�
            if (isButtonPressed || !CheckSight()) { DestroyDescription(); }
        }
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction.normalized);

        if (Physics.Raycast(ray, out RaycastHit rayHit, length, descObjLayer))
        {
            if (obj != rayHit.collider.gameObject)
            {
                obj = rayHit.collider.gameObject;
                InstantiatePanel(obj);
            }
        }
    }

    public void InstantiatePanel(GameObject go)
    {
        if (descPanel.activeSelf) { DestroyDescription(); }
        
        descPanel.SetActive(true);
        descPanel.GetComponent<RectTransform>().localScale = Vector3.one * 0.00125f;
        MakeDescription(go);
    }


    public void FollowingDescription(GameObject descPanel) // �г��� �÷��̾� �ü� ���󰡰� �ϱ�
    {
        if (descPanel.GetComponent<RectTransform>().localScale.x < 0.00125f)
        {
            descPanel.GetComponent<RectTransform>().localScale =
            new Vector3(descPanel.GetComponent<RectTransform>().localScale.x + 0.00001f,
            descPanel.GetComponent<RectTransform>().localScale.y + 0.00001f,
            descPanel.GetComponent<RectTransform>().localScale.z + 0.00001f);
        }
    }

    public void DestroyDescription() // �г� ���ֱ�
    {
        descPanel.SetActive(false);
        obj = null;
    }

    public void MakeDescription(GameObject go) // ���� ������Ʈ�� �̸��� ������ ���� ����â �ؽ�Ʈ�� �����ϱ�
    {
        descPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = go.GetComponent<DescObj_Lobby>().GetName();
        descPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = go.GetComponent<DescObj_Lobby>().GetDesc();
    }

    private bool CheckSight()
    {
        Vector3 viewportPos = mainCam.WorldToViewportPoint(obj.transform.position);
        bool isInView = viewportPos.z > 0f && (viewportPos.x > 0f && viewportPos.x < 1f) && (viewportPos.y > 0f && viewportPos.y < 1f);

        Vector3 closest = obj.GetComponent<Collider>().ClosestPoint(player.transform.position);
        Vector3 vDistance = (closest - player.transform.position);
        bool isClose = (vDistance.magnitude <= maxDistance) ? true : false;

        if (isInView && isClose) { return true; }
        else { return false; }
    }
}
