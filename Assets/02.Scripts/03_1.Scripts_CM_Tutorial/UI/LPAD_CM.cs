using DG.DemiLib;
using HighlightPlus;
using PixelCrushers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class LPAD_CM : MonoBehaviour
{
    public GameObject descrptionPanel = null;
    public Transform[] descriptionPanelSpawnPoint;

    public LineRenderer line;
    public BNG.UIPointer uiPointer;
    public bool isButtonPressed = false;

    // Latley Update - 240701 pm 0118
    public GameObject currentPanel;
    public GameObject glowobj;

    UnityEngine.XR.InputDevice right;

    private bool canMakeRay = true;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        uiPointer = gameObject.GetComponent<BNG.UIPointer>();
        descrptionPanel = null;
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
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction);

        if (Physics.Raycast(ray, out RaycastHit rayHit, length))
        {
            DescObjID_CM descObj = rayHit.collider.gameObject.GetComponent<DescObjID_CM>();

            if (descObj != null)
            {
                HighlighCount_CM highlightCount = rayHit.collider.gameObject.GetComponent<HighlighCount_CM>();
                if (highlightCount != null)
                {
                    highlightCount.ChangeFlag();
                }

                if (currentPanel != descObj.GetComponent<DescObjID_CM>().descPanel)
                {
                    currentPanel = descObj.GetComponent<DescObjID_CM>().descPanel;

                    InstantiatePanel_Tween(descObj.GetComponent<DescObjID_CM>().descPanel, descObj.gameObject);
                }
            }
        }
    }

    // NEW
    public void InstantiatePanel_Tween(GameObject panel, GameObject rayhit)
    {
        if (descrptionPanel != null)
        {
            glowobj.GetComponent<HighLightColorchange_CM>().GlowEnd();
            DestroyDescription();
        }

        descrptionPanel = Instantiate(panel);
        descrptionPanel.transform.SetParent(descriptionPanelSpawnPoint[rayhit.GetComponent<DescObjID_CM>().panelNum]);
        Debug.Log(rayhit.GetComponent<DescObjID_CM>().panelNum);
        Debug.Log(descriptionPanelSpawnPoint[rayhit.GetComponent<DescObjID_CM>().panelNum]);
        descrptionPanel.GetComponent<LaserDescriptionTween_CM>().HLObjInit(rayhit);

        glowobj = rayhit;
    }


    public void DestroyDescription()
    {
        Destroy(descrptionPanel);
    }

    public void RayStateChange(bool flag)
    {
        canMakeRay = flag;
    }
}
