using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class NarratorDialogueHub_CM_Tutorial : MonoBehaviour
{
    public DialogueSystemController dsc;

    [Header("Dialogue Databases")]
    public DialogueSystemTrigger dst1;
    public DialogueSystemTrigger dst2;
    public DialogueSystemTrigger dst3;
    public DialogueSystemTrigger dst4;
    public DialogueSystemTrigger dst5;
    public DialogueSystemTrigger dst6;
    public DialogueSystemTrigger dst7;

    public void StartCov_1()
    {
        StopConv();
        dst1.startConversationEntryID = 0;
        dst1.OnUse();
    }

    public void StartCov_2()
    {
        StopConv();
        dst2.startConversationEntryID = 0;
        dst2.OnUse();
    }

    public void StartCov_3()
    {
        StopConv();
        dst3.startConversationEntryID = 0;
        dst3.OnUse();
    }

    public void StartCov_4()
    {
        AudioMgr_CM.Instance.PlaySFXByInt(1);
        AudioMgr_CM.Instance.PlaySFXByInt(14);

        StopConv();
        dst4.startConversationEntryID = 0;
        dst4.OnUse();
    }

    public void StartCov_5()
    {
        AudioMgr_CM.Instance.PlaySFXByInt(1);
        AudioMgr_CM.Instance.PlaySFXByInt(12);

        StopConv();
        dst5.startConversationEntryID = 0;
        dst5.OnUse();
    }

    public void StartCov_6()
    {
        StopConv();
        dst6.startConversationEntryID = 0;
        dst6.OnUse();
    }

    public void StartCov_7()
    {
        StopConv();
        dst7.startConversationEntryID = 0;
        dst7.OnUse();
    }

    void StopConv()
    {
        dsc.StopAllConversations(); ;
    }
}
