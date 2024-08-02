using Language.Lua;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;
using HighlightPlus;

public class LaserPointer_Lys : MonoBehaviour
{
    private BNG.UIPointer uiPointer; // 포인터 컴포넌트
    private bool isButtonPressed = false; // 오른손 A버튼이 눌리는지 안 눌리는지
    public float maxDistance;           // 레이저 최대 길이
    public GameObject touchEffect;

    public GameObject obj = null;
    private GameObject descPanel = null;
    private GameObject player = null;
    private Camera mainCam = null;
    private GameObject NPC = null;
    private int NPCLayer;
    private bool showLine = true;

    private GameObject glowObj;
    private HighlightEffect highlightEffect;

    UnityEngine.XR.InputDevice right; // 오른손 컨트롤러 상태를 받는 변수

    void Start()
    {
        uiPointer = gameObject.GetComponent<BNG.UIPointer>(); // UI 컴포넌트 받기
        descPanel = UIManager_Lys.instance.GetDesc();
        obj = null;
        player = GameManager_Lys.instance.GetPlayer();
        mainCam = GameManager_Lys.instance.GetPlayerCam().GetComponent<Camera>();
        NPC = GameManager_Lys.instance.GetNPC();
    }

    void Update()
    {
        // 각 bool값 변수들에 A버튼이 눌리는지 안 눌리는지 실시간으로 받기        
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);

        if (isButtonPressed && showLine) // 트리거가 눌리고 있다면
        {
            uiPointer.HidePointerIfNoObjectsFound = false; // 레이저 보이게 하기
            CheckRay(transform.position, transform.forward, maxDistance);
            if (GameManager_Lys.instance.GetMovable() && GameManager_Lys.instance.GetSelectable()) { CheckRay(transform.position, transform.forward, 10f); } // 현재 레이저에 맞은 오브젝트가 뭔지 검사하기
        }
        else { uiPointer.HidePointerIfNoObjectsFound = true; }

        if (UIManager_Lys.instance.CheckDesc()) // 현재 설명창이 만들어진 상태라면
        {
            if (!CheckSight()) { DestroyDescription(); }
        }
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction.normalized);

        if (Physics.Raycast(ray, out RaycastHit rayHit, length))
        {
            GameManager_Lys.instance.WaitForNewUI();
            Instantiate(touchEffect, rayHit.point, Quaternion.LookRotation(player.transform.position));
            Debug.Log(rayHit.collider.gameObject.name);
            if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (obj != rayHit.collider.gameObject)
                {
                    obj = rayHit.collider.gameObject;
                    highlightEffect = obj.gameObject.GetComponent<HighLightColorchange_Lys>().GetHl();
                    if (highlightEffect != null)
                    {
                        highlightEffect.highlighted = true;
                        rayHit.collider.gameObject.GetComponent<HighLightColorchange_Lys>().GlowStart();
                    }
                    InstantiatePanel(obj);
                }
            }
            else if (rayHit.collider.gameObject.CompareTag("NPC") && GameManager_Lys.instance.GetTalkable())
            {
                if (!GameManager_Lys.instance.GetTuto1()) { NPC.GetComponent<SelectDialogue_Lys>().ActivateDST1(); }
                else { NPC.GetComponent<SelectDialogue_Lys>().ActivateDST2(); }
            }
        }
    }

    public void InstantiatePanel(GameObject go)
    {
        UIManager_Lys.instance.OnDesc(go);
        glowObj = go;
    }

    public void DestroyDescription() // 패널 없애기
    {
        UIManager_Lys.instance.OffDesc();
    }
    public void InitObj()
    {
        if (glowObj != null)
        {
            glowObj.gameObject.GetComponent<HighLightColorchange_Lys>().GetHl().highlighted = false;
            glowObj.GetComponent<HighLightColorchange_Lys>().GlowEnd();
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
            else { return false; }
        }
    }

    public void HideLine() { showLine = false; }
}
