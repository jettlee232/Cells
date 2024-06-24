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

    public LineRenderer line;
    public BNG.UIPointer uiPointer;
    public bool isTriggerPressed = false;
    public bool isButtonPressed = false;

    UnityEngine.XR.InputDevice right;

    public GameObject glowObj;

    public Dictionary<string, string> objDesc = new Dictionary<string, string>
    {
        { "Sphere.013_CM", "DescPanel_CM_S13" },
        { "Sphere.012_CM", "DescPanel_CM_S12" },
        { "Sphere.011_CM", "DescPanel_CM_S11" },
        { "Sphere.010_CM", "DescPanel_CM_S10" },
        { "Sphere.009_CM", "DescPanel_CM_S9" },
        { "Sphere.008_CM", "DescPanel_CM_S8" },
        { "Sphere.007_CM", "DescPanel_CM_S7" },
        { "Sphere.006_CM", "DescPanel_CM_S6" },
        { "Sphere.005_CM", "DescPanel_CM_S5" },
        { "Sphere.004_CM", "DescPanel_CM_S4" },
        { "Sphere.003_CM", "DescPanel_CM_S3" },
        { "Sphere.002_CM", "DescPanel_CM_S2" },
        { "Sphere.001_CM", "DescPanel_CM_S1" },
        { "Phospholipid_Tail_CM", "DescPanel_CM_P1"},
        { "Phospholipid_Single_CM", "DescPanel_CM_P2" },
        { "Phospholipid_Head_CM", "DescPanel_CM_P3" },
        { "Phospholipid_Double_CM", "DescPanel_CM_P4" },
        { "Phospholipid_Bulk_CM", "DescPanel_CM_P5" },
        { "Phospholipid_Single_EyesOnlyCM", "DescPanel_CM_E1" },
        { "Phospholipid_Double_EyesOnly_CM", "DescPanel_CM_E2" },
        { "Phospholipid_Bulk_EyesOnly_CM", "DescPanel_CM_E3" }
    };
    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        uiPointer = gameObject.GetComponent<BNG.UIPointer>();
        descrptionPanel = null;

        objName = "";
    }

    void Update()
    {
        // �� bool�� �����鿡 Ʈ���� ��ư�� A��ư�� �������� �� �������� �ǽð����� �ޱ�
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (right.isValid)
        {
            right.TryGetFeatureValue(CommonUsages.triggerButton, out isTriggerPressed);
            right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);
        }

        if (isTriggerPressed == true) // Ʈ���Ű� ������ �ִٸ�
        {
            //uiPointer.enabled = true;
            //uiPointer.HidePointerIfNoObjectsFound = false; // ������ ���̰� �ϱ�

            line.enabled = true;

            CheckRay(transform.position, transform.forward, 10f); // ���� �������� ���� ������Ʈ�� ���� �˻��ϱ�           
        }
        else // Ʈ���Ű� �� ������ �ִٸ�
        {
            //uiPointer.enabled = false;
            //uiPointer.HidePointerIfNoObjectsFound = true; // ������ �� ���̰� �ϱ�    

            line.enabled = false;
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
                    var highlightEffect = rayHit.collider.gameObject.GetComponent<HighlightEffect>();
                    if (highlightEffect != null)
                    {
                        highlightEffect.highlighted = true;
                        rayHit.collider.gameObject.GetComponent<HighLightColorchange_CM>().GlowStart();
                    }

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
        if (FireStoreManager_Test_CM.Instance.csvData.ContainsKey(go.name))
        {
            descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text =
            FireStoreManager_Test_CM.Instance.ReadCSV(go.name);

            descrptionPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text =
            FireStoreManager_Test_CM.Instance.ReadCSV(go.name + "_D");
        }

        objName = descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text;

        AudioMgr_CM.Instance.PlaySFXByInt(2);
    }
}
