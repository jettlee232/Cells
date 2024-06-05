using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Item_Mito;

public class ATPMixTable_Mito : MonoBehaviour
{
    // �� �������� ���� ����
    public MixTableSlot_Mito adpSlot;
    public MixTableSlot_Mito hIonSlot;
    public MixTableSlot_Mito atpSlot;

    // �������� ���� ����
    public bool isADP = false;

    public GameObject atpPrefab;

    public int curHIonCount = 0;
    public int maxHIonCount = 12;
    private int hIonPerATP = 3;

    // ���� �����̿� ������ ����
    public void AddHIon(int amount)
    {
        curHIonCount = Mathf.Clamp(curHIonCount + amount, 0, maxHIonCount);
        Destroy(hIonSlot.snapZone.HeldItem.gameObject);
        //hIonSlot.snapZone.HeldItem.gameObject.SetActive(false);
        hIonSlot.snapZone.ReleaseAll();
    }

    // ������ ���¿� ���� ���� ���� ������Ʈ
    public void UpdateSlotStatus(ItemType itemType, bool status)
    {
        switch (itemType)
        {
            case ItemType.ADP:
                isADP = status;
                break;
            case ItemType.H_Ion:
                AddHIon(1);
                break;
        }

        //CheckATP();
    }

    // ���� �����̿� �������� ���տ� �ʿ��� �����̿� ���� �̻��϶��� true
    public bool UseHIon(int amount)
    {
        if (curHIonCount >= amount)
        {
            curHIonCount -= amount;
            return true;
        }
        return false;
    }

    public void CheckATP()
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

            GameObject adpItem = Instantiate(atpPrefab);
            atpSlot.snapZone.GrabGrabbable(adpItem.GetComponent<Grabbable>());
        }
    }

    // ATP ���� �� �ʱ�ȭ
    private void RemoveAllItems()
    {
        if (adpSlot.snapZone.HeldItem != null)
        {
            Destroy(adpSlot.snapZone.HeldItem.gameObject);
            isADP = false;
        }
    }
}