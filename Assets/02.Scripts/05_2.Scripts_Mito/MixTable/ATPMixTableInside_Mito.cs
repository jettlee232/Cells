using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class ATPMixTableInside_Mito : MonoBehaviour
{
    public ATPSynthaseInActive_Mito synth;
    public ATPMixTableOutside_Mito atpMixTableOutside;

    // 각 아이템의 슬롯 참조
    public MixTableSlot_Mito adpSlot;
    public MixTableSlot_Mito phosphateSlot;
    public MixTableSlot_Mito atpSlot;

    // 아이템의 유무 변수
    public bool isADP = false;
    public bool isPhosphate = false;

    public GameObject atpPrefab;

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
        }

    }

    public void CheckATP()
    {
        if (isADP && isPhosphate && atpMixTableOutside.UseHIon(hIonPerATP))
        {
            MakeATP();
        }
    }

    void MakeATP()
    {
        if (atpSlot.snapZone.HeldItem == null)
        {
            RemoveAllItems();

            GameObject atpItem = Instantiate(atpPrefab);
            atpItem.GetComponent<Item_Mito>().SetOriginalScale(atpPrefab.transform.localScale);
            atpSlot.snapZone.GrabGrabbable(atpItem.GetComponent<Grabbable>());
            // ATP 생성 효과 추가

            synth.OnATPCreated();
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
