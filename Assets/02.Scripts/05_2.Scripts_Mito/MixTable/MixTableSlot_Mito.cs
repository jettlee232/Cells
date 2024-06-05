using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class MixTableSlot_Mito : MonoBehaviour
{
    // ���� ��ũ��Ʈ���� ���Ŀ� ������� ����
    public ADPMixTable_Mito adpMixTable;
    public ATPMixTable_Mito atpMixTable;

    public ItemType slotType;
    public SnapZone snapZone;
    public Inventory_Mito inventory;

    // ���տ��� �÷��� ��� �Ǵµ�?
    private bool isHandlingEvent = false;

    void Start()
    {
        adpMixTable ??= GetComponentInParent<ADPMixTable_Mito>();
        atpMixTable ??= GetComponentInParent<ATPMixTable_Mito>();
        snapZone = GetComponent<SnapZone>();
        snapZone.OnSnapEvent.AddListener(OnItemSnapped);
        snapZone.OnDetachEvent.AddListener(OnItemDetached);
    }

    // �������� ���Կ� �������� �̺�Ʈ
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

    // �������� ���Կ��� ���������� �̺�Ʈ
    void OnItemDetached(Grabbable item)
    {
        if (isHandlingEvent) return;

        isHandlingEvent = true;

        adpMixTable?.UpdateSlotStatus(slotType, false);
        atpMixTable?.UpdateSlotStatus(slotType, false);

        isHandlingEvent = false;
    }
}
