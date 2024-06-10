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
        { ItemType.Adenine, "아데닌" },
        { ItemType.Ribose, "리보스" },
        { ItemType.Phosphate, "인산" },
        { ItemType.ADP, "ADP" },
        { ItemType.ATP, "ATP" },
        { ItemType.H_Ion, "수소이온" },
        { ItemType.FREE, "자유" }
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
        return "알 수 없음";
    }
}
