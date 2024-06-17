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
    public DialogueSystemTrigger dst4_1; // 4_1��° Ʈ����
    public DialogueSystemTrigger dst4_2; // 5_2��° Ʈ����
    public DialogueSystemTrigger dst5; // 5��° Ʈ����
    void Start()
    {
        
    }

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCov_Second();
        }
    }
    */

    public void StartCov_First()
    {
        dst1.startConversationEntryID = 0;
        dst1.OnUse();
    }

    public void StartCov_Second()
    {
        dst2.startConversationEntryID = 0;
        dst2.OnUse();
    }

    public void StartCov_Third()
    {
        dst3.startConversationEntryID = 0;
        dst3.OnUse();
    }

    public void StartCov_Fourth_1()
    {
        dst4_1.startConversationEntryID = 0;
        dst4_1.OnUse();
    }

    public void StartCov_Fourth_2()
    {
        dst4_2.startConversationEntryID = 0;
        dst4_2.OnUse();
    }

    public void StartCov_Fifth()
    {
        dst5.startConversationEntryID = 0;
        dst5.OnUse();
    }
}
