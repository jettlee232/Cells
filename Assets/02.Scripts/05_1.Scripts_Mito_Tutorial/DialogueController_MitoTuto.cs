using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController_MitoTuto : MonoBehaviour
{
    public static DialogueController_MitoTuto Instance { get; private set; }

    public DialogueSystemTrigger[] dialogueSystemTriggers; // Ʈ���� ���

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // �ν��Ͻ��� �ߺ� �������� �ʵ��� ���� �ν��Ͻ��� �ı�
        }
    }

    public void ActivateDST(int n)
    {
        dialogueSystemTriggers[n - 1].startConversationEntryID = 0; // n��° Ʈ������ �������̼� ���� ��ȣ�� 0������ ���� (�̰� ���ص� �Ǳ�� �ѵ�, ���ϸ� ���߿� �������̼� ��Ȱ���� �Ұ�)
        dialogueSystemTriggers[n - 1].OnUse(); // On Use�� �������̼� �۵�            
    }

}
