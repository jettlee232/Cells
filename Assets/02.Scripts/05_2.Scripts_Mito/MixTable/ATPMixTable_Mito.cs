using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Item_Mito;

public class ATPMixTable_Mito : MonoBehaviour
{
    // 각 아이템의 슬롯 참조
    public MixTableSlot_Mito adpSlot;
    public MixTableSlot_Mito phosphateSlot;
    public MixTableSlot_Mito hIonSlot;
    public MixTableSlot_Mito atpSlot;

    // 아이템의 유무 변수
    public bool isADP = false;
    public bool isPhosphate = false;

    public GameObject atpPrefab;

    public int curHIonCount = 0;
    public int maxHIonCount = 12;
    private int hIonPerATP = 3;

    // 슬롯의 상태에 따라 유무 변수 업데이트
    public void UpdateSlotStatus(ItemType itemType, bool status)
    {
        switch (itemType)
        {
            case ItemType.ADP:
                isADP = status;
                break;
            case ItemType.Phosphate:
                isPhosphate = status;
                break;
            case ItemType.H_Ion:
                AddHIon(1);
                break;
        }

        //CheckATP();
    }

    // 현재 수소이온 변수값 증가
    public void AddHIon(int amount)
    {
        curHIonCount = Mathf.Clamp(curHIonCount + amount, 0, maxHIonCount);
        // 수소이온 12개 이상 넣을때의 처리 수정필요
        Destroy(hIonSlot.snapZone.HeldItem.gameObject);
        //hIonSlot.snapZone.HeldItem.gameObject.SetActive(false);
        hIonSlot.snapZone.ReleaseAll();
    }

    // 현재 수소이온 변수값이 조합에 필요한 수소이온 개수 이상일때만 true
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
        if (isADP && isPhosphate && UseHIon(hIonPerATP))
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
            // ATP 생성 효과 추가
        }
    }

    // ATP 조합 후 초기화
    private void RemoveAllItems()
    {
        if (adpSlot.snapZone.HeldItem != null)
        {
            Destroy(adpSlot.snapZone.HeldItem.gameObject);
            isADP = false;
        }
        if (phosphateSlot.snapZone.HeldItem != null)
        {
            Destroy(phosphateSlot.snapZone.HeldItem.gameObject);
            isPhosphate = false;
        }
    }
}