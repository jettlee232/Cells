using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class RayDescription_Mito : MonoBehaviour
{
    public GameObject descrptionPanel = null; // ����â ������Ʈ�� �޴� ����
    public GameObject laserDescriptionCanvas; // ����â ������
    public Transform descriptionPanelSpawnPoint; // ����â�� ���� ��ġ
    public string objName; // ����â�� ����Ű�� ������Ʈ�� �̸�

    public BNG.UIPointer uiPointer; // ������ ������Ʈ
    //public bool isTriggerPressed = false; // ���� Ʈ���Ű� �������� �� ��������
    public bool isGripPressed = false; // ���� �׸��� �������� �� ��������
    public bool isButtonPressed = false; // ������ A��ư�� �������� �� ��������

    UnityEngine.XR.InputDevice right; // ������ ��Ʈ�ѷ� ���¸� �޴� ����

    public RaycastHit rayHit;
    public Grabber grabber;

    void Start()
    {
        uiPointer = gameObject.GetComponent<BNG.UIPointer>(); // UI ������Ʈ �ޱ�
        descrptionPanel = null;
        objName = "";
    }

    void Update()
    {
        // �� bool�� �����鿡 Ʈ���� ��ư�� A��ư�� �������� �� �������� �ǽð����� �ޱ�        
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        //right.TryGetFeatureValue(CommonUsages.triggerButton, out isTriggerPressed);
        right.TryGetFeatureValue(CommonUsages.gripButton, out isGripPressed);
        right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);

        if (isButtonPressed == true) // ��ư�� ������ �ִٸ�
        {
            uiPointer.HidePointerIfNoObjectsFound = false; // ������ ���̰� �ϱ�

            rayHit = CheckRay(transform.position, transform.forward, 1f); // ���� �������� ���� ������Ʈ�� ���� �˻��ϱ�           
        }
        else // ��ư�� �� ������ �ִٸ�
        {
            uiPointer.HidePointerIfNoObjectsFound = true; // ������ �� ���̰� �ϱ�    
        }

        if (descrptionPanel != null) // ���� ����â�� ������� ���¶��
        {
            FollowingDescription(descrptionPanel); // ���� ������� ����â�� �� �ü��� ������� �ϱ�
        }

        //if (isGripPressed && descrptionPanel != null) // ���� ����â�� ������� �����̰� �׸���ư�� ������
        //{
        //    if (!grabber.InputCheckGrab())
        //        CatchItem(rayHit.collider.gameObject); // �ش� ������ ��������
        //}
        if (isButtonPressed == false && descrptionPanel != null) // ���� ����â�� ������� �����̰� A��ư�� �� ���� ���¶��
        {
            DestroyDescription(); // ���� ������� ����â�� ���ֱ�
        }
    }

    public RaycastHit CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction);

        if (Physics.Raycast(ray, out RaycastHit rayHit, length))
        {
            // ������ �հ��� �������� �������� ���� DescObj��� Layer�� ���� ������Ʈ���...
            if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("DescObj"))
            {
                // ���� ����â�� ����Ű�� ������Ʈ�� �̸��� �������� ���� ������Ʈ�� �̸��� �ٸ��ٸ�...
                // (����â�� ���� ���¶�� objName���� �ƹ��͵� �� ������������)
                if (objName != rayHit.collider.gameObject.name)
                {
                    objName = rayHit.collider.gameObject.name; // objName���� �������� ���� ������Ʈ�� �̸��� �ֱ�

                    InstantiatePanel(rayHit.collider.gameObject); // �г� �����
                }
            }
        }
        return rayHit;
    }

    public void InstantiatePanel(GameObject go)
    {
        if (descrptionPanel != null) // �̹� ������� �ִ� �г��� �ִٸ� �� �г��� �����
        {
            DestroyDescription();
        }

        // �г� ����� ��ġ�� ���� �ֱ�
        descrptionPanel = Instantiate(laserDescriptionCanvas);

        descrptionPanel.transform.position = descriptionPanelSpawnPoint.position;
        descrptionPanel.transform.rotation = Quaternion.identity;

        // �г��� �ʱ� ũ��� �۰� �����ϱ�
        descrptionPanel.GetComponent<RectTransform>().localScale = new Vector3(0.0002f, 0.0002f, 0.0002f);

        // ������Ʈ ���� ����
        MakeDescription(go);
    }


    public void FollowingDescription(GameObject descPanel) // �г��� �÷��̾� �ü� ���󰡰� �ϱ�
    {
        // �� �κ��� ������ ȿ���� ���� �����ǵ� ��� ��� �Ǳ� �ҵ��ϳ׿�... ���� �ʿ� ���� ������ ���� �˴ϴ�
        //if (descPanel.GetComponent<RectTransform>().localScale.x < 0.002f)
        //{
        //    descPanel.GetComponent<RectTransform>().localScale =
        //    new Vector3(descPanel.GetComponent<RectTransform>().localScale.x + 0.0005f,
        //    descPanel.GetComponent<RectTransform>().localScale.y + 0.0005f,
        //    descPanel.GetComponent<RectTransform>().localScale.z + 0.0005f);
        //}
        //

        // �г��� ��ġ�� ������ �гν�������Ʈ�� ��ġ�� ������ ��ġ�ϵ��� ������ �ǽð� ����
        descPanel.transform.position = descriptionPanelSpawnPoint.position;
        descPanel.transform.rotation = descriptionPanelSpawnPoint.rotation;
    }

    public void DestroyDescription() // �г� ���ֱ�
    {
        Destroy(descrptionPanel);
        objName = ""; // ���� ����Ű�� ������Ʈ�� ������ �˸��� ���� objName�� ����
    }

    public void MakeDescription(GameObject go) // ���� ������Ʈ�� �̸��� ������ ���� ����â �ؽ�Ʈ�� �����ϱ�
    {
        // �ϴ� �� �ڵ忡���� GameObject�� name���� �˻��ϱ� �ߴµ�, �ټ� ������ ����̴� Tag�� Layer�� �� �� ������Ʈ�� �ٸ� ���� ���̵� �� �� ����Ʈ�� ���ǽ��� ����ϱ⸦ ����

        // ���ӿ�����Ʈ�� �̸����� �˻��ؼ� ����â�� ���� ���ǽ� (���� ���� �������̵� json �������̵� ���� ���� �ʿ�)
        //if (go.name == "Cube1")
        //{
        //    descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Cube1";
        //    descrptionPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "My Name Is Cube 1";
        //}
        //else if (go.name == "Cube2")
        //{
        //    descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Cube2";
        //    descrptionPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "My Name Is Cube 2";
        //}

        // ���� �г��� ����Ű�� ������Ʈ�� �̸��� ����
        //objName = descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text;
    }
}
