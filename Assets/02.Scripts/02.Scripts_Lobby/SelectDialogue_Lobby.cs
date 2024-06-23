using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Unity.VisualScripting;
using UnityEngine.XR;
using UnityEngine.UIElements;

public class SelectDialogue_Lobby : MonoBehaviour
{
    // 우선 아까 생성했던 다이얼로그 시스템 트리거들을 통제할 수 있는 변수 2개를 선언 
    public DialogueSystemTrigger dialogueSystemTrigger1; // 1번째 트리거
    public DialogueSystemTrigger dialogueSystemTrigger2; // 2번째 트리거
    public DialogueSystemTrigger dialogueSystemTrigger3; // 3번째 트리거
    public GameObject rGrabber;
    public GameObject lGrabber;

    UnityEngine.XR.InputDevice right;
    UnityEngine.XR.InputDevice left;

    public float length = 5f;

    private Vector2 rMove = Vector2.zero;
    private GameObject player = null;

    private void Start()
    {
        player = GameManager_Lobby.instance.GetPlayer();
    }

    public void ActivateDST1() // 1번째 트리거 작동 함수
    {
        fStopPlayer_LB();
        if (GameManager_Lobby.instance.firstEnd) { dialogueSystemTrigger2.startConversationEntryID = 2; }
        dialogueSystemTrigger1.startConversationEntryID = 0; // 1번째 트리거의 컨버제이션 진입 번호를 0번으로 변경 (이거 안해도 되기는 한데, 안하면 나중에 컨버제이션 재활용이 불가)
        dialogueSystemTrigger1.OnUse(); // On Use로 컨버제이션 작동
        GameManager_Lobby.instance.firstEnd = true;
    }

    public void ActivateDST2() // 2번째 트리거 작동 함수
    {
        fStopPlayer_LB();
        if (GameManager_Lobby.instance.secondEnd) { dialogueSystemTrigger2.startConversationEntryID = 2; }
        else { dialogueSystemTrigger2.startConversationEntryID = 0; }
        dialogueSystemTrigger2.OnUse(); // On Use로 컨버제이션 작동
        GameManager_Lobby.instance.secondEnd = true;
    }

    public void ActivateDST3() // 1번째 트리거 작동 함수
    {
        fStopPlayer_LB();
        dialogueSystemTrigger3.startConversationEntryID = 0; // 1번째 트리거의 컨버제이션 진입 번호를 0번으로 변경 (이거 안해도 되기는 한데, 안하면 나중에 컨버제이션 재활용이 불가)
        dialogueSystemTrigger3.OnUse(); // On Use로 컨버제이션 작동
    }

    public void fCheckTutorial() { StartCoroutine(CheckTutorial()); }
    IEnumerator CheckTutorial()
    {
        bool moveForward = false;
        bool moveBackward = false;
        bool moveLeft = false;
        bool moveRight = false;
        bool holdGrip = false;

        while (true)
        {
            left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            left.TryGetFeatureValue(CommonUsages.primary2DAxis, out rMove);

            if (!moveForward) { if (rMove.y >= 0.1f) moveForward = true; }
            if (!moveBackward) { if (rMove.y <= -0.1f) moveBackward = true; }
            if (!moveLeft) { if (rMove.x <= -0.1f) moveLeft = true; }
            if (!moveRight) { if (rMove.x >= 0.1f) moveRight = true; }

            if (!holdGrip) { if ((rGrabber.transform.childCount + lGrabber.transform.childCount) >= 1) { holdGrip = true; } }

            if (moveForward && moveBackward && moveLeft && moveRight && holdGrip) { break; }

            yield return new WaitForSeconds(0.02f);
        }
        GameManager_Lobby.instance.secondCon = true;
    }

    public void fStopPlayer_LB() { GameManager_Lobby.instance.StopPlayer(); }
    public void fMovePlayer_LB() { GameManager_Lobby.instance.EnableMovePlayer(); }

    public void fWarpable_LB() { GameManager_Lobby.instance.SetWarpable(); }

    public void fShowNPCTalk_LB() { UIManager_Lobby.instance.ShowNPCTalk(); }

    public void fShowUpsideSubtitle_LB(string des) { UIManager_Lobby.instance.SetUpsideSubtitle(des); }

    #region Register with Lua

    private void OnEnable()
    {
        Lua.RegisterFunction("fCheckTutorial", this, SymbolExtensions.GetMethodInfo(() => fCheckTutorial()));
        Lua.RegisterFunction("fMovePlayer_LB", this, SymbolExtensions.GetMethodInfo(() => fMovePlayer_LB()));
        Lua.RegisterFunction("fWarpable_LB", this, SymbolExtensions.GetMethodInfo(() => fWarpable_LB()));
        Lua.RegisterFunction("fShowNPCTalk_LB", this, SymbolExtensions.GetMethodInfo(() => fShowNPCTalk_LB()));
        Lua.RegisterFunction("fShowUpsideSubtitle_LB", this, SymbolExtensions.GetMethodInfo(() => fShowUpsideSubtitle_LB(string.Empty)));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("fCheckTutorial");
        Lua.UnregisterFunction("fMovePlayer_LB");
        Lua.UnregisterFunction("fWarpable_LB");
        Lua.UnregisterFunction("fShowNPCTalk_LB");
        Lua.UnregisterFunction("fShowUpsideSubtitle_LB");
    }

    #endregion
}
