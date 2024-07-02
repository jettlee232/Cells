using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class BaseSlot_Mito : MonoBehaviour
{
    public ItemType slotType;
    public SnapZone snapZone;
    public Inventory_Mito inventory;

    protected bool isHandlingEvent = false;

    protected virtual void Start()
    {
        snapZone = GetComponent<SnapZone>();
        snapZone.OnSnapEvent.AddListener(OnItemSnapped);
        snapZone.OnDetachEvent.AddListener(OnItemDetached);
    }

    // 아이템이 슬롯에 붙을때의 이벤트
    protected virtual void OnItemSnapped(Grabbable item)
    {
        if (isHandlingEvent) return;

        isHandlingEvent = true;

        if (item.GetComponent<Item_Mito>().type == slotType)
        {
            HandleCorrectItemSnapped(item);
        }
        else
        {
            snapZone.ReleaseAll();
            inventory.AddItem(item);
        }

        isHandlingEvent = false;
    }

    // 아이템이 슬롯에서 떨어질때의 이벤트
    protected virtual void OnItemDetached(Grabbable item)
    {
        if (isHandlingEvent) return;

        isHandlingEvent = true;

        HandleItemDetached(item);

        isHandlingEvent = false;
    }

    protected virtual void HandleCorrectItemSnapped(Grabbable item)
    {
        
    }

    protected virtual void HandleItemDetached(Grabbable item)
    {
        
    }
}
