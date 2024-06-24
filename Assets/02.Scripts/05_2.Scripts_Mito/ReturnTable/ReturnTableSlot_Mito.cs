using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class ReturnTableSlot_Mito : MonoBehaviour
{
    // 슬롯 스크립트들은 추후에 상속으로 변경
    public ReturnTable_Mito returnTable;
    public ItemType slotType;
    public SnapZone snapZone;
    public Inventory_Mito inventory;

    private bool isHandlingEvent = false;

    void Start()
    {
        returnTable ??= GetComponentInParent<ReturnTable_Mito>();
        snapZone = GetComponent<SnapZone>();
        snapZone.OnSnapEvent.AddListener(OnItemSnapped);
        snapZone.OnDetachEvent.AddListener(OnItemDetached);
    }

    // 아이템이 슬롯에 붙을때의 이벤트
    void OnItemSnapped(Grabbable item)
    {
        if (isHandlingEvent) return;

        isHandlingEvent = true;

        if (item.GetComponent<Item_Mito>().type == slotType)
        {
            item.gameObject.SetActive(false);

            returnTable.UpdateSlotStatus(slotType, true);

            item.gameObject.SetActive(true);
        }
        else
        {
            snapZone.ReleaseAll();
            inventory.AddItem(item);
        }

        isHandlingEvent = false;
    }

    // 아이템이 슬롯에서 떨어질때의 이벤트
    void OnItemDetached(Grabbable item)
    {
        if (isHandlingEvent) return;

        isHandlingEvent = true;

        returnTable.UpdateSlotStatus(slotType, false);

        isHandlingEvent = false;
    }
}
