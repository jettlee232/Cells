using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI_Mito : MonoBehaviour
{
    public Inventory_Mito inventory;
    public Text adenineCntText;
    public Text riboseCntText;
    public Text phosphateCntText;
    public Text atpCntText;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        // �κ��丮 ����Ʈ�� ī��Ʈ�� UI�� �ݿ�
        adenineCntText.text = "�Ƶ���\n" + inventory.adenineItems.Count;
        riboseCntText.text = "������\n" + inventory.riboseItems.Count;
        phosphateCntText.text = "�λ�\n" + inventory.phosphateItems.Count;
        atpCntText.text = "ATP\n" + inventory.atpItems.Count;
    }
}
