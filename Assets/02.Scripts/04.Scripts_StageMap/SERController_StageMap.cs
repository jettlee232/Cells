using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SERController_StageMap : MonoBehaviour
{
    void Start()
    {
        foreach (Transform child in transform)
        {
            Collider[] colliders = child.GetComponents<CapsuleCollider>();

            foreach (Collider collider in colliders)
            {
                if (collider != null) { gameObject.AddComponent<CapsuleCollider>(); }
                collider.gameObject.SetActive(false);
            }
        }
    }
}
