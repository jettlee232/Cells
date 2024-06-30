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
        StopPlayer_LB();
        if (GameManager_Lobby.instance.firstEnd) { dialogueSystemTrigger2.startConversationEntryID = 2; }
        dialogueSystemTrigger1.startConversationEntryID = 0; // 1번째 트리거의 컨버제이션 진입 번호를 0번으로 변경 (이거 안해도 되기는 한데, 안하면 나중에 컨버제이션 재활용이 불가)
        dialogueSystemTrigger1.OnUse(); // On Use로 컨버제이션 작동
        GameManager_Lobby.instance.firstEnd = true;
    }

    public void ActivateDST2() // 2번째 트리거 작동 함수
    {
        StopPlayer_LB();
        if (GameManager_Lobby.instance.secondEnd) { dialogueSystemTrigger2.startConversationEntryID = 2; }
        else { dialogueSystemTrigger2.startConversationEntryID = 0; }
        dialogueSystemTrigger2.OnUse(); // On Use로 컨버제이션 작동
        GameManager_Lobby.instance.secondEnd = true;
    }

    public void CheckTutorial_LB() { StartCoroutine(CheckTutorial()); }
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

    public void StopPlayer_LB() { GameManager_Lobby.instance.StopPlayer(); }
    public void MovePlayer_LB() { GameManager_Lobby.instance.EnableMovePlayer(); }

    public void Warpable_LB() { GameManager_Lobby.instance.SetWarpable(); }

    public void ShowNPCTalk_LB() { UIManager_Lobby.instance.ShowBubble(); }

    public void ShowUpsideSubtitle_LB() { UIManager_Lobby.instance.SetQuest("동물 세포 포탈로 들어가보자!"); }

    #region Register with Lua

    private void OnEnable()
    {
        Lua.RegisterFunction("CheckTutorial_LB", this, SymbolExtensions.GetMethodInfo(() => CheckTutorial_LB()));
        Lua.RegisterFunction("MovePlayer_LB", this, SymbolExtensions.GetMethodInfo(() => MovePlayer_LB()));
        Lua.RegisterFunction("Warpable_LB", this, SymbolExtensions.GetMethodInfo(() => Warpable_LB()));
        Lua.RegisterFunction("ShowNPCTalk_LB", this, SymbolExtensions.GetMethodInfo(() => ShowNPCTalk_LB()));
        Lua.RegisterFunction("ShowUpsideSubtitle_LB", this, SymbolExtensions.GetMethodInfo(() => ShowUpsideSubtitle_LB()));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("CheckTutorial_LB");
        Lua.UnregisterFunction("MovePlayer_LB");
        Lua.UnregisterFunction("Warpable_LB");
        Lua.UnregisterFunction("ShowNPCTalk_LB");
        Lua.UnregisterFunction("ShowUpsideSubtitle_LB");
    }

    #endregion
}
