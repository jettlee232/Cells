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

        adenineCntText.text = "�ʿ�\n�Ƶ���\n" + (requiredAdenine - currentAdenine);
        riboseCntText.text = "�ʿ�\n������\n" + (requiredRibose - currentRibose);
        phosphateCntText.text = "�ʿ�\n�λ�\n" + (requiredPhosphate - currentPhosphate);
    }
}
