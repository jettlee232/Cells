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
    public List<Grabbable> atpItems = new List<Grabbable>();

    public InventorySlot_Mito adenineSlot; // �Ƶ��� ���� ����
    public InventorySlot_Mito riboseSlot; // ������ ���� ����
    public InventorySlot_Mito phosphateSlot; // �λ� ���� ����
    public InventorySlot_Mito atpSlot; // ATP ���� ����

    public void AddItem(Grabbable item)
    {
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
            // �λ�
            case ItemType.Phosphate:
                phosphateItems.Add(item);
                if (phosphateItems.Count > 1)
                    item.gameObject.SetActive(false);
                else
                    SnapItemToSlot(item, phosphateSlot);
                break;
            // ATP
            case ItemType.ATP:
                atpItems.Add(item);
                if (atpItems.Count > 1)
                    item.gameObject.SetActive(false);
                else
                    SnapItemToSlot(item, atpSlot);
                break;
        }

    }

    public void RemoveItem(Grabbable item)
    {
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
            case ItemType.ATP:
                StartCoroutine(RemoveAndSnapRemainingItems(atpItems, atpSlot, item));
                break;
        }

    }

    private IEnumerator RemoveAndSnapRemainingItems(List<Grabbable> items, InventorySlot_Mito slot, Grabbable itemToRemove)
    {
        yield return items.Remove(itemToRemove);

        if (items.Count > 0)
        {
            SnapItemToSlot(items[0], slot);
        }
    }

    private void SnapItemToSlot(Grabbable item, InventorySlot_Mito slot)
    {
        slot.ToggleIsHandlingEvent();
        item.gameObject.SetActive(true);
        slot.snapZone.GrabGrabbable(item);
        slot.ToggleIsHandlingEvent();
    }

}