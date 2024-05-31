using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class MixTableSlot_Mito : MonoBehaviour
{
    ADPMixTable_Mito mixTable;
    public ItemType slotType;
    public SnapZone snapZone;

    private bool isHandlingEvent = false;

    void Start()
    {
        mixTable = GetComponentInParent<ADPMixTable_Mito>();
        snapZone = GetComponent<SnapZone>();
        snapZone.OnSnapEvent.AddListener(OnItemSnapped);
        snapZone.OnDetachEvent.AddListener(OnItemDetached);
    }

    void OnItemSnapped(Grabbable item)
    {
        if (isHandlingEvent) return;

        isHandlingEvent = true;

        if (item.GetComponent<Item_Mito>().type == slotType)
        {
            mixTable.AddItem(item, this);
        }
        else
        {
            snapZone.ReleaseAll();
        }

        isHandlingEvent = false;
    }

    void OnItemDetached(Grabbable item)
    {
        if (isHandlingEvent) return;

        isHandlingEvent = true;

        mixTable.RemoveItem(item, this);

        isHandlingEvent = false;
    }
}
