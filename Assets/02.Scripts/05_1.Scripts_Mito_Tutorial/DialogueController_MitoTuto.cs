using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController_MitoTuto : MonoBehaviour
{
    public DialogueSystemTrigger dialogueSystemTrigger1; // 1��° Ʈ����
    public DialogueSystemTrigger dialogueSystemTrigger2; // 2��° Ʈ����

    public void ActivateDST1() // 1��° Ʈ���� �۵� �Լ�
    {
        dialogueSystemTrigger1.startConversationEntryID = 0; // 1��° Ʈ������ �������̼� ���� ��ȣ�� 0������ ���� (�̰� ���ص� �Ǳ�� �ѵ�, ���ϸ� ���߿� �������̼� ��Ȱ���� �Ұ�)
        dialogueSystemTrigger1.OnUse(); // On Use�� �������̼� �۵�           
    }

    public void ActivateDST2() // 2��° Ʈ���� �۵� �Լ�
    {
        dialogueSystemTrigger2.startConversationEntryID = 0; // 1��° Ʈ������ �������̼� ���� ��ȣ�� 0������ ���� (�̰� ���ص� �Ǳ�� �ѵ�, ���ϸ� ���߿� �������̼� ��Ȱ���� �Ұ�)
        dialogueSystemTrigger2.OnUse(); // On Use�� �������̼� �۵�              
    }

    // �̰� �Լ��� �۵��ϴ��� ���� �׽�Ʈ �����ϰ� ���� ������Ʈ��, ���߿� �����ϸ� ��
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateDST1();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            ActivateDST2();
        }
    }
}
