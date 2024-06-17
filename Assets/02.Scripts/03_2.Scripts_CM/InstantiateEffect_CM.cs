using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateEffect_CM : MonoBehaviour
{
    [Header("Size Variable")]
    private Vector3 initSize;
    public Vector3 goalSize;

    [Header("Spin Variable")]
    public float initSpeed = 100f;
    public float minSpeed = 50f;
    public float decRate = 10f;
    private float curSpeed;

    private Coroutine spinCo;
    public void GoStart()
    {
        initSize = new Vector3(0.001f, 0.001f, 0.001f);
        curSpeed = initSpeed;

        StartCoroutine(SizeUp());
        StartCoroutine(Spin());
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

    IEnumerator Spin()
    {
        while (true)
        {
            transform.Rotate(Vector3.up, curSpeed * Time.deltaTime);
            
            if (curSpeed > minSpeed)
            {
                curSpeed -= decRate * Time.deltaTime;                
                if (curSpeed < minSpeed)
                {
                    curSpeed = minSpeed;
                }
            }
            yield return new WaitForSeconds(0.025f);
        }      
    }
}
