using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class FollowingUI_Mito : MonoBehaviour
{
    public GameObject descrptionPanel = null; // ����â ������Ʈ�� �޴� ����
    public GameObject inventoryUI; // ����â ������
    public Transform descriptionPanelSpawnPoint; // ����â�� ���� ��ġ
    public string objName; // ����â�� ����Ű�� ������Ʈ�� �̸�

    UnityEngine.XR.InputDevice left;
    UnityEngine.XR.InputDevice right; // ������ ��Ʈ�ѷ� ���¸� �޴� ����

    void Start()
    {
        descrptionPanel = null;
        objName = "";
    }

    void Update()
    {
        left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        bool inventoryButtonPressed;
        left.TryGetFeatureValue(CommonUsages.secondaryButton, out inventoryButtonPressed);
        if (inventoryButtonPressed)
        {
            ToggleInventoryUI();
        }

        if (descrptionPanel != null) // ���� ����â�� ������� ���¶��
        {
            FollowingDescription(descrptionPanel); // ���� ������� ����â�� �� �ü��� ������� �ϱ�
        }

    }

    private void ToggleInventoryUI()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    public void InstantiatePanel(GameObject go)
    {
        if (descrptionPanel != null) // �̹� ������� �ִ� �г��� �ִٸ� �� �г��� �����
        {
            DestroyDescription();
        }

        // �г� ����� ��ġ�� ���� �ֱ�
        descrptionPanel = Instantiate(inventoryUI);

        descrptionPanel.transform.position = descriptionPanelSpawnPoint.position;
        descrptionPanel.transform.rotation = Quaternion.identity;

        // �г��� �ʱ� ũ��� �۰� �����ϱ�
        //descrptionPanel.GetComponent<RectTransform>().localScale = new Vector3(0.0002f, 0.0002f, 0.0002f);

        // ������Ʈ ���� ����
        //MakeDescription(go);
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
        if (go.name == "Cube1")
        {
            descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Cube1";
            descrptionPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "My Name Is Cube 1";
        }
        else if (go.name == "Cube2")
        {
            descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Cube2";
            descrptionPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "My Name Is Cube 2";
        }

        // ���� �г��� ����Ű�� ������Ʈ�� �̸��� ����
        objName = descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text;
    }
}
