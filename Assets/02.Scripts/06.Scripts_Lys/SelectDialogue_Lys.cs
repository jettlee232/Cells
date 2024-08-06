using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Unity.VisualScripting;
using UnityEngine.XR;
using UnityEngine.UIElements;

public class SelectDialogue_Lys : MonoBehaviour
{
    // 우선 아까 생성했던 다이얼로그 시스템 트리거들을 통제할 수 있는 변수 2개를 선언 
    public DialogueSystemTrigger dialogueSystemTrigger1; // 1번째 트리거
    public DialogueSystemTrigger dialogueSystemTrigger2; // 2번째 트리거
    public DialogueSystemTrigger dialogueSystemTrigger3; // 3번째 트리거
    public DialogueSystemTrigger dialogueSystemTrigger4; // 4번째 트리거

    public GameObject tuto;
    private TutorialManager_Lys tutorialManager;

    UnityEngine.XR.InputDevice right;

    public float length = 5f;
    private GameObject player = null;
    private GameObject tutorial = null;
    private bool checkFly = false;

    private void Start()
    {
        player = GameManager_Lys.instance.GetPlayer();
        tutorialManager = tuto.GetComponent<TutorialManager_Lys>();
        //tutorial = GameManager_Lys.instance.GetTutorialManager();
        //ActivateDST1();
    }

    public void ActivateDST1() // 1번째 트리거 작동 함수
    {
        //DisableMove_SM();
        //if (GameManager_Lys.instance.GetFirstEnd()) { dialogueSystemTrigger1.startConversationEntryID = 4; }
        //else { dialogueSystemTrigger1.startConversationEntryID = 0; }
        dialogueSystemTrigger1.startConversationEntryID = 0;
        dialogueSystemTrigger1.OnUse(); // On Use로 컨버제이션 작동
        GameManager_Lys.instance.Tuto1End();
    }

    public void ActivateDST2() // 2번째 트리거 작동 함수
    {
        //DisableMove_SM();
        //if (UIManager_Lys.instance.GetQuest()) { UIManager_Lys.instance.HideQuest(); }
        dialogueSystemTrigger2.OnUse(); // On Use로 컨버제이션 작동
    }

    public void ActivateDST3() // 3번째 트리거 작동 함수
    {
        dialogueSystemTrigger3.OnUse(); // On Use로 컨버제이션 작동
    }

    public void ActivateDST4() // 4번째 트리거 작동 함수
    {
        dialogueSystemTrigger4.OnUse(); // On Use로 컨버제이션 작동
    }

    public void GunTutorial_Lys() { tutorialManager.GunTutorial_Lys(); }
    public void RocketTutorial_Lys() { tutorialManager.RocketTutorial_Lys(); }
    public void InitPlayer_Lys() { GameManager_Lys.instance.MovePlayer(); UIManager_Lys.instance.HideQuest(); }
    public void ShowExample1_Lys() { tutorialManager.ShowExample1(); }
    public void ShowExample2_Lys() { tutorialManager.ShowExample2(); }
    public void ShowExample3_Lys() { tutorialManager.ShowExample3(); }
    public void HideExamples_Lys() { tutorialManager.InitTemp(); }

    public void HideUIs_Lys() { UIManager_Lys.instance.OffDesc(); }

    public void ClearTutorial_Lys() { GameManager_Lys.instance.MoveScene("06_Lys"); }

    public void ShowToolTip_Lys() { GameManager_Lys.instance.ShowShootToolTip(); }
    public void HideToolTip_Lys() { GameManager_Lys.instance.HideToolTip(); }

    public void ShowGun_Lys() { tutorialManager.ShowGun(); }
    public void HideGun_Lys() { tutorialManager.HideGun(); }
    public void ShowRocket_Lys() { tutorialManager.ShowRocket(); }
    public void HideRocket_Lys() { tutorialManager.HideRocket(); }
    public void DropWeapon_Lys() { tutorialManager.DropWeapon(); }

