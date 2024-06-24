using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class ADPMixTable_Mito : MonoBehaviour
{
    // 각 아이템의 슬롯 참조
    public MixTableSlot_Mito adenineSlot;
    public MixTableSlot_Mito riboseSlot;
    public MixTableSlot_Mito phosphateSlot_1;
    public MixTableSlot_Mito phosphateSlot_2;
    public MixTableSlot_Mito adpSlot;

    // 각 아이템의 유무 변수
    public bool isAdenine = false;
    public bool isRibose = false;
    public bool isPhosphate_1 = false;
    public bool isPhosphate_2 = false;

    public GameObject adpPrefab;

    // 슬롯의 상태에 따라 유무 변수 업데이트
    public void UpdateSlotStatus(ItemType itemType, bool status)
    {
        switch (itemType)
        {
            case ItemType.Adenine:
                isAdenine = status;
                break;
            case ItemType.Ribose:
                isRibose = status;
                break;
            case ItemType.Phosphate:
                UpdatePhosphateSlots();
                break;
        }
    }
    
    public void UpdatePhosphateSlots()
    {
        isPhosphate_1 = phosphateSlot_1.snapZone.HeldItem != null;
        isPhosphate_2 = phosphateSlot_2.snapZone.HeldItem != null;
    }

    public void CheckADP()
    {
        if (isAdenine && isRibose && isPhosphate_1 && isPhosphate_2)
        {
            MakeADP();
        }
    }

    void MakeADP()
    {
        if (adpSlot.snapZone.HeldItem == null)
        {
            RemoveAllItems();

            GameObject adpItem = Instantiate(adpPrefab);
            adpSlot.snapZone.GrabGrabbable(adpItem.GetComponent<Grabbable>());
        }
    }

    // ADP 조합 후 초기화
    private void RemoveAllItems()
    {
        if (adenineSlot.snapZone.HeldItem != null)
        {
            Destroy(adenineSlot.snapZone.HeldItem.gameObject);
            isAdenine = false;
        }
        if (riboseSlot.snapZone.HeldItem != null)
        {
            Destroy(riboseSlot.snapZone.HeldItem.gameObject);
            isRibose = false;
        }
        if (phosphateSlot_1.snapZone.HeldItem != null)
        {
            Destroy(phosphateSlot_1.snapZone.HeldItem.gameObject);
            isPhosphate_1 = false;
        }
        if (phosphateSlot_2.snapZone.HeldItem != null)
        {
            Destroy(phosphateSlot_2.snapZone.HeldItem.gameObject);
            isPhosphate_2 = false;
        }
    }
}