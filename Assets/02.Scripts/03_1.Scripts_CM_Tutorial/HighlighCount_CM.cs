using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class HighlighCount_CM : MonoBehaviour
{
    public TutorialManager_CM tutoMgr;
    private bool flag = true;


    public void ChangeFlag()
    {
        flag = false;
    }

    public bool ReturnFlag()
    {
        return flag;
    }
}
