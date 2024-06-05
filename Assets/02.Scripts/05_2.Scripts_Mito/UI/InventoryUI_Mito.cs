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

    // ������ �������� ������ ���
    public void UpdateCurrentItemText(Grabbable item)
    {
        Item_Mito itemMito = item.GetComponent<Item_Mito>();
        if (itemMito != null)
        {
            itemText.text = $"{itemMito.GetItemTypeName()}\n{GetItemCount(itemMito.type)}";
        }
    }

    // �κ��丮 �ؽ�Ʈ �ʱ�ȭ
    public void ClearCurrentItemText()
    {
        itemText.text = "";
    }

    // �κ��丮�� ������ ���� ��������
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
