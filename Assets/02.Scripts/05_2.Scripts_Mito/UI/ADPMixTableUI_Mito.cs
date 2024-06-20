using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ADPMixTableUI_Mito : MonoBehaviour
{
    public ADPMixTable_Mito adpMixTable;
    public TextMeshProUGUI adenineCheckText;
    public TextMeshProUGUI riboseCheckText;
    public TextMeshProUGUI phosphateCheckText_1;
    public TextMeshProUGUI phosphateCheckText_2;

    void Start()
    {
        UpdateUIText();
    }

    void Update()
    {
        UpdateUIText();
        UpdateUIImage();
    }

    void UpdateUIText()
    {
        string currentAdenine = adpMixTable.isAdenine ? "O" : "X";
        string currentRibose = adpMixTable.isRibose ? "O" : "X";
        string currentPhosphate_1 = adpMixTable.isPhosphate_1 ? "O" : "X";
        string currentPhosphate_2 = adpMixTable.isPhosphate_2 ? "O" : "X";

        adenineCheckText.text = currentAdenine;
        riboseCheckText.text = currentRibose;
        phosphateCheckText_1.text = currentPhosphate_1;
        phosphateCheckText_2.text = currentPhosphate_2;
    }

    void UpdateUIImage()
    {
        adenineCheckText.GetComponentInParent<Image>().color =
            adpMixTable.isAdenine ? Color.green : Color.red;

        riboseCheckText.GetComponentInParent<Image>().color =
            adpMixTable.isRibose ? Color.green : Color.red;

        phosphateCheckText_1.GetComponentInParent<Image>().color =
            adpMixTable.isPhosphate_1 ? Color.green : Color.red;

        phosphateCheckText_2.GetComponentInParent<Image>().color =
            adpMixTable.isPhosphate_2 ? Color.green : Color.red;
    }
}
