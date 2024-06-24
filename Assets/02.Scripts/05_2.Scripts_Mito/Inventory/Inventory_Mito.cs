using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class Inventory_Mito : MonoBehaviour
{
    // 각각의 아이템들을 담아놓은 리스트
    public List<Grabbable> adenineItems = new List<Grabbable>();
    public List<Grabbable> riboseItems = new List<Grabbable>();
    public List<Grabbable> phosphateItems = new List<Grabbable>();
    public List<Grabbable> adpItems = new List<Grabbable>();
    public List<Grabbable> atpItems = new List<Grabbable>();
    public List<Grabbable> hIonItems = new List<Grabbable>();

    public InventorySlot_Mito adenineSlot; // 아데닌 슬롯 참조
    public InventorySlot_Mito riboseSlot; // 리보스 슬롯 참조
    public InventorySlot_Mito phosphateSlot; // 인산염 슬롯 참조
    public InventorySlot_Mito adpSlot; // ADP 슬롯 참조
    public InventorySlot_Mito atpSlot; // ATP 슬롯 참조
    public InventorySlot_Mito hIonSlot; // 수소 이온 슬롯 참조

    public void AddItem(Grabbable item)
    {
        if (item.GetComponent<Item_Mito>().isInventory) return;

        // 우선 아이템이 들어오면 꺼놓음
        item.gameObject.SetActive(false);

        // 아이템의 종류에 따라 분류
        switch (item.GetComponent<Item_Mito>().type)
        {
            // 아데닌
            case ItemType.Adenine:
                // 해당 아이템 리스트에 추가
                adenineItems.Add(item);
                // 2개째부터는 일단 꺼놓음
                if (adenineItems.Count > 1)
                    item.gameObject.SetActive(false);
                // 해당 슬롯에 스냅
                else
                    SnapItemToSlot(item, adenineSlot);
                break;
            // 리보스
            case ItemType.Ribose:
                riboseItems.Add(item);
                if (riboseItems.Count > 1)
                    item.gameObject.SetActive(false);
                else
                    SnapItemToSlot(item, riboseSlot);
                break;
            // 인산염
            case ItemType.Phosphate:
                phosphateItems.Add(item);
                if (phosphateItems.Count > 1)
                    item.gameObject.SetActive(false);
                else
                    SnapItemToSlot(item, phosphateSlot);
                break;
            // ADP
            case ItemType.ADP:
                adpItems.Add(item);
                if (adpItems.Count > 1)
                    item.gameObject.SetActive(false);
                else
                    SnapItemToSlot(item, adpSlot);
                break;
            // ATP
            case ItemType.ATP:
                atpItems.Add(item);
                if (atpItems.Count > 1)
                    item.gameObject.SetActive(false);
                else
                    SnapItemToSlot(item, atpSlot);
                break;
            case ItemType.H_Ion:
                hIonItems.Add(item);
                if (hIonItems.Count > 1)
                    item.gameObject.SetActive(false);
                else
                    SnapItemToSlot(item, hIonSlot);
                break;
        }

        item.GetComponent<Item_Mito>().isInventory = true;
    }

    public void RemoveItem(Grabbable item)
    {
        if (!item.GetComponent<Item_Mito>().isInventory) return;

        // 아이템 종류따라 구분
        switch (item.GetComponent<Item_Mito>().type)
        {
            case ItemType.Adenine:
                StartCoroutine(RemoveAndSnapRemainingItems(adenineItems, adenineSlot, item));
                break;
            case ItemType.Ribose:
                StartCoroutine(RemoveAndSnapRemainingItems(riboseItems, riboseSlot, item));
                break;
            case ItemType.Phosphate:
                StartCoroutine(RemoveAndSnapRemainingItems(phosphateItems, phosphateSlot, item));
                break;
            case ItemType.ADP:
                StartCoroutine(RemoveAndSnapRemainingItems(adpItems, adpSlot, item));
                break;
            case ItemType.ATP:
                StartCoroutine(RemoveAndSnapRemainingItems(atpItems, atpSlot, item));
                break;
            case ItemType.H_Ion:
                StartCoroutine(RemoveAndSnapRemainingItems(hIonItems, hIonSlot, item));
                break;
        }

    }

    private IEnumerator RemoveAndSnapRemainingItems(List<Grabbable> items, InventorySlot_Mito slot, Grabbable itemToRemove)
    {
        // 리스트에서 제거되는걸 확실하게 기다렸다가 실행
        yield return items.Remove(itemToRemove);
        yield return itemToRemove.GetComponent<Item_Mito>().isInventory = false;

        if (items.Count > 0)
        {
            SnapItemToSlot(items[0], slot);
        }
    }
    
    // 슬롯에 아이템 스냅
    private void SnapItemToSlot(Grabbable item, InventorySlot_Mito slot)
    {
        slot.ToggleIsHandlingEvent();
        item.gameObject.SetActive(true);
        slot.snapZone.GrabGrabbable(item);
        slot.ToggleIsHandlingEvent();
    }
}