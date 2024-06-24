using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class Inventory_Mito : MonoBehaviour
{
    // ������ �����۵��� ��Ƴ��� ����Ʈ
    public List<Grabbable> adenineItems = new List<Grabbable>();
    public List<Grabbable> riboseItems = new List<Grabbable>();
    public List<Grabbable> phosphateItems = new List<Grabbable>();
    public List<Grabbable> adpItems = new List<Grabbable>();
    public List<Grabbable> atpItems = new List<Grabbable>();
    public List<Grabbable> hIonItems = new List<Grabbable>();

    public InventorySlot_Mito adenineSlot; // �Ƶ��� ���� ����
    public InventorySlot_Mito riboseSlot; // ������ ���� ����
    public InventorySlot_Mito phosphateSlot; // �λ꿰 ���� ����
    public InventorySlot_Mito adpSlot; // ADP ���� ����
    public InventorySlot_Mito atpSlot; // ATP ���� ����
    public InventorySlot_Mito hIonSlot; // ���� �̿� ���� ����

    public void AddItem(Grabbable item)
    {
        if (item.GetComponent<Item_Mito>().isInventory) return;

        // �켱 �������� ������ ������
        item.gameObject.SetActive(false);

        // �������� ������ ���� �з�
        switch (item.GetComponent<Item_Mito>().type)
        {
            // �Ƶ���
            case ItemType.Adenine:
                // �ش� ������ ����Ʈ�� �߰�
                adenineItems.Add(item);
                // 2��°���ʹ� �ϴ� ������
                if (adenineItems.Count > 1)
                    item.gameObject.SetActive(false);
                // �ش� ���Կ� ����
                else
                    SnapItemToSlot(item, adenineSlot);
                break;
            // ������
            case ItemType.Ribose:
                riboseItems.Add(item);
                if (riboseItems.Count > 1)
                    item.gameObject.SetActive(false);
                else
                    SnapItemToSlot(item, riboseSlot);
                break;
            // �λ꿰
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

        // ������ �������� ����
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
        // ����Ʈ���� ���ŵǴ°� Ȯ���ϰ� ��ٷȴٰ� ����
        yield return items.Remove(itemToRemove);
        yield return itemToRemove.GetComponent<Item_Mito>().isInventory = false;

        if (items.Count > 0)
        {
            SnapItemToSlot(items[0], slot);
        }
    }
    
    // ���Կ� ������ ����
    private void SnapItemToSlot(Grabbable item, InventorySlot_Mito slot)
    {
        slot.ToggleIsHandlingEvent();
        item.gameObject.SetActive(true);
        slot.snapZone.GrabGrabbable(item);
        slot.ToggleIsHandlingEvent();
    }
}