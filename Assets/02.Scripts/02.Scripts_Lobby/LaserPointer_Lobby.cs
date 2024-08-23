using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using static UnityEngine.ParticleSystem;

public class LaserPointer_Lobby : MonoBehaviour
{
    private BNG.UIPointer uiPointer; // 포인터 컴포넌트
    private bool isButtonPressed = false; // 오른손 A버튼이 눌리는지 안 눌리는지
    public float maxDistance;

    private GameObject obj = null;
    private int descObjLayer;
    private GameObject player = null;
    private Camera mainCam = null;
    private GameObject NPC = null;
    private GameObject InteractableManager;

    UnityEngine.XR.InputDevice right; // 오른손 컨트롤러 상태를 받는 변수

    // SYS Code
    public GameObject descrptionPanel = null;
    public Transform[] descriptionPanelSpawnPoint;
    public GameObject currentPanel;
    public GameObject glowobj;

    // SYS Code
    [Header("Particle")]
    public ParticleSystem handPanelParticle;
    private bool wasBButtonPressed;    

    private bool isSoundPlaying = false;

    void Start()
    {
        uiPointer = gameObject.GetComponent<BNG.UIPointer>(); // UI 컴포넌트 받기
        obj = null;
        descObjLayer = 1 << LayerMask.NameToLayer("DescObj");
        player = GameManager_Lobby.instance.GetPlayer();
        mainCam = GameManager_Lobby.instance.GetPlayerCam().GetComponent<Camera>();
        NPC = GameManager_Lobby.instance.GetNPC();
        InteractableManager = GameManager_Lobby.instance.GetInteractable();

        // SYS Code        
        handPanelParticle.Stop();
    }

    void Update()
    {
        // 각 bool값 변수들에 트리거 버튼과 A버튼이 눌리는지 안 눌리는지 실시간으로 받기        
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);

        if (isButtonPressed) // 버튼이 눌리고 있다면
        {
            uiPointer.HidePointerIfNoObjectsFound = false; // 레이저 보이게 하기
            CheckRay(transform.position, transform.forward, maxDistance); // 현재 레이저에 맞은 오브젝트가 뭔지 검사하기
        }
        else
        {
            uiPointer.HidePointerIfNoObjectsFound = true;
            //InteractableManager.GetComponent<InteractableManager_Lobby>().HideTextAll();
        }

        // Latley Update - 240724
        wasBButtonPressed = isButtonPressed;

        if (isButtonPressed == false) isSoundPlaying = false;
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction.normalized);
        obj = null;

        if (Physics.Raycast(ray, out RaycastHit rayHit, length))
        {
            /*
            if (rayHit.collider.gameObject.CompareTag("Interactable"))
            {
                if (obj != rayHit.collider.gameObject)
                {
                    obj = rayHit.collider.gameObject;
                    InteractableManager.GetComponent<InteractableManager_Lobby>().HideTextExclude(obj);
                }
            }
            else if (rayHit.collider.gameObject.CompareTag("NPC"))
            {
                if (GameManager_Lobby.instance.firstEnd)
                {
                    if (GameManager_Lobby.instance.secondCon)
                    {
                        // 두번째 대화 조건 만족
                        NPC.GetComponent<SelectDialogue_Lobby>().ActivateDST2();
                    }
                    else { return; }
                }
                else { NPC.GetComponent<SelectDialogue_Lobby>().ActivateDST1(); }
            }
            */
            if (rayHit.collider.gameObject.CompareTag("NPC"))
            {
                if (GameManager_Lobby.instance.firstEnd)
                {
                    // SYS Code
                    //if (GameManager_Lobby.instance.secondCon && GameManager_Lobby.instance.tutoStatus == 1)
                    if (GameManager_Lobby.instance.tutoStatus == 1)
                    {
                        // 두번째 대화 조건 만족                        
                        NPC.GetComponent<SelectDialogue_Lobby>().ActivateDST2();
                    }
                }
                else
                {
                    if (GameManager_Lobby.instance.tutoStatus == 0) NPC.GetComponent<SelectDialogue_Lobby>().ActivateDST1();
                }
            }

            // SYS Code
            DescObjID_CM descObj = rayHit.collider.gameObject.GetComponent<DescObjID_CM>();
            if (descObj != null)
            {
                if (isSoundPlaying == false)
                {
                    InstantiatePanel(descObj.GetComponent<DescObjID_CM>().descPanel);
                }
            }
        }
    }

    // SYS Code
    public void InstantiatePanel(GameObject panel)
    {
        isSoundPlaying = true;
        handPanelParticle.Play();

        if (descrptionPanel != null)
        {
            DestroyDescription();
        }

        descrptionPanel = Instantiate(panel);
        descrptionPanel.transform.SetParent(descriptionPanelSpawnPoint[0]);

        // Latley Update - 240724
        
        /*
        if (particleFlag == 1)
        {
            paricle.Stop();
            paricle.Play();
            particleFlag = 2;
        }
        if (particleFlag == 2 && paricle.isPlaying == false)
        {
            paricle.Stop();
            paricle.Play();
        }
        */
    }

    // SYS Code
    public void InstantiatePanel_Tween(GameObject rayhit)
    {
        if (descrptionPanel != null)
        {
            //glowobj.GetComponent<HighLightColorchange_CM>().GlowEnd();
            DestroyDescription();
        }

        descrptionPanel = Instantiate(rayhit);
        descrptionPanel.transform.SetParent(descriptionPanelSpawnPoint[rayhit.GetComponent<DescObjID_CM>().panelNum]);
        /*
        descrptionPanel.GetComponent<LaserDescriptionTween_CM>().HLObjInit(rayhit);

        glowobj = rayhit;
        glowobj.GetComponent<LaserDescriptionTween_CM>().HLObjInit(rayhit);
        */
    }

    // SYS Code
    public void DestroyDescription()
    {
        Destroy(descrptionPanel);
    }
}
