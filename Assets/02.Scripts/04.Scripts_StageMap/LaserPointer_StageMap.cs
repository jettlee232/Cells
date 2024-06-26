using Language.Lua;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;
using HighlightPlus;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LaserPointer_StageMap : MonoBehaviour
{
    private BNG.UIPointer uiPointer; // 포인터 컴포넌트
    private bool isButtonPressed = false; // 오른손 A버튼이 눌리는지 안 눌리는지
    public float maxDistance;           // 레이저 최대 길이
    public GameObject touchEffect;

    private GameObject obj = null;
    private GameObject descPanel = null;
    private int descObjLayer;
    private int UILayer;
    private LayerMask playerLayer;
    private int playerLayer_1;
    private int playerLayer_2;
    private GameObject player = null;
    private Camera mainCam = null;
    private GameObject NPC = null;
    private bool outer = false;
    private int NPCLayer;

    private GameObject glowObj;
    private HighlightEffect highlightEffect;

    UnityEngine.XR.InputDevice right; // 오른손 컨트롤러 상태를 받는 변수

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
    }

    void Update()
    {
        if (!outer)
        {
            // 각 bool값 변수들에 A버튼이 눌리는지 안 눌리는지 실시간으로 받기        
            right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);

            if (isButtonPressed) // 트리거가 눌리고 있다면
            {
                uiPointer.HidePointerIfNoObjectsFound = false; // 레이저 보이게 하기
                if (GameManager_StageMap.instance.GetMovable() && GameManager_StageMap.instance.GetSelectable()) { CheckRay(transform.position, transform.forward, 10f); } // 현재 레이저에 맞은 오브젝트가 뭔지 검사하기
            }
            else { uiPointer.HidePointerIfNoObjectsFound = true; }
        }
        
        if (descPanel.activeSelf) // 현재 설명창이 만들어진 상태라면
        {
            if (!CheckSight()) { DestroyDescription(); }
        }
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction.normalized);

        if (Physics.Raycast(ray, out RaycastHit rayHit_NPC, length * 1.5f, NPCLayer))
        {
            Instantiate(touchEffect, rayHit_NPC.point, Quaternion.LookRotation(player.transform.position));
            Debug.Log(rayHit_NPC.collider.gameObject.name);
            rayHit_NPC.collider.gameObject.GetComponent<NPCController_StageMap>().SetNPCTalk();

            Vector3 dir = rayHit_NPC.transform.position - player.transform.position;
            dir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
            player.transform.rotation = rotation;

            DestroyDescription();
        }
        else
        {
            if (Physics.Raycast(ray, out RaycastHit rayHit, length, ~(UILayer | playerLayer)))
            {
                Instantiate(touchEffect, rayHit.point, Quaternion.LookRotation(player.transform.position));
                Debug.Log(rayHit.collider.gameObject.name);
                if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("DescObj"))
                {
                    if (obj != rayHit.collider.gameObject)
                    {
                        obj = rayHit.collider.gameObject;
                        highlightEffect = obj.transform.parent.GetComponent<HighlightEffect>();
                        if (highlightEffect != null)
                        {
                            highlightEffect.highlighted = true;
                            rayHit.collider.gameObject.GetComponent<HighLightColorchange_StageMap>().GlowStart();
                        }
                        InstantiatePanel(obj);
                    }
                }
                else if (rayHit.collider.gameObject.CompareTag("NPC"))
                {
                    if (GameManager_StageMap.instance.GetSecondCon())
                    {
                        // 두번째 대화 조건 만족
                        NPC.GetComponent<SelectDialogue_StageMap>().ActivateDST2();
                    }
                    else { NPC.GetComponent<SelectDialogue_StageMap>().ActivateDST1(); }
                }
            }
        }
    }

    public void InstantiatePanel(GameObject go)
    {
        UIManager_StageMap.instance.OnDesc(go);
        glowObj = go;
    }

    public void DestroyDescription() // 패널 없애기
    {
        GameManager_StageMap.instance.WaitForNewUI();
        UIManager_StageMap.instance.OffDesc();
        if (glowObj != null)
        {
            glowObj.transform.parent.GetComponent<HighlightEffect>().highlighted = false;
            glowObj.GetComponent<HighLightColorchange_StageMap>().GlowEnd();
        }
        obj = null;
        glowObj = null;
    }

    private bool CheckSight()
    {
        if (!outer)
        {
            if (obj == null || mainCam == null || obj.transform == null) { DestroyDescription(); return false; }
            else
            {
                Vector3 viewportPos = mainCam.WorldToViewportPoint(obj.transform.position);

                bool isInView = viewportPos.z > 0f && (viewportPos.x > 0f && viewportPos.x < 1f) && (viewportPos.y > 0f && viewportPos.y < 1f);

                Vector3 closest = obj.GetComponent<Collider>().ClosestPoint(player.transform.position);
                Vector3 vDistance = (closest - player.transform.position);
                bool isClose = (vDistance.magnitude <= maxDistance) ? true : false;

                if (isInView && isClose) { return true; }
                else { return false; }
            }
        }
        else { return true; }
    }

    // 외부에서 (OrganelleEnter_StageMap) 오브젝트를 넣을 때
    public void SetObj(GameObject obj_out)
    {
        // obj_out은 외부 obj라는 뜻이다ㅎㅎ
        if (obj != obj_out)
        {
            obj = obj_out;
            highlightEffect = obj.transform.parent.GetComponent<HighlightEffect>();
            if (highlightEffect != null)
            {
                highlightEffect.highlighted = true;
                obj.GetComponent<HighLightColorchange_StageMap>().GlowStart();
            }
            InstantiatePanel(obj);
        }
    }
    public void InObj() { outer = true; }
    public void OutObj() { outer = false; }
}
