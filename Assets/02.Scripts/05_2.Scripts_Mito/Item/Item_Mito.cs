using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Mito : MonoBehaviour
{
    public float moveSpeed;
    public float rotSpeed;
    public float destroyTime = 60.0f;

    public enum ItemType
    {
        Adenine,
        Ribose,
        Phosphate,
        ADP,
        ATP,
        H_Ion,
        FREE
    }

    public ItemType type;

    private static readonly Dictionary<ItemType, string> itemTypeNames = new Dictionary<ItemType, string>
    {
        { ItemType.Adenine, "�Ƶ���" },
        { ItemType.Ribose, "������" },
        { ItemType.Phosphate, "�λ�" },
        { ItemType.ADP, "ADP" },
        { ItemType.ATP, "ATP" },
        { ItemType.H_Ion, "�����̿�" },
        { ItemType.FREE, "����" }
    };

    void Start()
    {
        //Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        
    }

    public string GetItemTypeName()
    {
        if (itemTypeNames.TryGetValue(type, out string name))
        {
            return name;
        }
        return "�� �� ����";
    }
}
