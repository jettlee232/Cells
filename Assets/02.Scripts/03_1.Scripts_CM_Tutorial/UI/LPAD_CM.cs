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

    // Latley Update - 240724
    ParticleSystem particleSys;
    public GameObject particle;
    private bool wasBButtonPressed;
    private bool isSoundPlaying = false;

    UnityEngine.XR.InputDevice right;

    TutorialManager_CM tutoMgr;

    private bool canMakeRay = true;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        uiPointer = gameObject.GetComponent<BNG.UIPointer>();
        descrptionPanel = null;
        tutoMgr = GameObject.FindGameObjectWithTag("GameController").GetComponent<TutorialManager_CM>();

        // Latley Update - 240724
        GameObject go = Instantiate(particle);
        go.transform.position = descriptionPanelSpawnPoint[0].position;
        go.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        particleSys = go.GetComponent<ParticleSystem>();
        particleSys.Stop();
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

        // Latley Update - 240724
        wasBButtonPressed = isButtonPressed;

        if (isButtonPressed == false) isSoundPlaying = false;
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction);

        if (Physics.Raycast(ray, out RaycastHit rayHit, length))
        {
            DescObjID_CM descObj = rayHit.collider.gameObject.GetComponent<DescObjID_CM>();
            NarratorDialogueHub_CM_Tutorial npc = rayHit.collider.gameObject.GetComponent<NarratorDialogueHub_CM_Tutorial>();

            if (descObj != null)
            {
                HighlighCount_CM highlightCount = rayHit.collider.gameObject.GetComponent<HighlighCount_CM>();
                if (highlightCount != null)
                {
                    highlightCount.ChangeFlag();
                }


                if (currentPanel == null || currentPanel != descObj.GetComponent<DescObjID_CM>().descPanel)
                {
                    currentPanel = descObj.GetComponent<DescObjID_CM>().descPanel;

                    if (isSoundPlaying == false) InstantiatePanel_Tween(descObj.GetComponent<DescObjID_CM>().descPanel, descObj.gameObject);
                }
            }

            if (npc != null && tutoMgr.allComplete == true)
            {
                tutoMgr.allComplete = false;
                npc.StartCov_7();
                tutoMgr.AllCompleteAndMoveToNextScene();
            }
        }
    }

    // NEW
    public void InstantiatePanel_Tween(GameObject panel, GameObject rayhit)
    {
        isSoundPlaying = true;
        tutoMgr.followPanelParticleSys.Play();

        if (descrptionPanel != null)
        {
            glowobj.GetComponent<HighLightColorchange_CM>().GlowEnd();
            DestroyDescription();
            GameObject.FindGameObjectWithTag("GameController").GetComponent<TutorialManager_CM>().CheckHighlightCount();
        }

        tutoMgr.FollowDelete(0);

        descrptionPanel = Instantiate(panel);
        descrptionPanel.transform.SetParent(descriptionPanelSpawnPoint[rayhit.GetComponent<DescObjID_CM>().panelNum]);
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
