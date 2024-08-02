using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotate_Lys_Game : MonoBehaviour
{
    private float rotationSpeed;

    private void OnEnable()
    {
        rotationSpeed = GameManager_Lys_Game.instance.GetRotateSpeed();
        StartCoroutine(cRotate());
    }

    IEnumerator cRotate()
    {
        while (true)
        {
            transform.Rotate(transform.up, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
