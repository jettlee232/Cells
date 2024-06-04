using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class LaserPointer_Lobby : MonoBehaviour
{
    public string objName = null; // ����â�� ����Ű�� ������Ʈ�� �̸�

    public BNG.UIPointer uiPointer; // ������ ������Ʈ
    public bool isTriggerPressed = false; // Ʈ���Ű� �������� �� ��������
    public bool isButtonPressed = false; // ������ A��ư�� �������� �� ��������

    private GameObject descPanel = null;
    private int descObjLayer;

    UnityEngine.XR.InputDevice right; // ������ ��Ʈ�ѷ� ���¸� �޴� ����

    void Start()
    {
        uiPointer = gameObject.GetComponent<BNG.UIPointer>(); // UI ������Ʈ �ޱ�
        descPanel = UIManager_Lobby.instance.GetDesc();
        objName = null;
        descObjLayer = 1 << LayerMask.NameToLayer("DescObj");
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
            if (isButtonPressed) { DestroyDescription(); }
        }
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction.normalized);

        if (Physics.Raycast(ray, out RaycastHit rayHit, length, descObjLayer))
        {
            // ���� ����â�� ����Ű�� ������Ʈ�� �̸��� �������� ���� ������Ʈ�� �̸��� �ٸ��ٸ�...
            // (����â�� ���� ���¶�� objName���� �ƹ��͵� �� ������������)
            if (objName != rayHit.collider.gameObject.GetComponent<DescObj_Lobby>().GetName())
            {
                objName = rayHit.collider.gameObject.GetComponent<DescObj_Lobby>().GetName();
                InstantiatePanel(rayHit.collider.gameObject);
            }
        }
    }

    public void InstantiatePanel(GameObject go)
    {
        if (descPanel.activeSelf) { DestroyDescription(); }

        descPanel.SetActive(true);

        // �г��� �ʱ� ũ��� �۰� �����ϱ�
        descPanel.GetComponent<RectTransform>().localScale = Vector3.one * 0.00125f;
        // ������Ʈ ���� ����
        MakeDescription(go);
    }


    public void FollowingDescription(GameObject descPanel) // �г��� �÷��̾� �ü� ���󰡰� �ϱ�
    {
        // �� �κ��� ������ ȿ���� ���� �����ǵ� ��� ��� �Ǳ� �ҵ��ϳ׿�... ���� �ʿ� ���� ������ ���� �˴ϴ�
        if (descPanel.GetComponent<RectTransform>().localScale.x < 0.00125f)
        {
            descPanel.GetComponent<RectTransform>().localScale =
            new Vector3(descPanel.GetComponent<RectTransform>().localScale.x + 0.00005f,
            descPanel.GetComponent<RectTransform>().localScale.y + 0.00005f,
            descPanel.GetComponent<RectTransform>().localScale.z + 0.00005f);
        }
    }

    public void DestroyDescription() // �г� ���ֱ�
    {
        descPanel.SetActive(false);
        objName = null; // ���� ����Ű�� ������Ʈ�� ������ �˸��� ���� objName�� ����
    }

    public void MakeDescription(GameObject go) // ���� ������Ʈ�� �̸��� ������ ���� ����â �ؽ�Ʈ�� �����ϱ�
    {
        descPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = go.GetComponent<DescObj_Lobby>().GetName();
        descPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = go.GetComponent<DescObj_Lobby>().GetDesc();

        // ���� �г��� ����Ű�� ������Ʈ�� �̸��� ����
        objName = descPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text;
    }
}
