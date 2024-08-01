using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class CutsceneHand_Home : MonoBehaviour
{
    public float totalTime;
    public GameObject temp1;
    //public GameObject temp2;

    void Start()
    {
        Invoke("FadeOutAndMoveScene", totalTime / 60f);
    }

    public void FadeOutAndMoveScene()
    {
        StartCoroutine(cHandMove());
    }

    IEnumerator cHandMove()
    {
        while (true)
        {
            //temp2.transform.position = temp1.transform.position;
            //temp2.transform.rotation = temp1.transform.rotation;
            yield return null;
        }
    }
}
