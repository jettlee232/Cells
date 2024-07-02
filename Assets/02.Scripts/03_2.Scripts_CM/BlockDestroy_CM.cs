using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BlockDestroy_CM : MonoBehaviour
{
    public GameObject particle;

    public bool flag = false;

    private void OnDestroy()
    {
        if (flag) Instantiate(particle, transform.position, transform.rotation);
    }
}
