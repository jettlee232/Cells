using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class OrganelleEnter_StageMap : MonoBehaviour
{
    public GameObject pointer;
    private BNG.UIPointer uiPointer; // 포인터 컴포넌트

    public bool isTriggerPressed = false; // 트리거가 눌리는지 안 눌리는지
    public bool isButtonPressed = false; // 오른손 A버튼이 눌리는지 안 눌리는지
    private bool enter = false;

    UnityEngine.XR.InputDevice right; // 오른손 컨트롤러 상태를 받는 변수

    private void Start()
    {
        uiPointer = pointer.GetComponent<BNG.UIPointer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 여기서 클릭을 하면 레이저포인터의 SetObj(this);를 불러온다
        enter = true;
        StartCoroutine(CheckClick());
        pointer.GetComponent<LaserPointer_StageMap>().InObj();
    }

    private void OnTriggerExit(Collider other)
    {
        enter = false;
        pointer.GetComponent<LaserPointer_StageMap>().OutObj();
    }

    IEnumerator CheckClick()
    {
        while (true)
        {
            if (!enter) break;
            right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            right.TryGetFeatureValue(CommonUsages.triggerButton, out isTriggerPressed);
            right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);

            if (isTriggerPressed) // 트리거가 눌리고 있다면
            {
                uiPointer.HidePointerIfNoObjectsFound = false; // 레이저 보이게 하기
                if (GameManager_StageMap.instance.GetMovable() && GameManager_StageMap.instance.GetSelectable()) { pointer.GetComponent<LaserPointer_StageMap>().SetObj(this.gameObject); }
            }
            else { uiPointer.HidePointerIfNoObjectsFound = true; }
            yield return null;
        }
    }
}