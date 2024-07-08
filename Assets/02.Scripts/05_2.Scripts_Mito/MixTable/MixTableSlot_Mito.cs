using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class MixTableSlot_Mito : MonoBehaviour
{
    // ���� ��ũ��Ʈ���� ���Ŀ� ������� ����
    public ADPMixTable_Mito adpMixTable;
    public ATPMixTableInside_Mito atpMixTableInside;
    public ATPMixTableOutside_Mito atpMixTableOutside;

    public ItemType slotType;
    public SnapZone snapZone;
    public Inventory_Mito inventory;

    // ���տ��� �÷��� ��� �Ǵµ�?
    private bool isHandlingEvent = false;

    void Start()
    {
        adpMixTable ??= GetComponentInParent<ADPMixTable_Mito>();
        atpMixTableInside ??= GetComponentInParent<ATPMixTableInside_Mito>();
        atpMixTableOutside ??= GetComponentInParent<ATPMixTableOutside_Mito>();
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
            item.gameObject.SetActive(false);

            adpMixTable?.UpdateSlotStatus(slotType, true);
            atpMixTableInside?.UpdateSlotStatus(slotType, true);
            atpMixTableOutside?.AddHIon(1);

            item.gameObject.SetActive(true);
        }
        else
        {
            snapZone.ReleaseAll();
            inventory.AddItem(item);
        }

        isHandlingEvent = false;
    }

    // ���� ������ ���� �����غ��� ���� ���Կ����� ���� �̺�Ʈ�� ��� ���� �ʳ�?
    // �������� ���Կ��� ���������� �̺�Ʈ
    void OnItemDetached(Grabbable item)
    {
        if (isHandlingEvent) return;

        isHandlingEvent = true;

        adpMixTable?.UpdateSlotStatus(slotType, false);
        atpMixTableInside?.UpdateSlotStatus(slotType, false);

        isHandlingEvent = false;
    }
}
