using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBullet_Lys_Game : MonoBehaviour
{
    public GameObject HitEffect;

    private void Start()
    {
        Destroy(this, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(HitEffect, this.transform.position, Quaternion.identity);
            other.gameObject.GetComponent<EnemyCommon_Lys_Game>().Die();
            Destroy(this);
        }
    }
}
