using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Mito : MonoBehaviour
{
    public float moveSpeed;
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

    void Start()
    {
        //Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        
    }
}
