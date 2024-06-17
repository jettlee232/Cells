using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRotate_CM : MonoBehaviour
{
    public bool rotateFlag = true;
    public float speed;
    public float rate;

    void Start()
    {
        speed = 150f;
        rate = 0.02f;

        StartCoroutine(RotateObj());
    }

    IEnumerator RotateObj()
    {
        while (rotateFlag)
        {
            transform.Rotate(Vector3.up * speed * Time.deltaTime, Space.World);
            yield return new WaitForSeconds(rate);
        }        
    }
}
