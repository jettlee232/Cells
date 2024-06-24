using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class ATPMixTableOutside_Mito : MonoBehaviour
{
    // �� �������� ���� ����
    public MixTableSlot_Mito hIonSlot;

    public int curHIonCount = 0;
    public int maxHIonCount = 12;

    // ���� �����̿� ������ ����
    public void AddHIon(int amount)
    {
        curHIonCount = Mathf.Clamp(curHIonCount + amount, 0, maxHIonCount);
        // �����̿� 12�� �̻� �������� ó�� �����ʿ�
        Destroy(hIonSlot.snapZone.HeldItem.gameObject);
        //hIonSlot.snapZone.HeldItem.gameObject.SetActive(false);
        hIonSlot.snapZone.ReleaseAll();
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
}
