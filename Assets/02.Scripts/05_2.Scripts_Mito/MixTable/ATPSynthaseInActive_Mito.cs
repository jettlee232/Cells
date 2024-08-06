using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ATPSynthaseInActive_Mito : MonoBehaviour
{
    public GameObject atpInsidePanel;
    public GameObject atpOutsidePanel;
    public GameObject friendAtpSynthase;

    public GameObject text;

    public void ShowUIPanel(ATPMixTablePos_Mito.PosType posType)
    {
        HideAllUIPanels();

        if (posType == ATPMixTablePos_Mito.PosType.In)
        {
            atpInsidePanel.SetActive(true);
        }
        else if (posType == ATPMixTablePos_Mito.PosType.Out)
        {
            atpOutsidePanel.SetActive(true);
        }
    }

    public void HideUIPanel(ATPMixTablePos_Mito.PosType posType)
    {
        if (posType == ATPMixTablePos_Mito.PosType.In)
        {
            atpInsidePanel.SetActive(false);
        }
        else if (posType == ATPMixTablePos_Mito.PosType.Out)
        {
            atpOutsidePanel.SetActive(false);
        }
    }

    void HideAllUIPanels()
    {
        atpInsidePanel.SetActive(false);
        atpOutsidePanel.SetActive(false);
    }

    public void OnATPCreated()
    {
        friendAtpSynthase.SetActive(true);
        text.SetActive(false);
        gameObject.transform.GetChild(0).SetParent(transform.parent);
        gameObject.SetActive(false);
    }
}
