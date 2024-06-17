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
        ActivateDST1();
    }

    public void ActivateDST1() // 1��° Ʈ���� �۵� �Լ�
    {
        if (GameManager_StageMap.instance.GetFirstEnd()) { dialogueSystemTrigger1.startConversationEntryID = 4; }
        else { dialogueSystemTrigger1.startConversationEntryID = 0; }
        dialogueSystemTrigger1.OnUse(); // On Use�� �������̼� �۵�
        DisableMove();
        GameManager_StageMap.instance.FirstEnd();
    }

    public void ActivateDST2() // 2��° Ʈ���� �۵� �Լ�
    {
        dialogueSystemTrigger2.OnUse(); // On Use�� �������̼� �۵�
        DisableMove();
        GameManager_StageMap.instance.secondEnd = true;
    }

    #region ��ũ��Ʈ�� ���� �Լ�
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
