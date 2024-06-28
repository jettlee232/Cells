using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class ReturnTable_Mito : MonoBehaviour
{
    public ReturnTableSlot_Mito atpSlot; // ATP ������ ���� ����
    public GameManager_Mito gameManager; // ���� ������ ���� ���� �Ŵ��� ����
    int scoreIncrease = 100; // �ݳ��� ATP �� ���� �����ϴ� ����
    float timeIncrease = 0.5f; // �ݳ��� ATP �� ���� �����ϴ� ���� �ð�
    
    // �������� ���� ����
    public bool isATP = false;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager_Mito").GetComponent<GameManager_Mito>();
    }

    // ������ ���¿� ���� ���� ���� ������Ʈ
    public void UpdateSlotStatus(ItemType itemType, bool status)
    {
        switch (itemType)
        {
            case ItemType.ATP:
                isATP = status;
                break;
        }

        CheckATP();
    }

    public void CheckATP()
    {
        if (isATP)
        {
            ReturnATP();
        }
    }

    void ReturnATP()
    {
        RemoveAllItems();

        gameManager.IncreaseScore(scoreIncrease);
        gameManager.IncreaseTime(timeIncrease);
    }

    // ATP �ݳ� �� �ʱ�ȭ
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
