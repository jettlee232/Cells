using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Unity.VisualScripting;
using UnityEngine.XR;
using UnityEngine.UIElements;

public class SelectDialogue_StageMap : MonoBehaviour
{
    // �켱 �Ʊ� �����ߴ� ���̾�α� �ý��� Ʈ���ŵ��� ������ �� �ִ� ���� 2���� ���� 
    public DialogueSystemTrigger dialogueSystemTrigger1; // 1��° Ʈ����
    public DialogueSystemTrigger dialogueSystemTrigger2; // 2��° Ʈ����

    UnityEngine.XR.InputDevice right;

    public float length = 5f;
    private GameObject player = null;
    private bool checkFly = false;

    private void Start()
    {
        player = GameManager_StageMap.instance.GetPlayer();
    }

    public void ActivateDST1() // 1��° Ʈ���� �۵� �Լ�
    {
        if (GameManager_StageMap.instance.firstEnd) { dialogueSystemTrigger1.startConversationEntryID = 4; }
        else { dialogueSystemTrigger1.startConversationEntryID = 0; }
        dialogueSystemTrigger1.OnUse(); // On Use�� �������̼� �۵�
        GameManager_StageMap.instance.firstEnd = true;
    }

    public void ActivateDST2() // 2��° Ʈ���� �۵� �Լ�
    {
        dialogueSystemTrigger2.OnUse(); // On Use�� �������̼� �۵�
        GameManager_StageMap.instance.secondEnd = true;
    }

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

    #region Register with Lua

    private void OnEnable()
    {
        Lua.RegisterFunction("fCheckFlyTutorial", this, SymbolExtensions.GetMethodInfo(() => fCheckFlyTutorial()));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("fCheckFlyTutorial");
    }

    #endregion
}
