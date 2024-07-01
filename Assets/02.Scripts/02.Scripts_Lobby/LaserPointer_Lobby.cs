using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

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
    private bool isDialogue = false;

    UnityEngine.XR.InputDevice right; // 오른손 컨트롤러 상태를 받는 변수

    void Start()
    {
        uiPointer = gameObject.GetComponent<BNG.UIPointer>(); // UI 컴포넌트 받기
        obj = null;
        descObjLayer = 1 << LayerMask.NameToLayer("DescObj");
        player = GameManager_Lobby.instance.GetPlayer();
        mainCam = GameManager_Lobby.instance.GetPlayerCam().GetComponent<Camera>();
        NPC = GameManager_Lobby.instance.GetNPC();
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
        else { uiPointer.HidePointerIfNoObjectsFound = true; }

        if (obj != null && obj.gameObject.layer == LayerMask.NameToLayer("DescObj")) // 현재 설명창이 만들어진 상태라면
        {
            if (!CheckSight()) { DestroyDescription(); }
        }
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction.normalized);

        if (Physics.Raycast(ray, out RaycastHit rayHit, length))
        {
            if (rayHit.collider.gameObject.CompareTag("Interactable"))
            {
                if (obj != rayHit.collider.gameObject)
                {
                    obj = rayHit.collider.gameObject;
                    obj.transform.GetChild(0).GetComponent<TextObject_Lobby>().ShowText();
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
        }
    }

    public void DestroyDescription() // 3D Text 패널 없애기
    {
        if (obj == null) return;
        obj.transform.GetChild(0).GetComponent<TextObject_Lobby>().HideText();
        obj = null;
    }

    private bool CheckSight()
    {
        Vector3 viewportPos = Vector3.one;
        if (obj == null || mainCam == null || obj.transform == null) { DestroyDescription(); }
        else { viewportPos = mainCam.WorldToViewportPoint(obj.transform.position); }

        bool isInView = viewportPos.z > 0f && (viewportPos.x >= 0f && viewportPos.x <= 1f) && (viewportPos.y >= 0f && viewportPos.y <= 1f);

        return isInView;
    }
}
