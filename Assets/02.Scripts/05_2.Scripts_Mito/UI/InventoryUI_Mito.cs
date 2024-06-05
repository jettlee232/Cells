using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Item_Mito;

public class InventoryUI_Mito : MonoBehaviour
{
    public Inventory_Mito inventory;
    //public Text adenineCntText;
    //public Text riboseCntText;
    //public Text phosphateCntText;
    //public Text adpCntText;
    public Text itemText;

    void Start()
    {
        ClearCurrentItemText();
    }

    // 감지된 아이템의 정보를 출력
    public void UpdateCurrentItemText(Grabbable item)
    {
        Item_Mito itemMito = item.GetComponent<Item_Mito>();
        if (itemMito != null)
        {
            itemText.text = $"{itemMito.GetItemTypeName()}\n{GetItemCount(itemMito.type)}";
        }
    }

    // 인벤토리 텍스트 초기화
    public void ClearCurrentItemText()
    {
        itemText.text = "";
    }

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
}
