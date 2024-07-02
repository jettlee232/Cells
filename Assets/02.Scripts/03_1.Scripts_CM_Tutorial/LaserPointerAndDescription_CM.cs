using HighlightPlus;
using PixelCrushers;
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
    public bool isButtonPressed = false;
    private bool wasButtonPressed = false;

    UnityEngine.XR.InputDevice right;

    public GameObject glowObj;

    private bool canMakeRay = true;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        uiPointer = gameObject.GetComponent<BNG.UIPointer>();
        descrptionPanel = null;

        objName = "";
    }

    void Update()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (right.isValid)
        {
            right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);
        }

        if (canMakeRay == true)
        {
            if (isButtonPressed == true)
            {
                line.enabled = true;

                CheckRay(transform.position, transform.forward, 10f);
            }
            else
            {
                line.enabled = false;
            }
        }

        if (descrptionPanel != null)
        {
            FollowingDescription(descrptionPanel);
        }
        /*
        if (isButtonPressed == true && descrptionPanel != null && !wasButtonPressed)
        {
            DestroyDescription();
            GameObject.FindGameObjectWithTag("GameController").GetComponent<TutorialManager_CM>().CheckHighlightCount();
        }
        */

        wasButtonPressed = isButtonPressed;
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction);

        if (Physics.Raycast(ray, out RaycastHit rayHit, length))
        {
            if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("DescObj"))
            {
                if (objName != rayHit.collider.gameObject.name)
                {
                    //Debug.Log("Current objName : " + objName + " / RayHit ObjName : " + rayHit.collider.gameObject.name);

                    objName = rayHit.collider.gameObject.name;

                    var highlightEffect = rayHit.collider.gameObject.GetComponent<HighlightEffect>();
                    if (highlightEffect != null)
                    {
                        highlightEffect.highlighted = true;
                        rayHit.collider.gameObject.GetComponent<HighLightColorchange_CM>().GlowStart();
                    }

                    var highlightCount = rayHit.collider.gameObject.GetComponent<HighlighCount_CM>();
                    if (highlightCount != null)
                    {
                        highlightCount.ChangeFlag();
                    }

                    //InstantiatePanel(rayHit.collider.gameObject);
                    InstantiatePanel_Tween(rayHit.collider.gameObject.GetComponent<DescObjID_CM>().descPanel, rayHit.collider.gameObject);
                }
            }
        }
    }

    // NEW
    public void InstantiatePanel_Tween(GameObject panel, GameObject rayhit)
    {
        if (descrptionPanel != null) DestroyDescription();

        descrptionPanel = Instantiate(panel);
        descrptionPanel.transform.SetParent(descriptionPanelSpawnPoint);

        glowObj = rayhit;

        descrptionPanel.GetComponent<LaserDescriptionTween_CM>().HLObjInit(glowObj);
    }

    public void InstantiatePanel(GameObject go)
    {
        if (descrptionPanel != null) DestroyDescription();

        descrptionPanel = Instantiate(laserDescriptionCanvas);

        descrptionPanel.transform.position = descriptionPanelSpawnPoint.position;
        descrptionPanel.transform.rotation = Quaternion.identity;

        descrptionPanel.GetComponent<RectTransform>().localScale = new Vector3(0.0002f, 0.0002f, 0.0002f);

        MakeDescription(go);

        glowObj = go;
    }


    public void FollowingDescription(GameObject descPanel) // �г��� �÷��̾� �ü� ���󰡰� �ϱ�
    {
        if (descPanel.GetComponent<RectTransform>().localScale.x < 0.002f)
        {
            descPanel.GetComponent<RectTransform>().localScale =
            new Vector3(descPanel.GetComponent<RectTransform>().localScale.x + 0.0005f,
            descPanel.GetComponent<RectTransform>().localScale.y + 0.0005f,
            descPanel.GetComponent<RectTransform>().localScale.z + 0.0005f);
        }

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
        objName = "";
    }

    public void MakeDescription(GameObject go)
    {
        if (FireStoreManager_Test_CM.Instance.csvData.ContainsKey(go.name))
        {
            descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text =
            FireStoreManager_Test_CM.Instance.ReadCSV(go.name);

            descrptionPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text =
            FireStoreManager_Test_CM.Instance.ReadCSV(go.name + "_D");
        }

        objName = go.name;

        AudioMgr_CM.Instance.PlaySFXByInt(2);
    }

    public void RayStateChange(bool flag)
    {
        canMakeRay = flag;
        if (flag == false && line.enabled == true) line.enabled = false;
    }
}
