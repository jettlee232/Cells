using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BlockDestroy_CM : MonoBehaviour
{
    public GameObject particle;
    public bool destroyFlag = false;

    private void OnDestroy()
    {
        if (destroyFlag == true) Instantiate(particle, transform.position, transform.rotation);
    }
}
