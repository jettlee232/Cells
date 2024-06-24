using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateEffect_CM : MonoBehaviour
{
    [Header("Size Variable")]
    private Vector3 initSize;
    public Vector3 goalSize;

    public void GoStart()
    {
        initSize = new Vector3(0.001f, 0.001f, 0.001f);

        StartCoroutine(SizeUp());
    }

    IEnumerator SizeUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.002f);

            initSize.x += 0.001f;
            initSize.y += 0.001f;
            initSize.z += 0.001f;

            transform.localScale = initSize;

            if (initSize.x >= goalSize.x)
            {
                break;
            }
        }
    }
}
