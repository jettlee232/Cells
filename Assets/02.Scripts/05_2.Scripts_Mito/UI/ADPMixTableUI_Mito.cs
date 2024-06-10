using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADPMixTableUI_Mito : MonoBehaviour
{
    public ADPMixTable_Mito adpMixTable;
    public Text adenineCntText;
    public Text riboseCntText;
    public Text phosphateCntText;

    private int requiredAdenine = 1;
    private int requiredRibose = 1;
    private int requiredPhosphate = 2;

    void Start()
    {
        UpdateUIText();
    }

    void Update()
    {
        UpdateUIText();
    }

    void UpdateUIText()
    {
        int currentAdenine = adpMixTable.isAdenine ? 1 : 0;
        int currentRibose = adpMixTable.isRibose ? 1 : 0;
        int currentPhosphate = (adpMixTable.isPhosphate_1 ? 1 : 0) + (adpMixTable.isPhosphate_2 ? 1 : 0);

        adenineCntText.text = "필요\n아데닌\n" + (requiredAdenine - currentAdenine);
        riboseCntText.text = "필요\n리보스\n" + (requiredRibose - currentRibose);
        phosphateCntText.text = "필요\n인산\n" + (requiredPhosphate - currentPhosphate);
    }
}
