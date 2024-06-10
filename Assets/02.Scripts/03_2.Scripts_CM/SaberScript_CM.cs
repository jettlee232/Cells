using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SaberScript_CM : MonoBehaviour
{
    public bool canHit = false;

    private Vector3 prevPos;  
    public float speedThreshold = 1.0f;  

    public LayerMask layer;

    [Header("Game Variable")]
    public GameManager_CM gameMgr;
    public GameObject rightAnsTextEffect;
    public GameObject wrongAnsTextEffect;

    [Header("HitObjectTag")]
    public string rightTagName;
    public string wrongTagName;

    void Start()
    {
        prevPos = transform.position;
        StartCoroutine(CheckSpeed());
    }

    /*
    void Update()
    {
        
    }
    */

    private void OnTriggerEnter(Collider other)
    {     
        if (canHit)
        {
            if (other.gameObject.tag == rightTagName)
            {
                gameMgr.Scoreup();
                Destroy(other.gameObject);

                Instantiate(rightAnsTextEffect, other.gameObject.transform.position, Quaternion.identity);
            }
            else if (other.gameObject.tag == wrongTagName)
            {
                gameMgr.ScoreDown();
                Destroy(other.gameObject);

                Instantiate(wrongAnsTextEffect, other.gameObject.transform.position, Quaternion.identity);
            }
            else if (other.gameObject.name == "RestartBlock")
            {
                gameMgr.GameRestart(); // 게임 재시작
            }
            else if (other.gameObject.name == "ExitBlock")
            {
                //SceneManager.LoadScene(2);
            }
        }        
    }

    IEnumerator CheckSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.02f);
            
            float speed = (transform.position - prevPos).magnitude / Time.deltaTime;
            
            if (speed >= speedThreshold) canHit = true;
            else canHit = false;

            prevPos = transform.position;
        }        
    }
}
