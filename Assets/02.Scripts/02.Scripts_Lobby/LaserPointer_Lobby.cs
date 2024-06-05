using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class LaserPointer_Lobby : MonoBehaviour
{
    public BNG.UIPointer uiPointer; // 포인터 컴포넌트
    public bool isTriggerPressed = false; // 트리거가 눌리는지 안 눌리는지
    public bool isButtonPressed = false; // 오른손 A버튼이 눌리는지 안 눌리는지
    public float maxDistance;

    public GameObject obj = null;
    private GameObject descPanel = null;
    private int descObjLayer;
    private GameObject player = null;
    private Camera mainCam = null;

    UnityEngine.XR.InputDevice right; // 오른손 컨트롤러 상태를 받는 변수

    void Start()
    {
        uiPointer = gameObject.GetComponent<BNG.UIPointer>(); // UI 컴포넌트 받기
        descPanel = UIManager_Lobby.instance.GetDesc();
        obj = null;
        descObjLayer = 1 << LayerMask.NameToLayer("DescObj");
        player = GameManager_Lobby.instance.GetPlayer();
        mainCam = GameManager_Lobby.instance.GetPlayerCam().GetComponent<Camera>();
    }

    void Update()
    {
        // 각 bool값 변수들에 트리거 버튼과 A버튼이 눌리는지 안 눌리는지 실시간으로 받기        
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.triggerButton, out isTriggerPressed);
        right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);

        if (isTriggerPressed) // 트리거가 눌리고 있다면
        {
            uiPointer.HidePointerIfNoObjectsFound = false; // 레이저 보이게 하기
            CheckRay(transform.position, transform.forward, 10f); // 현재 레이저에 맞은 오브젝트가 뭔지 검사하기
        }
        else { uiPointer.HidePointerIfNoObjectsFound = true; }

        if (descPanel.activeSelf) // 현재 설명창이 만들어진 상태라면
        {
            FollowingDescription(UIManager_Lobby.instance.GetDesc()); // 현재 만들어진 설명창이 내 시선을 따라오게 하기
            if (isButtonPressed || !CheckSight()) { DestroyDescription(); }
        }
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction.normalized);

        if (Physics.Raycast(ray, out RaycastHit rayHit, length, descObjLayer))
        {
            if (obj != rayHit.collider.gameObject)
            {
                obj = rayHit.collider.gameObject;
                InstantiatePanel(obj);
            }
        }
    }

    public void InstantiatePanel(GameObject go)
    {
        if (descPanel.activeSelf) { DestroyDescription(); }
        
        descPanel.SetActive(true);
        descPanel.GetComponent<RectTransform>().localScale = Vector3.one * 0.00125f;
        MakeDescription(go);
    }


    public void FollowingDescription(GameObject descPanel) // 패널이 플레이어 시선 따라가게 하기
    {
        if (descPanel.GetComponent<RectTransform>().localScale.x < 0.00125f)
        {
            descPanel.GetComponent<RectTransform>().localScale =
            new Vector3(descPanel.GetComponent<RectTransform>().localScale.x + 0.00001f,
            descPanel.GetComponent<RectTransform>().localScale.y + 0.00001f,
            descPanel.GetComponent<RectTransform>().localScale.z + 0.00001f);
        }
    }

    public void DestroyDescription() // 패널 없애기
    {
        descPanel.SetActive(false);
        obj = null;
    }

    public void MakeDescription(GameObject go) // 게임 오브젝트의 이름과 종류에 따라 설명창 텍스트를 수정하기
    {
        descPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = go.GetComponent<DescObj_Lobby>().GetName();
        descPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = go.GetComponent<DescObj_Lobby>().GetDesc();
    }

    private bool CheckSight()
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
