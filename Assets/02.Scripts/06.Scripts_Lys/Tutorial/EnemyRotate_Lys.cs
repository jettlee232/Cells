using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotate_Lys : MonoBehaviour
{
    private float rotationSpeed;

    private void OnEnable()
    {
        rotationSpeed = GameManager_Lys.instance.GetRotateSpeed();
        StartCoroutine(cRotate());
    }

    IEnumerator cRotate()
    {
        while(true)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
            yield return null;
        }
    }
}
