using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item_Mito;

public class ReturnTable_Mito : MonoBehaviour
{
    public ReturnTableSlot_Mito atpSlot; // ATP ������ ���� ����
    public GameManager_Mito gameManager; // ���� ������ ���� ���� �Ŵ��� ����
    public int scoreIncrease = 10; // �ݳ��� ATP �� ���� �����ϴ� ����
    public float timeIncrease = 30.0f; // �ݳ��� ATP �� ���� �����ϴ� ���� �ð�
    
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
