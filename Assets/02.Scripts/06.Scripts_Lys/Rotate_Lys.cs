using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Lys : MonoBehaviour
{
    public float rotationSpeed = 50f;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}
