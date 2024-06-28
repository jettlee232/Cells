using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ATPMixTableUI_Mito : MonoBehaviour
{
    //public ATPMixTable_Mito atpMixTable;
    public ATPMixTableInside_Mito atpMixTableInside;
    public ATPMixTableOutside_Mito atpMixTableOutside;
    public TextMeshProUGUI adpCheckText;
    public TextMeshProUGUI phosphateCheckText;
    public TextMeshProUGUI hIonCountText;
    public TextMeshProUGUI hIonPercentageText;

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
        UpdateInsideUIImage();
        UpdateOutsideUIText();
        UpdateOutsideUIImage();
    }

    void UpdateInsideUIText()
    {
        string currentADP = atpMixTableInside.isADP ? "O" : "X";
        string currentPhosphate = atpMixTableInside.isPhosphate ? "O" : "X";

        adpCheckText.text = currentADP;
        phosphateCheckText.text = currentPhosphate;
    }

    void UpdateInsideUIImage()
    {
        adpCheckText.GetComponentInParent<Image>().color =
            atpMixTableInside.isADP ? Color.green : Color.red;

        phosphateCheckText.GetComponentInParent<Image>().color =
            atpMixTableInside.isPhosphate ? Color.green : Color.red;
    }

    void UpdateOutsideUIText()
    {
        int currentHIon = atpMixTableOutside.curHIonCount;
        float maxHIonCount = atpMixTableOutside.maxHIonCount;
        float hIonPercentage = (currentHIon / maxHIonCount) * 100.0f;

        hIonCountText.text = $"{Mathf.Max(0, requiredHIonPerATP - currentHIon)}";
        hIonPercentageText.text = $"{hIonPercentage:F1}%";
    }

    void UpdateOutsideUIImage()
    {
        hIonCountText.GetComponentInParent<Image>().color =
            atpMixTableOutside.curHIonCount >= requiredHIonPerATP ? Color.green : Color.red;
    }

}