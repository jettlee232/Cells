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
        { "Cube1", "My Name Is Cube 1" },
        { "Cube2", "My Name Is Cube 2" },
        { "Cube3", "My Name Is Cube 3" },
        { "Sphere.013_CM", "This is Sphere.013_CM" },
        { "Sphere.012_CM", "This is Sphere.012_CM" },
        { "Sphere.011_CM", "This is Sphere.011_CM" },
        { "Sphere.010_CM", "This is Sphere.010_CM" },
        { "Sphere.009_CM", "This is Sphere.009_CM" },
        { "Sphere.008_CM", "This is Sphere.008_CM" },
        { "Sphere.007_CM", "This is Sphere.007_CM" },
        { "Sphere.006_CM", "This is Sphere.006_CM" },
        { "Sphere.005_CM", "This is Sphere.005_CM" },
        { "Sphere.004_CM", "This is Sphere.004_CM" },
        { "Sphere.003_CM", "This is Sphere.003_CM" },
        { "Sphere.002_CM", "This is Sphere.002_CM" },
        { "Sphere.001_CM", "This is Sphere.001_CM" },
        { "Phospholipid_Tail_CM", "This is Phospholipid_Tail_CM"},
        { "Phospholipid_Single_CM", "This is Phospholipid_Single_CM" },
        { "Phospholipid_Head_CM", "This is Phospholipid_Head_CM" },
        { "Phospholipid_Double_CM", "This is Phospholipid_Double_CM" },
        { "Phospholipid_Bulk_CM", "This is Phospholipid_Bulk_CM "},
        { "Phospholipid_Single_EyesOnlyCM", "This is Phospholipid_Single_EyesOnlyCM "},
        { "Phospholipid_Double_EyesOnly_CM", "This is Phospholipid_Double_EyesOnly_CM "},
        { "Phospholipid_Bulk_EyesOnly_CM", "This is Phospholipid_Bulk_EyesOnly_CM "}
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
        // �ϴ� �� �ڵ忡���� GameObject�� name���� �˻��ϱ� �ߴµ�, �ټ� ������ ����̴� Tag�� Layer�� �� �� ������Ʈ�� �ٸ� ���� ���̵� �� �� ����Ʈ�� ���ǽ��� ����ϱ⸦ ����

        // ���ӿ�����Ʈ�� �̸����� �˻��ؼ� ����â�� ���� ���ǽ� (���� ���� �������̵� json �������̵� ���� ���� �ʿ�)
        if (objDesc.ContainsKey(go.name))
        {
            descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = go.name;
            descrptionPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = objDesc[go.name];
        }

        // ���� �г��� ����Ű�� ������Ʈ�� �̸��� ����
        objName = descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text;

        AudioMgr_CM.Instance.PlaySFXByInt(2);
    }
}
