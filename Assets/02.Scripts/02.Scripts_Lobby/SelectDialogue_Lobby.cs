using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Unity.VisualScripting;
using UnityEngine.XR;
using UnityEngine.UIElements;

public class SelectDialogue_Lobby : MonoBehaviour
{
    // �켱 �Ʊ� �����ߴ� ���̾�α� �ý��� Ʈ���ŵ��� ������ �� �ִ� ���� 2���� ���� 
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

    private void Start()
    {
        player = GameManager_Lobby.instance.GetPlayer();
    }

    public void ActivateDST1() // 1��° Ʈ���� �۵� �Լ�
    {
        dialogueSystemTrigger1.startConversationEntryID = 0; // 1��° Ʈ������ �������̼� ���� ��ȣ�� 0������ ���� (�̰� ���ص� �Ǳ�� �ѵ�, ���ϸ� ���߿� �������̼� ��Ȱ���� �Ұ�)
        dialogueSystemTrigger1.OnUse(); // On Use�� �������̼� �۵�
        GameManager_Lobby.instance.firstEnd = true;
    }

    public void ActivateDST2() // 2��° Ʈ���� �۵� �Լ�
    {
        if (GameManager_Lobby.instance.secondEnd) { dialogueSystemTrigger2.startConversationEntryID = 2; }
        else { dialogueSystemTrigger2.startConversationEntryID = 0; }
        dialogueSystemTrigger2.OnUse(); // On Use�� �������̼� �۵�
        GameManager_Lobby.instance.secondEnd = true;
    }

    public void ActivateDST3() // 1��° Ʈ���� �۵� �Լ�
    {
        dialogueSystemTrigger3.startConversationEntryID = 0; // 1��° Ʈ������ �������̼� ���� ��ȣ�� 0������ ���� (�̰� ���ص� �Ǳ�� �ѵ�, ���ϸ� ���߿� �������̼� ��Ȱ���� �Ұ�)
        dialogueSystemTrigger3.OnUse(); // On Use�� �������̼� �۵�
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

    #region Register with Lua

    private void OnEnable()
    {
        Lua.RegisterFunction("fCheckTutorial", this, SymbolExtensions.GetMethodInfo(() => fCheckTutorial()));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("fCheckTutorial");
    }

    #endregion
}
