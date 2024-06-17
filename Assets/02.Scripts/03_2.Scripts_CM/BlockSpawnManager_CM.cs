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
        isGameRunning = true;
        while (isGameRunning)
        {
            yield return new WaitForSeconds(1.5f);

            int blockSpawnPosRnd = Random.Range(0, 12);
            int blockRnd1 = Random.Range(0, 2);
            int blockRnd2 = Random.Range(0, 6);

            GameObject go = null;

            if (blockRnd1 == 0)
            {
                go = Instantiate(blockWF[blockRnd2]);
            }
            else if (blockRnd1 == 1)
            {
                go = Instantiate(blockSS[blockRnd2]);
            }
            // 난이도가 높아짐, 일단은 보류
            /*
            else 
            {                
                go2 = Instantiate(blockSS[blockRnd2]);
                if (blockSpawnPosRnd < 2) go2.transform.position = blockSpawnPos[blockSpawnPosRnd + 1].position;
                else go2.transform.position = blockSpawnPos[blockSpawnPosRnd - 1].position;
                go2.transform.rotation = Quaternion.identity;

                int blockRnd3 = Random.Range(0, 6);
                go1 = Instantiate(blockWF[blockRnd3]);
            }
            */

            go.transform.position = blockSpawnPos[blockSpawnPosRnd].position;
            go.transform.rotation = Quaternion.identity;
            //go.transform.GetChild(2).gameObject.SetActive(false); // 3D 텍스트 오브젝트 감추기
            go.transform.SetParent(spawnedBlockGroup);
        }        

        // *** 이거 왜 있는 거였더라? 나중에 다시 테스트 ***
        for (int i = 0; i < spawnedBlockGroup.childCount; i++) 
        {
            Destroy(spawnedBlockGroup.GetChild(i).gameObject);
        }
    }
}
