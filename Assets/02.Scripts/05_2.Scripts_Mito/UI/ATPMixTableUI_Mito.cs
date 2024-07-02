using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ATPMixTableUI_Mito : MonoBehaviour
{
    public ATPMixTableInside_Mito[] atpMixTableInsides;
    public ATPMixTableOutside_Mito[] atpMixTableOutsides;

    public TextMeshProUGUI[] adpCheckTexts;
    public TextMeshProUGUI[] phosphateCheckTexts;
    public TextMeshProUGUI[] hIonCountTexts;
    public TextMeshProUGUI[] hIonPercentageTexts;

    public Image[] adpCheckImages;
    public Image[] phosphateCheckImages;
    public Image[] hIonCheckImages;

    private int requiredADP = 1;
    private int requiredPhosphate = 1;
    private int requiredHIonPerATP = 3;

    void Start()
    {
        //UpdateInsideUIText();
        //UpdateOutsideUIText();
    }

    void Update()
    {
        UpdateAllUIs();
    }

    void UpdateAllUIs()
    {
        for (int i = 0; i < atpMixTableInsides.Length; i++)
        {
            if (atpMixTableInsides[i] != null && atpMixTableInsides[i].gameObject.activeSelf)
            {
                UpdateInsideUIText(i);
                UpdateInsideUIImage(i);
            }
        }

        for (int i = 0; i < atpMixTableOutsides.Length; i++)
        {
            if (atpMixTableOutsides[i] != null && atpMixTableOutsides[i].gameObject.activeSelf)
            {
                UpdateOutsideUIText(i);
                UpdateOutsideUIImage(i);
            }
        }
    }

    void UpdateInsideUIText(int index)
    {
        string currentADP = atpMixTableInsides[index].isADP ? "O" : "X";
        string currentPhosphate = atpMixTableInsides[index].isPhosphate ? "O" : "X";

        adpCheckTexts[index].text = currentADP;
        phosphateCheckTexts[index].text = currentPhosphate;
    }

    void UpdateInsideUIImage(int index)
    {
        adpCheckImages[index].color = atpMixTableInsides[index].isADP ? Color.green : Color.red;
        phosphateCheckImages[index].color = atpMixTableInsides[index].isPhosphate ? Color.green : Color.red;
    }

    void UpdateOutsideUIText(int index)
    {
        int currentHIon = atpMixTableOutsides[index].curHIonCount;
        float maxHIonCount = atpMixTableOutsides[index].maxHIonCount;
        float hIonPercentage = (currentHIon / maxHIonCount) * 100.0f;

        hIonCountTexts[index].text = $"{Mathf.Max(0, requiredHIonPerATP - currentHIon)}";
        hIonPercentageTexts[index].text = $"{hIonPercentage:F1}%";
    }

    void UpdateOutsideUIImage(int index)
    {
        hIonCheckImages[index].color = atpMixTableOutsides[index].curHIonCount >= requiredHIonPerATP ? Color.green : Color.red;
    }

}