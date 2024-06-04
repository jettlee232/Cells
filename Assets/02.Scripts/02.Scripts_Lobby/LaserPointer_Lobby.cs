using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class LaserPointer_Lobby : MonoBehaviour
{
    public string objName = null; // 설명창이 가리키는 오브젝트의 이름

    public BNG.UIPointer uiPointer; // 포인터 컴포넌트
    public bool isTriggerPressed = false; // 트리거가 눌리는지 안 눌리는지
    public bool isButtonPressed = false; // 오른손 A버튼이 눌리는지 안 눌리는지

    private GameObject descPanel = null;
    private int descObjLayer;

    UnityEngine.XR.InputDevice right; // 오른손 컨트롤러 상태를 받는 변수

    void Start()
    {
        uiPointer = gameObject.GetComponent<BNG.UIPointer>(); // UI 컴포넌트 받기
        descPanel = UIManager_Lobby.instance.GetDesc();
        objName = null;
        descObjLayer = 1 << LayerMask.NameToLayer("DescObj");
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
            if (isButtonPressed) { DestroyDescription(); }
        }
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction.normalized);

        if (Physics.Raycast(ray, out RaycastHit rayHit, length, descObjLayer))
        {
            // 현재 설명창이 가리키는 오브젝트의 이름과 레이저가 맞은 오브젝트의 이름도 다르다면...
            // (설명창이 닫힌 상태라면 objName에는 아무것도 안 적혀있을거임)
            if (objName != rayHit.collider.gameObject.GetComponent<DescObj_Lobby>().GetName())
            {
                objName = rayHit.collider.gameObject.GetComponent<DescObj_Lobby>().GetName();
                InstantiatePanel(rayHit.collider.gameObject);
            }
        }
    }

    public void InstantiatePanel(GameObject go)
    {
        if (descPanel.activeSelf) { DestroyDescription(); }

        descPanel.SetActive(true);

        // 패널의 초기 크기는 작게 설정하기
        descPanel.GetComponent<RectTransform>().localScale = Vector3.one * 0.00125f;
        // 오브젝트 설명 띄우기
        MakeDescription(go);
    }


    public void FollowingDescription(GameObject descPanel) // 패널이 플레이어 시선 따라가게 하기
    {
        // 이 부분은 동적인 효과를 위해 넣은건데 사실 없어도 되긴 할듯하네요... 보고 필요 없다 싶으면 빼도 됩니다
        if (descPanel.GetComponent<RectTransform>().localScale.x < 0.00125f)
        {
            descPanel.GetComponent<RectTransform>().localScale =
            new Vector3(descPanel.GetComponent<RectTransform>().localScale.x + 0.00005f,
            descPanel.GetComponent<RectTransform>().localScale.y + 0.00005f,
            descPanel.GetComponent<RectTransform>().localScale.z + 0.00005f);
        }
    }

    public void DestroyDescription() // 패널 없애기
    {
        descPanel.SetActive(false);
        objName = null; // 현재 가리키는 오브젝트가 없음을 알리기 위해 objName을 비우기
    }

    public void MakeDescription(GameObject go) // 게임 오브젝트의 이름과 종류에 따라 설명창 텍스트를 수정하기
    {
        descPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = go.GetComponent<DescObj_Lobby>().GetName();
        descPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = go.GetComponent<DescObj_Lobby>().GetDesc();

        // 현재 패널이 가리키는 오브젝트의 이름을 저장
        objName = descPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text;
    }
}
