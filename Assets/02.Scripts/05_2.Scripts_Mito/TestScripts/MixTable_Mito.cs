using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class MixTable_Mito : MonoBehaviour
{
    public MixTableSlot_Mito adenineSlot; // 아데닌 슬롯 참조
    public MixTableSlot_Mito riboseSlot; // 리보스 슬롯 참조
    public MixTableSlot_Mito phosphateSlot_1; // 인산 슬롯1 참조
    public MixTableSlot_Mito phosphateSlot_2; // 인산 슬롯2 참조
    public MixTableSlot_Mito adpSlot; // ADP 슬롯 참조
    public MixTableSlot_Mito atpSlot; // ATP 슬롯 참조

    public bool isAdenine = false;
    public bool isRibose = false;
    public bool isPhosphate_1 = false;
    public bool isPhosphate_2 = false;
    public bool isADP = false;

    public GameObject adpPrefab;
    public GameObject atpPrefab;

    public int hIonCount = 0;
    public int maxHIonCount = 12;
    public int hIonPerATP = 3;

    private int requiredAdenine = 1;
    private int requiredRibose = 1;
    private int requiredPhosphate = 2;

    public void AddHIon(int amount)
    {
        hIonCount = Mathf.Clamp(hIonCount + amount, 0, maxHIonCount);
    }

    public bool UseHIon(int amount)
    {
        if (hIonCount >= amount)
        {
            hIonCount -= amount;
            return true;
        }
        return false;
    }

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
                isPhosphate_1 = phosphateSlot_1.snapZone.HeldItem != null;
                isPhosphate_2 = phosphateSlot_2.snapZone.HeldItem != null;
                break;
            case ItemType.ADP:
                isADP = status;
                break;
            case ItemType.H_Ion:
                AddHIon(1);
                break;
        }

        CheckADP();
        CheckATP();
    }

    void CheckADP()
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

    void CheckATP()
    {
        if (isADP && UseHIon(hIonPerATP))
        {
            MakeATP();
        }
    }

    void MakeATP()
    {
        if (atpSlot.snapZone.HeldItem == null)
        {
            RemoveAllItems();

            GameObject atpItem = Instantiate(atpPrefab);
            atpSlot.snapZone.GrabGrabbable(atpItem.GetComponent<Grabbable>());
        }
    }

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
        if (adpSlot.snapZone.HeldItem != null)
        {
            Destroy(adpSlot.snapZone.HeldItem.gameObject);
            isADP = false;
        }
    }
}
