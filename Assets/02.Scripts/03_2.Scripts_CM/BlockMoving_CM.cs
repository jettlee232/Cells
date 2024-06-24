using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMoving_CM : MonoBehaviour
{
    public float speed = 10f;
    Transform trns;

    void Start()
    {
        trns = GetComponent<Transform>();
        StartCoroutine(BlockMove());
    }

    IEnumerator BlockMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.02f);

            trns.Translate(Vector3.back * speed * Time.deltaTime);
        }
    }
}
