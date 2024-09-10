using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Unity.VisualScripting;
using UnityEngine.XR;
using UnityEngine.UIElements;
using Language.Lua;
using System;
using DG.Tweening;
using MoreMountains.Feedbacks;
using static Oculus.Interaction.OptionalAttribute;

public class SelectDialogue_Lobby : MonoBehaviour
{
    // �켱 �Ʊ� �����ߴ� ���̾�α� �ý��� Ʈ���ŵ��� ������ �� �ִ� ���� 2���� ���� 
    public DialogueSystemController dsc;
    public DialogueSystemTrigger dialogueSystemTrigger1; // 1��° Ʈ����
    public DialogueSystemTrigger dialogueSystemTrigger2; // 2��° Ʈ����
    public DialogueSystemTrigger dialogueSystemTrigger3; // 3��° Ʈ����
    public GameObject rGrabber;
    public GameObject lGrabber;

    UnityEngine.XR.InputDevice right;
    UnityEngine.XR.InputDevice left;

    public float length = 5f;

    private Vector2 rMove = Vector2.zero;
    private GameObject player = null;

    // SYS Code
    [Header("AIChat Dummy & StarWatch")]
    public Transform aiChatDummy;
    private DG.Tweening.Tween sizeTween = null;
    public MMF_Player feedback_aiDummyCanv1;
    public MMF_Player feedback_aiDummyCanv2;
    public MMF_Player feedback_starWatchText;
    private int feedbackCnt = 0;
    public MMF_Player fb_speechBubble;
    public MMF_Player fb_speechBubble_0;
    public GameObject dummyPanelCanvas;

    [Header("Speech Bubble Tween")]
    public GameObject panelBubble;


    private void Start()
    {
        player = GameManager_Lobby.instance.GetPlayer();        
    }

    // SYS Code
    public void InstDummyPanel_LB(bool flag)
    {
        if (flag == true)
        {
            dummyPanelCanvas.SetActive(true);            
        }
        else
        {
            dummyPanelCanvas.SetActive(false);
        }
    }

    // SYS Code
    public void DialogueCanvasSize_LB(bool flag)
    {
        if (flag == true) // true�� Ŀ����
        {
            if (feedbackCnt != 0)
            {
                if (feedbackCnt == 1)
                {
                    feedback_aiDummyCanv1.SetDirection(MMFeedbacks.Directions.TopToBottom);
                    feedback_aiDummyCanv1.PlayFeedbacks();

                    feedbackCnt = 2;
                }
                else if (feedbackCnt == 2)
                {
                    feedback_aiDummyCanv1.StopFeedbacks(false);
                    feedback_aiDummyCanv1.SetDirection(MMFeedbacks.Directions.BottomToTop);
                    feedback_aiDummyCanv1.PlayFeedbacks();

                    feedback_aiDummyCanv2.SetDirection(MMFeedbacks.Directions.TopToBottom);
                    feedback_aiDummyCanv2.PlayFeedbacks();

                    feedbackCnt = 3;
                }
                else if (feedbackCnt == 3)
                {
                    feedback_aiDummyCanv2.StopFeedbacks(false);
                    feedback_aiDummyCanv2.SetDirection(MMFeedbacks.Directions.BottomToTop);
                    feedback_aiDummyCanv2.PlayFeedbacks();
                }
            }
            else
            {
                aiChatDummy.gameObject.SetActive(true);

                if (sizeTween != null) sizeTween.Kill(); // �̹� �ٸ� Ʈ���� ���� ���̾��ٸ� ���� ���̾��� Ʈ���� �ߴ��ϱ�

                sizeTween = aiChatDummy.transform.DOScale(new Vector3(1f, 1f, 1f), 1f).OnComplete(() => {
                    sizeTween = null;
                });

                feedbackCnt = 1;
            }
        }
        else // false�� �۾�����
        {
            if (sizeTween != null) sizeTween.Kill(); // �̹� �ٸ� Ʈ���� ���� ���̾��ٸ� ���� ���̾��� Ʈ���� �ߴ��ϱ�

            sizeTween = aiChatDummy.transform.DOScale(Vector3.zero, 1f).OnComplete(() => {
                sizeTween = null;
                aiChatDummy.gameObject.SetActive(false);
            });
        }
    }

    public void SpeechBubble_LB(bool flag)
    {
        if (flag == true)
        {
            fb_speechBubble.SetDirection(MMFeedbacks.Directions.TopToBottom);
            fb_speechBubble.PlayFeedbacks();
            fb_speechBubble_0.PlayFeedbacks();
        }
        else
        {
            fb_speechBubble.SetDirection(MMFeedbacks.Directions.BottomToTop);
            fb_speechBubble.PlayFeedbacks();
        }
    }

