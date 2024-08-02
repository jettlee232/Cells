using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager_Lys : MonoBehaviour
{
    public GameObject[] Enemy;
    public float curTime = 0f;
    public float coolTime = 2f;

    void Start()
    {
        RandomCoolTime();
    }

    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime > coolTime)
        {
            curTime = 0f;
            SpawnEnemy();
            RandomCoolTime();
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPos = Vector3.zero;
        if (Random.Range(0, 2) == 0) { spawnPos = new Vector3(Random.Range(-10f, 10f), 10f, 15f); }
        else { spawnPos = new Vector3(Random.Range(-10f, 10f), -10f, 15f); }
        int type = Random.Range(0, 3);

        Instantiate(Enemy[type], spawnPos, Quaternion.identity);
    }

    public void RandomCoolTime()
    {
        int rnd = Random.Range(1, 2);
        coolTime = rnd;
    }
}
