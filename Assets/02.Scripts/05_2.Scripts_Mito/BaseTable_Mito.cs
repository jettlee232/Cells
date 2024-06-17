using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class BaseTable_Mito : MonoBehaviour
{
    // �� ���̺� ���� ���¸� �����ϱ� ���� ����Ʈ
    public List<bool> slotStatuses = new List<bool>();

    // ���� ���� �ʱ�ȭ (���̺��� ���� ������ �ٸ� �� �����Ƿ� virtual �޼���� ����)
    protected virtual void Start()
    {
        InitializeSlots(3); // �⺻������ 3���� ������ �ʱ�ȭ
    }

    // ���� �ʱ�ȭ �޼���
    protected void InitializeSlots(int slotCount)
    {
        slotStatuses = new List<bool>(new bool[slotCount]);
    }

    // ���� ���� ������Ʈ �޼���
    public virtual void UpdateSlotStatus(int slotIndex, bool status)
    {
        if (slotIndex >= 0 && slotIndex < slotStatuses.Count)
        {
            slotStatuses[slotIndex] = status;
        }
    }
}
