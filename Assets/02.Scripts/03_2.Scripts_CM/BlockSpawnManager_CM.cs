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

    public bool isHardMode = false;
    public Material hardModeMat_Sphere;
    public Material hardModeMat_Text;

    private bool blockMadeFlag = false;

    public void BlockSpawnStart()
    {
        blockMadeFlag = true;
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
            if (isGameRunning == false) break;

            yield return new WaitForSeconds(1.5f);

            if (isGameRunning == false) break;

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
            go.transform.localScale = new Vector3(0.175f, 0.175f, 0.175f);
            //go.transform.GetChild(2).gameObject.SetActive(false);
            go.transform.SetParent(spawnedBlockGroup);

            if (isHardMode == true)
            {
                Debug.Log("Go Child 1 : " + go.transform.GetChild(1).name + " / 2 : " + go.transform.GetChild(2).name);
                go.transform.GetChild(1).GetComponent<MeshRenderer>().material = hardModeMat_Sphere;
                go.transform.GetChild(2).GetComponent<MeshRenderer>().material = hardModeMat_Text;
            }
        }
    }

    public void DestroyAllBlocks()
    {
        for (int i = 0; i < spawnedBlockGroup.childCount; i++)
        {
            Destroy(spawnedBlockGroup.GetChild(i).gameObject);
        }
    }
}
