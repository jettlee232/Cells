using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockSpawnManager_CM : MonoBehaviour
{
    public GameObject[] blockWF;
    public GameObject[] blockSS;

    public Transform[] blockSpawnPos;

    public Transform spawnedBlockGroup;

    public bool isGameRunning = true;

    public void BlockSpawnStart()
    {
        StartCoroutine(MakeBlock());
    }

    public void BlockSpawnStop()
    {
        isGameRunning = false;
    }

    IEnumerator MakeBlock()
    {
        Debug.Log("Block Spawn Start!");
        isGameRunning = true;
        while (isGameRunning)
        {
            yield return new WaitForSeconds(1.5f);

            int blockSpawnPosRnd = Random.Range(0, 12);
            int blockRnd1 = Random.Range(0, 2);
            int blockRnd2 = Random.Range(0, 6);

            GameObject go1 = null;
            GameObject go2 = null;

            if (blockRnd1 == 0)
            {
                go1 = Instantiate(blockWF[blockRnd2]);
            }
            else if (blockRnd1 == 1)
            {
                go1 = Instantiate(blockSS[blockRnd2]);
            }
            else // 난이도가 높아짐, 일단은 보류
            {                
                go2 = Instantiate(blockSS[blockRnd2]);
                if (blockSpawnPosRnd < 2) go2.transform.position = blockSpawnPos[blockSpawnPosRnd + 1].position;
                else go2.transform.position = blockSpawnPos[blockSpawnPosRnd - 1].position;
                go2.transform.rotation = Quaternion.identity;

                int blockRnd3 = Random.Range(0, 6);
                go1 = Instantiate(blockWF[blockRnd3]);
            }

            go1.transform.position = blockSpawnPos[blockSpawnPosRnd].position;
            go1.transform.rotation = Quaternion.identity;
            go1.transform.SetParent(spawnedBlockGroup);
        }        

        for (int i = 0; i < spawnedBlockGroup.childCount; i++) 
        {
            Destroy(spawnedBlockGroup.GetChild(i).gameObject);
        }
    }
}
