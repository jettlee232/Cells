using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager_MT : MonoBehaviour
{
    public GameObject vrPlayerPrefab;
    public Vector3 spawnPos;

    void Start()
    {
        StartCoroutine(replaceStart());
    }

    IEnumerator replaceStart()
    {
        yield return new WaitForSeconds(1f);
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.Instantiate(vrPlayerPrefab.name, spawnPos, Quaternion.identity);
        }
        else { yield return new WaitForSeconds(0.2f); }
    }
}
