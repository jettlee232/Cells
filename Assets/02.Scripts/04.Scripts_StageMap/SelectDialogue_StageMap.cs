using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Unity.VisualScripting;
using UnityEngine.XR;
using UnityEngine.UIElements;
using static Oculus.Interaction.OptionalAttribute;
using System;

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
        DisableMove_SM();
        if (GameManager_StageMap.instance.GetFirstEnd()) { dialogueSystemTrigger1.startConversationEntryID = 4; }
        else { dialogueSystemTrigger1.startConversationEntryID = 0; }
        dialogueSystemTrigger1.OnUse(); // On Use로 컨버제이션 작동
        GameManager_StageMap.instance.FirstEnd();
    }

    public void ActivateDST2() // 2번째 트리거 작동 함수
    {
        DisableMove_SM();
        if (UIManager_StageMap.instance.GetQuest()) { UIManager_StageMap.instance.HideQuest(); }
        dialogueSystemTrigger2.OnUse(); // On Use로 컨버제이션 작동
        GameManager_StageMap.instance.SecondEnd();
    }

    #region 스크립트에 쓰일 함수
    public void CheckFlyTutorial_SM()
    {
        //UIManager_StageMap.instance.SetQuest("비행을 하면서 3개의 타겟을 찾아보자!");       
        tutorial.GetComponent<TutorialManager_StageMap>().TooltipSpriteChange(0, 1);

        tutorial.GetComponent<TutorialManager_StageMap>().NewTooltip(0, "비행을 하면서 3개의 타겟을 찾아보자!");
        tutorial.GetComponent<TutorialManager_StageMap>().UnShowingTooltipAnims(0);
        tutorial.GetComponent<TutorialManager_StageMap>().UnShowingTooltipAnims(1);
        tutorial.GetComponent<TutorialManager_StageMap>().StartTutorial();

        
    }

    public void EnableMove_SM() { GameManager_StageMap.instance.EnableMove(); }
    public void DisableMove_SM()
    {
        GameManager_StageMap.instance.DisableMove();
        GameManager_StageMap.instance.RemoveSelect();
    }
    public void EnableOrganelle_SM() { UIManager_StageMap.instance.EnanbleOrganelleButton(); }
    public void Subtitle_Explore_SM() { UIManager_StageMap.instance.SetQuest("동물세포를 탐험해 보자!", 5f); }
    public void ShowTutorial_SM() { UIManager_StageMap.instance.ShowTutorial(); }
    public void ShowOrganelle_SM() { UIManager_StageMap.instance.ShowOrganelleUI(); }
    public void HideOrganelle_SM() { UIManager_StageMap.instance.HideOrganelleUI(); }

    // SYS Code
    public void NewTooltip_SM(double index, string content) { tutorial.GetComponent<TutorialManager_StageMap>().NewTooltip((int)index, content); }
    public void TooltipOver_SM(double index) { tutorial.GetComponent<TutorialManager_StageMap>().TooltipOver((int)index); }

    // SYS Code
    public void LaserPointerONOFF_SM(bool flag) { tutorial.GetComponent<TutorialManager_StageMap>().LaserPointerONOFF(flag); }

    // SYS Code
    public void ShowingTooltipAnims_SM(double hand, double index) { tutorial.GetComponent<TutorialManager_StageMap>().ShowingTooltipAnims((int)hand, (int)index); }
    public void UnShowingTooltipAnims_SM(double hand) { tutorial.GetComponent<TutorialManager_StageMap>().UnShowingTooltipAnims((int)hand); }

    // SYS Code
    public void ChangeTutorialStatus_SM(double status) { GameManager_StageMap.instance.ChangeTutorialStatus((int)status); }
    public void StartAdventure_SM(bool flag) 
    { 
        if (flag == true)
        {
            tutorial.GetComponent<TutorialManager_StageMap>().tooltips[0].TooltipOff();
            GameManager_StageMap.instance.StartAdventure();
        }
        else
        {

        }
    }


    #endregion

    #region Register with Lua

    private void OnEnable()
    {
        Lua.RegisterFunction("CheckFlyTutorial_SM", this, SymbolExtensions.GetMethodInfo(() => CheckFlyTutorial_SM()));
        Lua.RegisterFunction("EnableMove_SM", this, SymbolExtensions.GetMethodInfo(() => EnableMove_SM()));
        Lua.RegisterFunction("DisableMove_SM", this, SymbolExtensions.GetMethodInfo(() => DisableMove_SM()));
        Lua.RegisterFunction("EnableOrganelle_SM", this, SymbolExtensions.GetMethodInfo(() => EnableOrganelle_SM()));
        Lua.RegisterFunction("Subtitle_Explore_SM", this, SymbolExtensions.GetMethodInfo(() => Subtitle_Explore_SM()));
        Lua.RegisterFunction("ShowTutorial_SM", this, SymbolExtensions.GetMethodInfo(() => ShowTutorial_SM()));
        Lua.RegisterFunction("ShowOrganelle_SM", this, SymbolExtensions.GetMethodInfo(() => ShowOrganelle_SM()));
        Lua.RegisterFunction("HideOrganelle_SM", this, SymbolExtensions.GetMethodInfo(() => HideOrganelle_SM()));

        // SYS Code
        Lua.RegisterFunction("NewTooltip_SM", this, SymbolExtensions.GetMethodInfo(() => NewTooltip_SM((double)0, (string)null)));
        Lua.RegisterFunction("TooltipOver_SM", this, SymbolExtensions.GetMethodInfo(() => TooltipOver_SM((double)0)));
        Lua.RegisterFunction("LaserPointerONOFF_SM", this, SymbolExtensions.GetMethodInfo(() => LaserPointerONOFF_SM((bool)false)));
        Lua.RegisterFunction("ShowingTooltipAnims_SM", this, SymbolExtensions.GetMethodInfo(() => ShowingTooltipAnims_SM((double)0, (double)0)));
        Lua.RegisterFunction("UnShowingTooltipAnims_SM", this, SymbolExtensions.GetMethodInfo(() => UnShowingTooltipAnims_SM((double)0)));
        Lua.RegisterFunction("ChangeTutorialStatus_SM", this, SymbolExtensions.GetMethodInfo(() => ChangeTutorialStatus_SM((double)0)));
        Lua.RegisterFunction("StartAdventure_SM", this, SymbolExtensions.GetMethodInfo(() => StartAdventure_SM((bool)false)));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("CheckFlyTutorial_SM");
        Lua.UnregisterFunction("EnableMove_SM");
        Lua.UnregisterFunction("DisableMove_SM");
        Lua.UnregisterFunction("EnableOrganelle_SM");
        Lua.UnregisterFunction("Subtitle_Explore_SM");
        Lua.UnregisterFunction("ShowTutorial_SM");
        Lua.UnregisterFunction("ShowOrganelle_SM");
        Lua.UnregisterFunction("HideOrganelle_SM");

        // SYS Code
        Lua.UnregisterFunction("NewTooltip_SM");
        Lua.UnregisterFunction("TooltipOver_SM");
        Lua.UnregisterFunction("LaserPointerONOFF_SM");
        Lua.UnregisterFunction("ShowingTooltipAnims_SM");
        Lua.UnregisterFunction("UnShowingTooltipAnims_SM");
        Lua.UnregisterFunction("ChangeTutorialStatus_SM");
        Lua.UnregisterFunction("StartAdventure_SM");
    }

    #endregion
}
