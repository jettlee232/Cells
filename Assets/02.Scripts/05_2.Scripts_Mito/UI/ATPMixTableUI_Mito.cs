using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ATPMixTableUI_Mito : MonoBehaviour
{
    //public ATPMixTable_Mito atpMixTable;
    public ATPMixTableInside_Mito atpMixTableInside;
    public ATPMixTableOutside_Mito atpMixTableOutside;
    public Text adpCntText;
    public Text phosphateCntText;
    public Text maxHIonCountText;
    public Text needHIonCountText;

    private int requiredADP = 1;
    private int requiredPhosphate = 1;
    private int requiredHIonPerATP = 3;

    void Start()
    {
        UpdateInsideUIText();
        UpdateOutsideUIText();
    }

    void Update()
    {
        UpdateInsideUIText();
        UpdateOutsideUIText();
    }

    void UpdateInsideUIText()
    {
        int currentADP = atpMixTableInside.isADP ? 1 : 0;
        int currentPhosphate = atpMixTableInside.isPhosphate ? 1 : 0;

        adpCntText.text = "필요\nADP\n" + (requiredADP - currentADP);
        phosphateCntText.text = "필요\n인산염\n" + (requiredPhosphate - currentPhosphate);
        
    }

    void UpdateOutsideUIText()
    {
        int currentHIon = atpMixTableOutside.curHIonCount;
        float maxHIonCount = atpMixTableOutside.maxHIonCount;
        float hIonPercentage = (currentHIon / maxHIonCount) * 100.0f;

        //maxHIonCountText.text = $"전체\n수소이온\n{currentHIon / atpMixTable.maxHIonCount * 100}%";//수정필요
        maxHIonCountText.text = $"전체\n수소이온\n{hIonPercentage:F1}%";
        //needHIonCountText.text = "필요\n수소이온\n" + (requiredHIonPerATP - currentHIon);//수정필요
        needHIonCountText.text = $"필요\n수소이온\n{Mathf.Max(0, requiredHIonPerATP - currentHIon)}";
    }
}