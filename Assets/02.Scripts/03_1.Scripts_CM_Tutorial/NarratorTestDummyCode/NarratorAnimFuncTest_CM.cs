using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorAnimFuncTest_CM : MonoBehaviour
{
    public GameObject saberSSPrefab;
    public Transform saberPrefabSpawnPos;


    public void MakeSaberSS()
    {
        GameObject go = Instantiate(saberSSPrefab);
        go.name = saberSSPrefab.name;
        go.transform.position = saberPrefabSpawnPos.transform.position;
        go.transform.rotation = Quaternion.identity;
    }
}
