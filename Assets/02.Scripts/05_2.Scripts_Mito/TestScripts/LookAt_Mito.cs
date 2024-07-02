using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt_Mito : MonoBehaviour
{
    Transform lookAt;
    void Start()
    {
        lookAt = Camera.main.transform;
    }

    void Update()
    {
        if (lookAt)
        {
            transform.LookAt(Camera.main.transform);
        }
        else if (Camera.main != null)
        {
            lookAt = Camera.main.transform;
        }
        else if (Camera.main == null)
        {
            return;
        }
    }
}