    public void TextFeel_LB(bool flag)
    {
        if (flag == true)
        {
            feedback_starWatchText.SetDirection(MMFeedbacks.Directions.TopToBottom);
            feedback_starWatchText.PlayFeedbacks();
        }
        else
        {
            feedback_starWatchText.SetDirection(MMFeedbacks.Directions.BottomToTop);
            feedback_starWatchText.PlayFeedbacks();
        }
    }

    public void ActivateDST1() // 1��° Ʈ���� �۵� �Լ�
    {
        StopPlayer_LB();
        if (GameManager_Lobby.instance.firstEnd) { dialogueSystemTrigger2.startConversationEntryID = 2; }
        dialogueSystemTrigger1.startConversationEntryID = 0; // 1��° Ʈ������ �������̼� ���� ��ȣ�� 0������ ���� (�̰� ���ص� �Ǳ�� �ѵ�, ���ϸ� ���߿� �������̼� ��Ȱ���� �Ұ�)
        dialogueSystemTrigger1.OnUse(); // On Use�� �������̼� �۵�
        GameManager_Lobby.instance.firstEnd = true;
    }

    public void ActivateDST2() // 2��° Ʈ���� �۵� �Լ�
    {
        // SYS Code
        GameManager_Lobby.instance.tutoStatus = 2;

        // SYS Code
        GameObject go = GameObject.Find("Custom Dialogue UI");

        StopPlayer_LB();
        HideNPCTalk_LB();
        GlowAllEnd_LB();
        if (GameManager_Lobby.instance.secondEnd) { dialogueSystemTrigger2.startConversationEntryID = 2; }
        else { dialogueSystemTrigger2.startConversationEntryID = -1; }

        dsc.StopAllConversations();
        dialogueSystemTrigger2.OnUse(); // On Use�� �������̼� �۵�
    }

    public void ActivateDST3()
    {
        StopPlayer_LB();
        dialogueSystemTrigger3.startConversationEntryID = 1;
        dsc.StopAllConversations();
        dialogueSystemTrigger3.OnUse(); // On Use�� �������̼� �۵�
    }

    // SYS Code
    public void CheckTutorial_LB() 
    {        
        StartCoroutine(CheckTutorial());
    }
    IEnumerator CheckTutorial()
    {
        // SYS Code
        /*
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
        */
        // SYS Code
        //GameManager_Lobby.instance.secondCon = true;        
        yield return new WaitForSeconds(1f);
        GameManager_Lobby.instance.tutoStatus = 1;
    }

    public void StopPlayer_LB() { GameManager_Lobby.instance.StopPlayer(); }
    public void MovePlayer_LB() { GameManager_Lobby.instance.EnableMovePlayer(); }

    public void Warpable_LB() { GameManager_Lobby.instance.SetWarpable(); GameManager_Lobby.instance.GetInteractable().GetComponent<InteractableManager_Lobby>().SetLight(); }
    
    public void ShowNPCTalk_LB() { UIManager_Lobby.instance.ShowBubble(); }
    public void HideNPCTalk_LB() { UIManager_Lobby.instance.HideBubble(); }

    public void GlowAllStart_LB() { GameManager_Lobby.instance.GlowAllStart(); }
    public void GlowAllEnd_LB() { GameManager_Lobby.instance.GlowAllEnd(); }

    public void ShowUpsideSubtitle_LB() { UIManager_Lobby.instance.SetQuest("���� ���� ��Ż�� ������!"); }

    // SYS Code
    public void GlowStartOnlySelected_LB(double start, double end) { GameManager_Lobby.instance.GlowStartOnlySelected(start, end); }
    public void GlowEndOnlySelected_LB(double start, double end) { GameManager_Lobby.instance.GlowEndOnlySelected(start, end); }


    // SYS Code
    public void NewTooltip_LB(double index, string content) { GameManager_Lobby.instance.NewTooltip((int)index, content); }
    public void TooltipOver_LB(double index) { GameManager_Lobby.instance.TooltipOver((int)index); }

    // SYS Code
    public void PortalShaderControllerEnable_LB(bool flag) { GameManager_Lobby.instance.PortalShaderControllerEnable(flag); }

    // SYS Code
    public void ShowingTooltipAnim_LB(double hand, double anim) { GameManager_Lobby.instance.ShowingTooltipAnim((int)hand, (int)anim); }
    public void UnShowingTooltipAnim_LB(double hand) { GameManager_Lobby.instance.UnShowingTooltipAnim((int)hand); }

    #region Register with Lua

