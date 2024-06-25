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
    private GameObject tutorial = null;
    private bool checkFly = false;

    private void Start()
    {
        player = GameManager_StageMap.instance.GetPlayer();
        tutorial = GameManager_StageMap.instance.GetTutorialManager();
        ActivateDST1();
    }

    public void ActivateDST1() // 1번째 트리거 작동 함수
    {
        if (GameManager_StageMap.instance.GetFirstEnd()) { dialogueSystemTrigger1.startConversationEntryID = 4; }
        else { DisableMove_SM(); dialogueSystemTrigger1.startConversationEntryID = 0; }
        dialogueSystemTrigger1.OnUse(); // On Use로 컨버제이션 작동
        GameManager_StageMap.instance.FirstEnd();
    }

    public void ActivateDST2() // 2번째 트리거 작동 함수
    {
        if (UIManager_StageMap.instance.GetUpsideSubtitle()) { UIManager_StageMap.instance.VanishUpsideSubtitle(); }
        dialogueSystemTrigger2.OnUse(); // On Use로 컨버제이션 작동
        GameManager_StageMap.instance.SecondEnd();
    }

    #region 스크립트에 쓰일 함수
    //public void fCheckFlyTutorial() { StartCoroutine(CheckFlyTutorial()); }
    public void CheckFlyTutorial_SM()
    {
        UIManager_StageMap.instance.SetUpsideSubtitle("비행을 하면서 3개의 타겟을 찾아보자!");
        tutorial.GetComponent<TutorialManager_StageMap>().StartTutorial();
    }

    public void EnableMove_SM() { GameManager_StageMap.instance.EnableMove(); Debug.Log("실행됨1"); }
    public void DisableMove_SM()
    {
        GameManager_StageMap.instance.DisableMove();
        GameManager_StageMap.instance.RemoveSelect();
    }
    public void EnableOrganelle_SM() { UIManager_StageMap.instance.EnanbleOrganelleButton(); Debug.Log("실행됨3"); }
    public void WaitForNewUI_SM() { GameManager_StageMap.instance.WaitForNewUI(); Debug.Log("실행됨2"); }
    public void Subtitle_Explore_SM() { StartCoroutine(cSubtitle_Explore_SM()); Debug.Log("실행됨4"); }
    IEnumerator cSubtitle_Explore_SM()
    {
        UIManager_StageMap.instance.SetUpsideSubtitle("동물세포를 탐험해 보자!");
        yield return new WaitForSeconds(5f);
        UIManager_StageMap.instance.VanishUpsideSubtitle();
    }
    #endregion

    #region Register with Lua

    private void OnEnable()
    {
        Lua.RegisterFunction("CheckFlyTutorial_SM", this, SymbolExtensions.GetMethodInfo(() => CheckFlyTutorial_SM()));
        Lua.RegisterFunction("EnableMove_SM", this, SymbolExtensions.GetMethodInfo(() => EnableMove_SM()));
        Lua.RegisterFunction("DisableMove_SM", this, SymbolExtensions.GetMethodInfo(() => DisableMove_SM()));
        Lua.RegisterFunction("EnableOrganelle_SM", this, SymbolExtensions.GetMethodInfo(() => EnableOrganelle_SM()));
        Lua.RegisterFunction("WaitForNewUI_SM", this, SymbolExtensions.GetMethodInfo(() => WaitForNewUI_SM()));
        Lua.RegisterFunction("Subtitle_Explore_SM", this, SymbolExtensions.GetMethodInfo(() => Subtitle_Explore_SM()));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("CheckFlyTutorial_SM");
        Lua.UnregisterFunction("EnableMove_SM");
        Lua.UnregisterFunction("DisableMove_SM");
        Lua.UnregisterFunction("EnableOrganelle_SM");
        Lua.UnregisterFunction("WaitForNewUI_SM");
        Lua.UnregisterFunction("Subtitle_Explore_SM");
    }

    #endregion
}
