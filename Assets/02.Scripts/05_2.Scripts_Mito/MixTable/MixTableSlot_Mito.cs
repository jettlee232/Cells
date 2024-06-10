using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class MixTableSlot_Mito : MonoBehaviour
{
    // 슬롯 스크립트들은 추후에 상속으로 변경
    public ADPMixTable_Mito adpMixTable;
    public ATPMixTable_Mito atpMixTable;

    public ItemType slotType;
    public SnapZone snapZone;
    public Inventory_Mito inventory;

    // 조합에는 플래그 없어도 되는듯?
    private bool isHandlingEvent = false;

    void Start()
    {
        adpMixTable ??= GetComponentInParent<ADPMixTable_Mito>();
        atpMixTable ??= GetComponentInParent<ATPMixTable_Mito>();
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
            adpMixTable?.UpdateSlotStatus(slotType, true);
            atpMixTable?.UpdateSlotStatus(slotType, true);
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

        adpMixTable?.UpdateSlotStatus(slotType, false);
        atpMixTable?.UpdateSlotStatus(slotType, false);

        isHandlingEvent = false;
    }
}
