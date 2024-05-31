using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class InventorySlot_Mito : MonoBehaviour
{
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

    void OnItemSnapped(Grabbable item)
    {
        if (isHandlingEvent) return;

        if (item.GetComponent<Item_Mito>().type == slotType)
        {
            isHandlingEvent = true;
            inventory.AddItem(item);
        }
        else
        {
            isHandlingEvent = true;
            snapZone.ReleaseAll();
            inventory.AddItem(item);
        }

        isHandlingEvent = false;
    }

    void OnItemDetached(Grabbable item)
    {
        if (isHandlingEvent) return;

        isHandlingEvent = true;

        inventory.RemoveItem(item);

        isHandlingEvent = false;
    }

    public void ToggleIsHandlingEvent()
    {
        isHandlingEvent = !isHandlingEvent;
    }
}
