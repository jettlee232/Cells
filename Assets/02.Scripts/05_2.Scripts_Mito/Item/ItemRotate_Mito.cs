using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotate_Mito : MonoBehaviour
{
    void Start()
    {
        float randomYRotation = Random.Range(0f, 360f);

        transform.rotation = Quaternion.Euler(-90.0f, randomYRotation, 0f);
    }
}
