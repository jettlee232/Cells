using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SaberScript_CM : MonoBehaviour
{
    public bool canHit = false;

    private Vector3 prevPos;  
    public float speedThreshold = 1.0f;  

    public LayerMask layer;

    [Header("Game Variable")]
    public TutorialManager_CM tutoMgr;
    public GameManager_CM gameMgr;
    public GameObject rightAnsTextEffect;
    public GameObject wrongAnsTextEffect;

    [Header("HitObjectTag")]
    public string rightTagName;
    public string wrongTagName;

    void Start()
    {
        if (GameObject.Find("TutorialMgr") == true) tutoMgr = GameObject.Find("TutorialMgr").GetComponent<TutorialManager_CM>();
        if (GameObject.Find("GameMgr") == true) gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager_CM>();

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
                if (gameMgr != null) // 게임매니저가 있는 게임 화면일 경우
                {
                    gameMgr.Scoreup();                    
                }
                else if (gameMgr == null)// 게임매니저가 없는 튜토리얼 화면일 경우
                {
                    tutoMgr.CorrectAnswer(other.transform.parent.gameObject);
                }

                other.gameObject.GetComponent<BlockDestroy_CM>().destroyFlag = true;
                Instantiate(rightAnsTextEffect, other.gameObject.transform.position, Quaternion.identity);
                Destroy(other.transform.parent.gameObject);
            }
            else if (other.gameObject.tag == wrongTagName)
            {
                if (gameMgr != null)
                {
                    gameMgr.ScoreDown();                    
                }
                else if (gameMgr == null)
                {
                    tutoMgr.WrongAnswer(other.transform.parent.gameObject);
                }

                other.gameObject.GetComponent<BlockDestroy_CM>().destroyFlag = true;
                Instantiate(wrongAnsTextEffect, other.gameObject.transform.position, Quaternion.identity);
                Destroy(other.transform.parent.gameObject);
            }
            else if (other.gameObject.name == "RestartBlock")
            {
                if (gameMgr != null) gameMgr.GameRestart(); // 게임 재시작
            }
            else if (other.gameObject.name == "ExitBlock")
            {
                if (gameMgr != null) SceneManager.LoadScene(2);
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
