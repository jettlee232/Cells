using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ATPMixTableUI_Mito : MonoBehaviour
{
    public ATPMixTable_Mito atpMixTable;
    public Text adpCntText;
    public Text maxHIonCountText;
    public Text needHIonCountText;

    private int requiredADP = 1;
    private int requiredHIonPerATP = 3;

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
        int currentHIon = atpMixTable.curHIonCount;
        int currentADP = atpMixTable.isADP ? 1 : 0;

        adpCntText.text = "�ʿ�\nADP\n" + (requiredADP - currentADP);
        maxHIonCountText.text = $"��ü\n�����̿�\n{currentHIon}/{atpMixTable.maxHIonCount}";//�����ʿ�
        needHIonCountText.text = "�ʿ�\n�����̿�\n" + (requiredHIonPerATP - currentHIon);//�����ʿ�
    }
}