using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BlockDestroy_CM : MonoBehaviour
{
    public GameObject particle;

    private bool flag = true;
    public bool destroyedByOther = false;

    public void UnDestoryableBySaber()
    {
        flag = false;
    }

    public bool ReturnFlag()
    {
        return flag;
    }

    public void DestroyedByOther()
    {
        destroyedByOther = true;
    }

    private void OnDestroy()
    {
        if (destroyedByOther) Instantiate(particle, transform.position, transform.rotation);
    }
}
