using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Item_Mito;

public class InventoryUI_Mito : MonoBehaviour
{
    public Inventory_Mito inventory;

    //public Text itemText;
    public TextMeshProUGUI adenineCntText;
    public TextMeshProUGUI riboseCntText;
    public TextMeshProUGUI phosphateCntText;
    public TextMeshProUGUI hIonCntText;
    public TextMeshProUGUI adpCntText;
    public TextMeshProUGUI atpCntText;

    private void Update()
    {
        if (inventory.gameObject.activeSelf)
            UpdateItemCounts();
    }

    //// 감지된 아이템의 정보를 출력
    //public void UpdateCurrentItemText(Grabbable item)
    //{
    //    Item_Mito itemMito = item.GetComponent<Item_Mito>();
    //    if (itemMito != null)
    //    {
    //        itemText.text = $"{itemMito.GetItemTypeName()}\n{GetItemCount(itemMito.type)}";
    //    }
    //}

    //// 인벤토리 텍스트 초기화
    //public void ClearCurrentItemText()
    //{
    //    itemText.text = "";
    //}

    // 인벤토리의 아이템 개수 가져오기
    private int GetItemCount(ItemType type)
    {
        switch (type)
        {
            case ItemType.Adenine:
                return inventory.adenineItems.Count;
            case ItemType.Ribose:
                return inventory.riboseItems.Count;
            case ItemType.Phosphate:
                return inventory.phosphateItems.Count;
            case ItemType.ADP:
                return inventory.adpItems.Count;
            case ItemType.ATP:
                return inventory.atpItems.Count;
            case ItemType.H_Ion:
                return inventory.hIonItems.Count;
            default:
                return 0;
        }
    }

    private void UpdateItemCounts()
    {
        adenineCntText.text = GetItemCount(ItemType.Adenine).ToString();
        riboseCntText.text = GetItemCount(ItemType.Ribose).ToString();
        phosphateCntText.text = GetItemCount(ItemType.Phosphate).ToString();
        hIonCntText.text = GetItemCount(ItemType.H_Ion).ToString();
        adpCntText.text = GetItemCount(ItemType.ADP).ToString();
        atpCntText.text = GetItemCount(ItemType.ATP).ToString();
    }

}
