using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class BaseTable_Mito : MonoBehaviour
{
    // 각 테이블 슬롯 상태를 관리하기 위한 리스트
    public List<bool> slotStatuses = new List<bool>();

    // 슬롯 상태 초기화 (테이블마다 슬롯 개수가 다를 수 있으므로 virtual 메서드로 설정)
    protected virtual void Start()
    {
        InitializeSlots(3); // 기본적으로 3개의 슬롯을 초기화
    }

    // 슬롯 초기화 메서드
    protected void InitializeSlots(int slotCount)
    {
        slotStatuses = new List<bool>(new bool[slotCount]);
    }

    // 슬롯 상태 업데이트 메서드
    public virtual void UpdateSlotStatus(int slotIndex, bool status)
    {
        if (slotIndex >= 0 && slotIndex < slotStatuses.Count)
        {
            slotStatuses[slotIndex] = status;
        }
    }
}
