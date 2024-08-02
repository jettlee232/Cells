using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBullet_Lys : MonoBehaviour
{
    public GameObject HitEffect;
    int enemyLayer;
    public float explosionRange = 6f;

    private void Start()
    {
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        Destroy(this, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(HitEffect, this.transform.position, Quaternion.identity);
            Collider[] colls = Physics.OverlapSphere(this.transform.position, explosionRange, enemyLayer);
            List<GameObject> npcs = new List<GameObject>();

            if (colls.Length > 0)
            {
                foreach (Collider coll in colls)
                {
                    coll.gameObject.GetComponent<EnemyController_Lys>().Die();
                }
            }
            Destroy(this);
        }
    }
}
