using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class ATPMixTableOutside_Mito : MonoBehaviour
{
    // 각 아이템의 슬롯 참조
    public MixTableSlot_Mito hIonSlot;

    public int curHIonCount = 0;
    public int maxHIonCount = 12;

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
}
