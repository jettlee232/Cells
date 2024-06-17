using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;


public class LaserPointerAndDescription_CM : MonoBehaviour
{
    public GameObject descrptionPanel = null;
    public GameObject laserDescriptionCanvas;
    public Transform descriptionPanelSpawnPoint;
    public string objName;

    public BNG.UIPointer uiPointer;
    public bool isTriggerPressed = false;
    public bool isButtonPressed = false;

    UnityEngine.XR.InputDevice right;

    public GameObject glowObj;

    void Start()
    {        
        uiPointer = gameObject.GetComponent<BNG.UIPointer>();
        descrptionPanel = null;

        objName = "";        
    }

    void Update()
    {
        // �� bool�� �����鿡 Ʈ���� ��ư�� A��ư�� �������� �� �������� �ǽð����� �ޱ�
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.triggerButton, out isTriggerPressed);
        right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);

        if (isTriggerPressed == true) // Ʈ���Ű� ������ �ִٸ�
        {
            uiPointer.HidePointerIfNoObjectsFound = false; // ������ ���̰� �ϱ�

            CheckRay(transform.position, transform.forward, 10f); // ���� �������� ���� ������Ʈ�� ���� �˻��ϱ�           
        }
        else // Ʈ���Ű� �� ������ �ִٸ�
        {
            uiPointer.HidePointerIfNoObjectsFound = true; // ������ �� ���̰� �ϱ�    
        }

        if (descrptionPanel != null) // ���� ����â�� �� ������� ���¶��
        {
            FollowingDescription(descrptionPanel); // ���� ������� ����â�� �� �ü��� ������� �ϱ�
        }
        if (isButtonPressed == true && descrptionPanel != null) // ���� ����â�� ������� �����̰� A��ư�� ���� ���¶��
        {
            DestroyDescription(); // ���� ������� ����â�� ���ֱ�
        }
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
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

                    //glowObj = rayHit.collider.gameObject;
                    rayHit.collider.gameObject.GetComponent<HighlightEffect>().highlighted = true;
                    rayHit.collider.gameObject.GetComponent<HighLightColorchange_CM>().GlowStart();

                    InstantiatePanel(rayHit.collider.gameObject); // �г� �����
                }
            }
        }
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

        glowObj = go;
    }

    
    public void FollowingDescription(GameObject descPanel) // �г��� �÷��̾� �ü� ���󰡰� �ϱ�
    {
        // �� �κ��� ������ ȿ���� ���� �����ǵ� ��� ��� �Ǳ� �ҵ��ϳ׿�... ���� �ʿ� ���� ������ ���� �˴ϴ�
        if (descPanel.GetComponent<RectTransform>().localScale.x < 0.002f)
        {
            descPanel.GetComponent<RectTransform>().localScale =
            new Vector3(descPanel.GetComponent<RectTransform>().localScale.x + 0.0005f,
            descPanel.GetComponent<RectTransform>().localScale.y + 0.0005f,
            descPanel.GetComponent<RectTransform>().localScale.z + 0.0005f);
        }
        //
        
        // �г��� ��ġ�� ������ �гν�������Ʈ�� ��ġ�� ������ ��ġ�ϵ��� ������ �ǽð� ����
        descPanel.transform.position = descriptionPanelSpawnPoint.position;
        descPanel.transform.rotation = descriptionPanelSpawnPoint.rotation;
    }

    public void DestroyDescription() // �г� ���ֱ�
    {
        if (glowObj != null)
        {
            glowObj.GetComponent<HighlightEffect>().highlighted = false;
            glowObj.GetComponent<HighLightColorchange_CM>().GlowEnd();
        }
                       
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
