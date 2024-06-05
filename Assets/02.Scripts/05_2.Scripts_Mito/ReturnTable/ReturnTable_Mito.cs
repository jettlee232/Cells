using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class ReturnTable_Mito : MonoBehaviour
{
    public ReturnTableSlot_Mito atpSlot; // ATP 아이템 슬롯 참조
    public GameManager_Mito gameManager; // 점수 관리를 위한 게임 매니저 참조
    public int scoreIncrease = 10; // 반납한 ATP 한 개당 증가하는 점수
    public float timeIncrease = 30.0f; // 반납한 ATP 한 개당 증가하는 제한 시간
    
    // 아이템의 유무 변수
    public bool isATP = false;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager_Mito").GetComponent<GameManager_Mito>();
    }

    // 슬롯의 상태에 따라 유무 변수 업데이트
    public void UpdateSlotStatus(ItemType itemType, bool status)
    {
        switch (itemType)
        {
            case ItemType.ATP:
                isATP = status;
                break;
        }

        //CheckATP();
    }

    public void CheckATP()
    {
        if (isATP)
        {
            MakeADP();
        }
    }

    void MakeADP()
    {
        RemoveAllItems();

        gameManager.IncreaseScore(scoreIncrease);
        gameManager.IncreaseTime(timeIncrease);
    }

    // ATP 반납 후 초기화
    private void RemoveAllItems()
    {
        if (atpSlot.snapZone.HeldItem != null)
        {
            Destroy(atpSlot.snapZone.HeldItem.gameObject);
            //atpSlot.snapZone.HeldItem.gameObject.SetActive(false);
            isATP = false;
        }
    }
}
