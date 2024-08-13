using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class InventorySlot_Mito : MonoBehaviour
{
    // 슬롯 스크립트들은 추후에 상속으로 변경
    Inventory_Mito inventory;
    public ItemType slotType;
    public SnapZone snapZone;

    private bool isHandlingEvent = false;

    void Start()
    {
        inventory = GetComponentInParent<Inventory_Mito>();
        snapZone = GetComponent<SnapZone>();
        snapZone.OnSnapEvent.AddListener(OnItemSnapped);
        snapZone.OnDetachEvent.AddListener(OnItemDetached);
    }

    // 아이템이 슬롯에 붙을때의 이벤트
    void OnItemSnapped(Grabbable item)
    {
        if (isHandlingEvent) return;

        isHandlingEvent = true;

        Debug.Log($"OnItemSnapped called for item: {item.name} in slot: {slotType}");

        // 아이템을 일치하는 슬롯에 넣을때
        if (item.GetComponent<Item_Mito>().type == slotType)
        {
            inventory.AddItem(item);
        }
        // 아이템을 다른 슬롯에 넣을때
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

        inventory.RemoveItem(item);

        isHandlingEvent = false;
    }

    // 이벤트 중복 호출 방지 플래그
    public void ToggleIsHandlingEvent()
    {
        isHandlingEvent = !isHandlingEvent;
    }
}