    #region 스크립트에 쓰일 함수

    #endregion

    #region Register with Lua

    private void OnEnable()
    {
        //Lua.RegisterFunction("CheckFlyTutorial_SM", this, SymbolExtensions.GetMethodInfo(() => CheckFlyTutorial_SM()));
        Lua.RegisterFunction("GunTutorial_Lys", this, SymbolExtensions.GetMethodInfo(() => GunTutorial_Lys()));
        Lua.RegisterFunction("RocketTutorial_Lys", this, SymbolExtensions.GetMethodInfo(() => RocketTutorial_Lys()));
        Lua.RegisterFunction("InitPlayer_Lys", this, SymbolExtensions.GetMethodInfo(() => InitPlayer_Lys()));
        Lua.RegisterFunction("ShowExample1_Lys", this, SymbolExtensions.GetMethodInfo(() => ShowExample1_Lys()));
        Lua.RegisterFunction("ShowExample2_Lys", this, SymbolExtensions.GetMethodInfo(() => ShowExample2_Lys()));
        Lua.RegisterFunction("ShowExample3_Lys", this, SymbolExtensions.GetMethodInfo(() => ShowExample3_Lys()));
        Lua.RegisterFunction("HideExamples_Lys", this, SymbolExtensions.GetMethodInfo(() => HideExamples_Lys()));
        Lua.RegisterFunction("HideUIs_Lys", this, SymbolExtensions.GetMethodInfo(() => HideUIs_Lys()));
        Lua.RegisterFunction("ClearTutorial_Lys", this, SymbolExtensions.GetMethodInfo(() => ClearTutorial_Lys()));
        Lua.RegisterFunction("ShowToolTip_Lys", this, SymbolExtensions.GetMethodInfo(() => ShowToolTip_Lys()));
        Lua.RegisterFunction("HideToolTip_Lys", this, SymbolExtensions.GetMethodInfo(() => HideToolTip_Lys()));
        Lua.RegisterFunction("ShowGun_Lys", this, SymbolExtensions.GetMethodInfo(() => ShowGun_Lys()));
        Lua.RegisterFunction("HideGun_Lys", this, SymbolExtensions.GetMethodInfo(() => HideGun_Lys()));
        Lua.RegisterFunction("ShowRocket_Lys", this, SymbolExtensions.GetMethodInfo(() => ShowRocket_Lys()));
        Lua.RegisterFunction("HideRocket_Lys", this, SymbolExtensions.GetMethodInfo(() => HideRocket_Lys()));
        Lua.RegisterFunction("DropWeapon_Lys", this, SymbolExtensions.GetMethodInfo(() => DropWeapon_Lys()));
        // 예시용
    }

    private void OnDisable()
    {
        //Lua.UnregisterFunction("CheckFlyTutorial_SM");
        Lua.UnregisterFunction("GunTutorial_Lys");
        Lua.UnregisterFunction("RocketTutorial_Lys");
        Lua.UnregisterFunction("InitPlayer_Lys");
        Lua.UnregisterFunction("ShowExample1_Lys");
        Lua.UnregisterFunction("ShowExample2_Lys");
        Lua.UnregisterFunction("ShowExample3_Lys");
        Lua.UnregisterFunction("HideExamples_Lys");
        Lua.UnregisterFunction("HideUIs_Lys");
        Lua.UnregisterFunction("ClearTutorial_Lys");
        Lua.UnregisterFunction("ShowToolTip_Lys");
        Lua.UnregisterFunction("HideToolTip_Lys");
        Lua.UnregisterFunction("ShowGun_Lys");
        Lua.UnregisterFunction("HideGun_Lys");
        Lua.UnregisterFunction("ShowRocket_Lys");
        Lua.UnregisterFunction("HideRocket_Lys");
        Lua.UnregisterFunction("DropWeapon_Lys");
        // 예시용
    }

    #endregion
}
