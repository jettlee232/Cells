using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Unity.VisualScripting;
using UnityEngine.XR;
using UnityEngine.UIElements;

public class SelectDialogue_StageMap : MonoBehaviour
{
    // 우선 아까 생성했던 다이얼로그 시스템 트리거들을 통제할 수 있는 변수 2개를 선언 
    public DialogueSystemTrigger dialogueSystemTrigger1; // 1번째 트리거
    public DialogueSystemTrigger dialogueSystemTrigger2; // 2번째 트리거

    UnityEngine.XR.InputDevice right;

    public float length = 5f;
    private GameObject player = null;
    private bool checkFly = false;

    private void Start()
    {
        player = GameManager_StageMap.instance.GetPlayer();
        ActivateDST1();
    }

    public void ActivateDST1() // 1번째 트리거 작동 함수
    {
        if (GameManager_StageMap.instance.GetFirstEnd()) { dialogueSystemTrigger1.startConversationEntryID = 4; }
        else { dialogueSystemTrigger1.startConversationEntryID = 0; }
        dialogueSystemTrigger1.OnUse(); // On Use로 컨버제이션 작동
        DisableMove();
        GameManager_StageMap.instance.FirstEnd();
    }

    public void ActivateDST2() // 2번째 트리거 작동 함수
    {
        dialogueSystemTrigger2.OnUse(); // On Use로 컨버제이션 작동
        DisableMove();
        GameManager_StageMap.instance.secondEnd = true;
    }

    #region 스크립트에 쓰일 함수
    public void fCheckFlyTutorial() { StartCoroutine(CheckFlyTutorial()); }
    IEnumerator CheckFlyTutorial()
    {
        while (true)
        {
            right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            right.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out checkFly);

            if (checkFly) { break; }
            yield return new WaitForSeconds(0.02f);
        }
        GameManager_StageMap.instance.secondCon = true;
    }

    public void EnableMove() { GameManager_StageMap.instance.EnableMove(); }
    public void DisableMove()
    {
        GameManager_StageMap.instance.DisableMove();
        GameManager_StageMap.instance.StopPlayer();
        GameManager_StageMap.instance.RemoveSelect();
    }
    public void EnableOrganelle() { UIManager_StageMap.instance.EnableButton(); }
    #endregion

    #region Register with Lua

    private void OnEnable()
    {
        Lua.RegisterFunction("fCheckFlyTutorial", this, SymbolExtensions.GetMethodInfo(() => fCheckFlyTutorial()));
        Lua.RegisterFunction("EnableMove", this, SymbolExtensions.GetMethodInfo(() => EnableMove()));
        Lua.RegisterFunction("EnableOrganelle", this, SymbolExtensions.GetMethodInfo(() => EnableOrganelle()));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("fCheckFlyTutorial");
        Lua.UnregisterFunction("EnableMove");
        Lua.UnregisterFunction("EnableOrganelle");
    }

    #endregion
}
