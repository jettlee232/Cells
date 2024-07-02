using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDeletePlane_CM : MonoBehaviour
{
    public GameManager_CM gameMgr;

    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.gameObject.CompareTag("WFBlock") || other.gameObject.CompareTag("SSBlock"))
        {
            Destroy(other.transform.parent.gameObject);
            gameMgr.ScoreDown();
        }
        */

        if (other.gameObject.layer == LayerMask.NameToLayer("DescObj"))
        {
            Destroy(other.transform.parent.gameObject);
            gameMgr.ScoreDown();
        }
    }
}
