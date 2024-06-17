using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class NarratorDialogueHub_CM_Tutorial : MonoBehaviour
{   
    [Header("Dialogue Databases")]
    public DialogueSystemTrigger dst1; // 1��° Ʈ����
    public DialogueSystemTrigger dst2; // 2��° Ʈ����
    public DialogueSystemTrigger dst3; // 3��° Ʈ����
    public DialogueSystemTrigger dst4; // 4��° Ʈ����    
    public DialogueSystemTrigger dst5; // 5��° Ʈ����
    public DialogueSystemTrigger dst6; // 6��° Ʈ����

    public void StartCov_1()
    {
        dst1.startConversationEntryID = 0;
        dst1.OnUse();
    }

    public void StartCov_2()
    {
        dst2.startConversationEntryID = 0;
        dst2.OnUse();
    }

    public void StartCov_3()
    {
        dst3.startConversationEntryID = 0;
        dst3.OnUse();
    }

    public void StartCov_4()
    {
        dst4.startConversationEntryID = 0;
        dst4.OnUse();
    }

    public void StartCov_5()
    {
        dst5.startConversationEntryID = 0;
        dst5.OnUse();
    }

    public void StartCov_6()
    {
        dst6.startConversationEntryID = 0;
        dst6.OnUse();
    }
}
