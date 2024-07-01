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

    public Image adenineCheckImage;
    public Image riboseCheckImage;
    public Image phosphateCheckImage_1;
    public Image phosphateCheckImage_2;

    void Start()
    {
        //UpdateUIText();
    }

    void Update()
    {
        if (adpMixTable.gameObject.activeSelf)
        {
            UpdateUIText();
            UpdateUIImage();
        }
    }

    public void UpdateUIText()
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

    public void UpdateUIImage()
    {
        if (adpMixTable == null) return;

        //adenineCheckText.GetComponentInParent<Image>().color =
        //    adpMixTable.isAdenine ? Color.green : Color.red;

        adenineCheckImage.color = adpMixTable.isAdenine ? Color.green : Color.red;
        riboseCheckImage.color = adpMixTable.isRibose ? Color.green : Color.red;
        phosphateCheckImage_1.color = adpMixTable.isPhosphate_1 ? Color.green : Color.red;
        phosphateCheckImage_2.color = adpMixTable.isPhosphate_2 ? Color.green : Color.red;
    }
}
