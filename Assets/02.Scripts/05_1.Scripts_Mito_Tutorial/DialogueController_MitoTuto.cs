using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController_MitoTuto : MonoBehaviour
{
    public DialogueSystemTrigger[] dialogueSystemTriggers; // Ʈ���� ���

    public void ActivateDST3() // 3��° Ʈ���� �۵� �Լ�
    {
        dialogueSystemTriggers[2].startConversationEntryID = 0; // 1��° Ʈ������ �������̼� ���� ��ȣ�� 0������ ���� (�̰� ���ص� �Ǳ�� �ѵ�, ���ϸ� ���߿� �������̼� ��Ȱ���� �Ұ�)
        dialogueSystemTriggers[2].OnUse(); // On Use�� �������̼� �۵�           
    }

    public void ActivateDST4()
    {
        dialogueSystemTriggers[3].startConversationEntryID = 0; // 1��° Ʈ������ �������̼� ���� ��ȣ�� 0������ ���� (�̰� ���ص� �Ǳ�� �ѵ�, ���ϸ� ���߿� �������̼� ��Ȱ���� �Ұ�)
        dialogueSystemTriggers[3].OnUse(); // On Use�� �������̼� �۵�              
    }

    public void ActivateDST(int n)
    {
        dialogueSystemTriggers[n - 1].startConversationEntryID = 0; // n��° Ʈ������ �������̼� ���� ��ȣ�� 0������ ���� (�̰� ���ص� �Ǳ�� �ѵ�, ���ϸ� ���߿� �������̼� ��Ȱ���� �Ұ�)
        dialogueSystemTriggers[n - 1].OnUse(); // On Use�� �������̼� �۵�            
    }

    // �̰� �Լ��� �۵��ϴ��� ���� �׽�Ʈ �����ϰ� ���� ������Ʈ��, ���߿� �����ϸ� ��
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateDST(3);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            ActivateDST(4);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateDST(5);
        }
    }
}
