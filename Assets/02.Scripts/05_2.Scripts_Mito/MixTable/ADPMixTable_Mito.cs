using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class ADPMixTable_Mito : MonoBehaviour
{
    // 각각의 아이템들을 담아놓은 리스트
    public List<Grabbable> adenineItems = new List<Grabbable>();
    public List<Grabbable> riboseItems = new List<Grabbable>();
    public List<Grabbable> phosphateItems = new List<Grabbable>();
    public List<Grabbable> atpItems = new List<Grabbable>();

    public MixTableSlot_Mito adenineSlot; // 아데닌 슬롯 참조
    public MixTableSlot_Mito riboseSlot; // 리보스 슬롯 참조
    public MixTableSlot_Mito phosphateSlot_1; // 인산 슬롯 참조
    public MixTableSlot_Mito phosphateSlot_2; // 인산 슬롯 참조
    public MixTableSlot_Mito adpSlot; // ADP 슬롯 참조

    public bool isAdenine = false;
    public bool isRibose = false;
    public bool isPhosphate_1 = false;
    public bool isPhosphate_2 = false;

    public GameObject adpPrefab;

    public void AddItem(Grabbable item, MixTableSlot_Mito slot)
    {
        switch (item.GetComponent<Item_Mito>().type)
        {
            case ItemType.Adenine:
                adenineItems.Add(item);
                break;
            case ItemType.Ribose:
                riboseItems.Add(item);
                break;
            case ItemType.Phosphate:
                phosphateItems.Add(item);
                break;
        }
        CheckForADP();
    }

    public void RemoveItem(Grabbable item, MixTableSlot_Mito slot)
    {
        switch (item.GetComponent<Item_Mito>().type)
        {
            case ItemType.Adenine:
                adenineItems.Remove(item);
                break;
            case ItemType.Ribose:
                riboseItems.Remove(item);
                break;
            case ItemType.Phosphate:
                phosphateItems.Remove(item);
                break;
        }
    }

    void CheckForADP()
    {
        if (adenineItems.Count >= 1 && riboseItems.Count >= 1 && phosphateItems.Count >= 2)
        {
            MakeADP();
        }
    }

    void MakeADP()
    {
        if (adpSlot.snapZone.HeldItem == null)
        {
            // Remove the required items from the slots
            RemoveAndDeactivateItem(adenineItems);
            RemoveAndDeactivateItem(riboseItems);
            RemoveAndDeactivateItem(phosphateItems);
            RemoveAndDeactivateItem(phosphateItems); // 두번째 인산 제거

            // Instantiate ADP and snap it to the ADP slot
            GameObject adpInstance = Instantiate(adpPrefab);
            SnapItemToSlot(adpInstance.GetComponent<Grabbable>(), adpSlot);

            adenineSlot.snapZone.ReleaseAll();
            riboseSlot.snapZone.ReleaseAll();
            phosphateSlot_1.snapZone.ReleaseAll();
            phosphateSlot_2.snapZone.ReleaseAll();
        }
    }

    void RemoveAndDeactivateItem(List<Grabbable> items)
    {
        if (items.Count > 0)
        {
            Grabbable item = items[0];
            item.gameObject.SetActive(false);
            items.RemoveAt(0);
        }
    }

    private void SnapItemToSlot(Grabbable item, MixTableSlot_Mito slot)
    {
        slot.snapZone.GrabGrabbable(item);
    }
}