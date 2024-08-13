using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class InventorySlot_Mito : MonoBehaviour
{
    // ���� ��ũ��Ʈ���� ���Ŀ� ������� ����
    Inventory_Mito inventory;
    public ItemType slotType;
    public SnapZone snapZone;

    private bool isHandlingEvent = false;

    void Start()
    {
        inventory = GetComponentInParent<Inventory_Mito>();
        snapZone = GetComponent<SnapZone>();
        snapZone.OnSnapEvent.AddListener(OnItemSnapped);
        snapZone.OnDetachEvent.AddListener(OnItemDetached);
    }

    // �������� ���Կ� �������� �̺�Ʈ
    void OnItemSnapped(Grabbable item)
    {
        if (isHandlingEvent) return;

        isHandlingEvent = true;

        Debug.Log($"OnItemSnapped called for item: {item.name} in slot: {slotType}");

        // �������� ��ġ�ϴ� ���Կ� ������
        if (item.GetComponent<Item_Mito>().type == slotType)
        {
            inventory.AddItem(item);
        }
        // �������� �ٸ� ���Կ� ������
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

        inventory.RemoveItem(item);

        isHandlingEvent = false;
    }

    // �̺�Ʈ �ߺ� ȣ�� ���� �÷���
    public void ToggleIsHandlingEvent()
    {
        isHandlingEvent = !isHandlingEvent;
    }
}