    private void OnEnable()
    {
        Lua.RegisterFunction("CheckTutorial_LB", this, SymbolExtensions.GetMethodInfo(() => CheckTutorial_LB()));
        Lua.RegisterFunction("MovePlayer_LB", this, SymbolExtensions.GetMethodInfo(() => MovePlayer_LB()));
        Lua.RegisterFunction("Warpable_LB", this, SymbolExtensions.GetMethodInfo(() => Warpable_LB()));
        Lua.RegisterFunction("ShowNPCTalk_LB", this, SymbolExtensions.GetMethodInfo(() => ShowNPCTalk_LB()));
        Lua.RegisterFunction("HideNPCTalk_LB", this, SymbolExtensions.GetMethodInfo(() => ShowNPCTalk_LB()));
        Lua.RegisterFunction("GlowAllStart_LB", this, SymbolExtensions.GetMethodInfo(() => GlowAllStart_LB()));
        Lua.RegisterFunction("GlowAllEnd_LB", this, SymbolExtensions.GetMethodInfo(() => GlowAllEnd_LB()));
        Lua.RegisterFunction("ShowUpsideSubtitle_LB", this, SymbolExtensions.GetMethodInfo(() => ShowUpsideSubtitle_LB()));

        // SYS Code
        Lua.RegisterFunction("GlowStartOnlySelected_LB", this, SymbolExtensions.GetMethodInfo(() => GlowStartOnlySelected_LB((double)0, (double)0)));
        Lua.RegisterFunction("GlowEndOnlySelected_LB", this, SymbolExtensions.GetMethodInfo(() => GlowEndOnlySelected_LB((double)0, (double)0)));

        // SYS Code
        Lua.RegisterFunction("NewTooltip_LB", this, SymbolExtensions.GetMethodInfo(() => NewTooltip_LB((double)0, (string)null)));
        Lua.RegisterFunction("TooltipOver_LB", this, SymbolExtensions.GetMethodInfo(() => TooltipOver_LB((double)0)));

        // SYS Code
        Lua.RegisterFunction("PortalShaderControllerEnable_LB", this, SymbolExtensions.GetMethodInfo(() => PortalShaderControllerEnable_LB((bool)false)));
        Lua.RegisterFunction("ShowingTooltipAnim_LB", this, SymbolExtensions.GetMethodInfo(() => ShowingTooltipAnim_LB((double)0, (double)0)));
        Lua.RegisterFunction("UnShowingTooltipAnim_LB", this, SymbolExtensions.GetMethodInfo(() => UnShowingTooltipAnim_LB((double)0)));

        // SYS Code
        Lua.RegisterFunction("DialogueCanvasSize_LB", this, SymbolExtensions.GetMethodInfo(() => DialogueCanvasSize_LB((bool)false)));
        Lua.RegisterFunction("TextFeel_LB", this, SymbolExtensions.GetMethodInfo(() => TextFeel_LB((bool)false)));
        Lua.RegisterFunction("SpeechBubble_LB", this, SymbolExtensions.GetMethodInfo(() => SpeechBubble_LB((bool)false)));
        Lua.RegisterFunction("InstDummyPanel_LB", this, SymbolExtensions.GetMethodInfo(() => InstDummyPanel_LB((bool)false)));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("CheckTutorial_LB");
        Lua.UnregisterFunction("MovePlayer_LB");
        Lua.UnregisterFunction("Warpable_LB");
        Lua.UnregisterFunction("ShowNPCTalk_LB");
        Lua.UnregisterFunction("HideNPCTalk_LB");
        Lua.UnregisterFunction("GlowAllStart_LB");
        Lua.UnregisterFunction("GlowAllEnd_LB");
        Lua.UnregisterFunction("ShowUpsideSubtitle_LB");

        // SYS Code
        Lua.UnregisterFunction("GlowStartOnlySelected_LB");
        Lua.UnregisterFunction("GlowEndOnlySelected_LB");

        // SYS Code
        Lua.UnregisterFunction("NewTooltip_LB");
        Lua.UnregisterFunction("TooltipOver_LB");

        // SYS Code
        Lua.UnregisterFunction("PortalShaderControllerEnable_LB");
        Lua.UnregisterFunction("ShowingTooltipAnim_LB");
        Lua.UnregisterFunction("UnShowingTooltipAnim_LB");

        // SYS Code
        Lua.UnregisterFunction("DialogueCanvasSize_LB");
        Lua.UnregisterFunction("TextFeel_LB");
        Lua.UnregisterFunction("SpeechBubble_LB");
        Lua.UnregisterFunction("InstDummyPanel_LB");
    }

    #endregion
}
