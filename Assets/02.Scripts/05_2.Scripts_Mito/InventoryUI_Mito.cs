using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI_Mito : MonoBehaviour
{
    public Inventory_Mito inventory;
    public Text adenineCntText;
    public Text riboseCntText;
    public Text phosphateCntText;
    public Text atpCntText;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        // 인벤토리 리스트의 카운트를 UI에 반영
        adenineCntText.text = "아데닌\n" + inventory.adenineItems.Count;
        riboseCntText.text = "리보스\n" + inventory.riboseItems.Count;
        phosphateCntText.text = "인산\n" + inventory.phosphateItems.Count;
        atpCntText.text = "ATP\n" + inventory.atpItems.Count;
    }
}
