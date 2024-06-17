using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_CM : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DestroyItSelf());
    }

    IEnumerator DestroyItSelf()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
