using Language.Lua;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;
using HighlightPlus;
using DG.Tweening;

public class LaserPointer_StageMap : MonoBehaviour
{
    private BNG.UIPointer uiPointer; // 포인터 컴포넌트
    private bool isButtonPressed = false; // 오른손 A버튼이 눌리는지 안 눌리는지
    public float maxDistance;           // 레이저 최대 길이
    public GameObject touchEffect;

    public GameObject obj = null;
    private GameObject descPanel = null;
    private int descObjLayer;
    private int UILayer;
    private LayerMask playerLayer;
    private int playerLayer_1;
    private int playerLayer_2;
    private GameObject player = null;
    private Camera mainCam = null;
    private GameObject NPC = null;
    private int NPCLayer;

    private GameObject glowObj;
    private HighlightEffect highlightEffect;

    UnityEngine.XR.InputDevice right; // 오른손 컨트롤러 상태를 받는 변수

    // SYS Code
    [Header("Particle")]
    private bool isSoundPlaying = false;
    public ParticleSystem handPanelParticle;
    public ParticleSystem watchHandPanelParticle;

    [Header("Explain Canvas Tween")]
    public Transform explainCanvas;
    public Transform firstPos;
    public Transform lastPos;
    private Tween moveTween;

    void Start()
    {
        uiPointer = gameObject.GetComponent<BNG.UIPointer>(); // UI 컴포넌트 받기
        descPanel = UIManager_StageMap.instance.GetDesc();
        obj = null;
        descObjLayer = 1 << LayerMask.NameToLayer("DescObj");
        NPCLayer = 1 << LayerMask.NameToLayer("NPC");
        UILayer = 1 << LayerMask.NameToLayer("UI");
        playerLayer_1 = 1 << LayerMask.NameToLayer("Player");
        playerLayer_2 = 2 << LayerMask.NameToLayer("Hand");
        playerLayer = (playerLayer_1 | playerLayer_2);
        player = GameManager_StageMap.instance.GetPlayer();
        mainCam = GameManager_StageMap.instance.GetPlayerCam().GetComponent<Camera>();
        NPC = GameManager_StageMap.instance.GetNPC();

        // SYS Code
        handPanelParticle.Stop();
        watchHandPanelParticle.Stop();
    }

    void Update()
    {
        // 각 bool값 변수들에 A버튼이 눌리는지 안 눌리는지 실시간으로 받기        
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);

        if (isButtonPressed) // 트리거가 눌리고 있다면
        {
            uiPointer.HidePointerIfNoObjectsFound = false; // 레이저 보이게 하기
            CheckRay(transform.position, transform.forward, maxDistance);
            if (GameManager_StageMap.instance.GetMovable() && GameManager_StageMap.instance.GetSelectable()) { CheckRay(transform.position, transform.forward, 10f); } // 현재 레이저에 맞은 오브젝트가 뭔지 검사하기
        }
        else { uiPointer.HidePointerIfNoObjectsFound = true; isSoundPlaying = false; }

        /*
        if (UIManager_StageMap.instance.CheckDesc()) // 현재 설명창이 만들어진 상태라면
        {
            if (!CheckSight()) { DestroyDescription(); }
        }
        */
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction.normalized);

        //if (Physics.Raycast(ray, out RaycastHit rayHit_NPC, length * 1.5f, NPCLayer))
        //{
        //    GameManager_StageMap.instance.WaitForNewUI();
        //    Instantiate(touchEffect, rayHit_NPC.point, Quaternion.LookRotation(player.transform.position));
        //    Debug.Log(rayHit_NPC.collider.gameObject.name);
        //    rayHit_NPC.collider.gameObject.GetComponent<NPCController_StageMap>().SetNPCTalk();

        //    Vector3 dir = rayHit_NPC.transform.position - player.transform.position;
        //    dir.y = 0;
        //    Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
        //    player.transform.rotation = rotation;

        //    DestroyDescription();
        //}
        //else
        //{
        if (Physics.Raycast(ray, out RaycastHit rayHit, length, ~(UILayer | playerLayer)))
        {
            GameManager_StageMap.instance.WaitForNewUI();
            Instantiate(touchEffect, rayHit.point, Quaternion.LookRotation(player.transform.position));
            Debug.Log(rayHit.collider.gameObject.name);
            if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Organelle"))
            {
                if (obj != rayHit.collider.gameObject)
                {
                    obj = rayHit.collider.gameObject;
                    highlightEffect = obj.gameObject.GetComponent<HighLightColorchange_StageMap>().GetHl();
                    if (highlightEffect != null)
                    {
                        highlightEffect.highlighted = true;
                        rayHit.collider.gameObject.GetComponent<HighLightColorchange_StageMap>().GlowStart();
                    }
                    if (isSoundPlaying == false) InstantiatePanel(obj);
                }
            }
            else if (rayHit.collider.gameObject.CompareTag("NPC"))
            {
                // SYS Code
                //if (GameManager_StageMap.instance.GetSecondCon())
                if (GameManager_StageMap.instance.ReturnTutoEnd() == true)
                {
                    // 두번째 대화 조건 만족
                    NPC.GetComponent<SelectDialogue_StageMap>().ActivateDST2();
                }
                // SYS Code
                //else { NPC.GetComponent<SelectDialogue_StageMap>().ActivateDST1(); }
            }
        }
        //}
    }

    public void InstantiatePanel(GameObject go)
    {
        isSoundPlaying = true;
        handPanelParticle.Play();
        watchHandPanelParticle.Play();

        // SYS Code - Explain Canvas Move Tween
        //explainCanvas.position = firstPos.position;
        //moveTween = explainCanvas.DOLocalMove(lastPos.position, 1f).OnComplete(KillMoveTween);

        UIManager_StageMap.instance.OnDesc(go);
        glowObj = go;
        AudioMgr_CM.Instance.PlaySFXByInt(4); // SSS
    }

    // SYS Code
    void KillMoveTween() { moveTween.Kill(); moveTween = null; }

    public void DestroyDescription() // 패널 없애기
    {
        //KillMoveTween();
        watchHandPanelParticle.Stop();
        UIManager_StageMap.instance.OffDesc();
        AudioMgr_CM.Instance.PlaySFXByInt(16); // SSS
    }
    public void InitObj()
    {
        if (glowObj != null)
        {
            glowObj.gameObject.GetComponent<HighLightColorchange_StageMap>().GetHl().highlighted = false;
            glowObj.GetComponent<HighLightColorchange_StageMap>().GlowEnd();
        }
        obj = null;
        glowObj = null;
    }

    private bool CheckSight()
    {
        if (obj == null || mainCam == null || obj.transform == null) { DestroyDescription(); return false; }
        else
        {
            Vector3 viewportPos = mainCam.WorldToViewportPoint(obj.transform.position);

            bool isInView = viewportPos.z > 0f && (viewportPos.x > 0f && viewportPos.x < 1f) && (viewportPos.y > 0f && viewportPos.y < 1f);

            if (isInView) { return true; }
            else { Debug.Log(viewportPos.ToString()); return false; }
        }
    }
}
