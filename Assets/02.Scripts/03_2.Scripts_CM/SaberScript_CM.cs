using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaberScript_CM : MonoBehaviour
{
    public bool canHit = false;

    private Vector3 prevPos;
    private float speedThreshold = 1.25f;

    public LayerMask layer;

    [Header("Game Variable")]
    public TutorialManager_CM tutoMgr;
    public GameManager_CM gameMgr;
    public GameObject rightAnsTextEffect;
    public GameObject wrongAnsTextEffect;
    public Transform playerTrns;

    [Header("HitObjectTag")]
    public string rightTagName;
    public string wrongTagName;

    [Header("Hit Effect")]
    public GameObject philicHitEffect;
    public GameObject phobicHitEffect;

    void Start()
    {
        int curSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (curSceneIndex == 2 || curSceneIndex == 8) tutoMgr = GameObject.Find("TutorialMgr").GetComponent<TutorialManager_CM>();
        if (curSceneIndex == 3) gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager_CM>();
        playerTrns = GameObject.FindGameObjectWithTag("MainCamera").transform;

        prevPos = transform.position;
        StartCoroutine(CheckSpeed());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canHit)
        {
            if (other.gameObject.tag == rightTagName)
            {
                if (gameMgr != null)
                {
                    gameMgr.Scoreup();
                }
                else if (gameMgr == null)
                {
                    tutoMgr.CorrectAnswer(other.transform.parent.gameObject);
                }

                GameObject go = Instantiate(rightAnsTextEffect, other.gameObject.transform.position, Quaternion.identity);
                Vector3 oppositeDirection = go.transform.position - playerTrns.position;
                go.transform.rotation = Quaternion.LookRotation(oppositeDirection);

                other.gameObject.transform.GetComponent<BlockDestroy_CM>().flag = true;
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

                GameObject go = Instantiate(wrongAnsTextEffect, other.gameObject.transform.position, Quaternion.identity);
                Vector3 oppositeDirection = go.transform.position - playerTrns.position;
                go.transform.rotation = Quaternion.LookRotation(oppositeDirection);

                other.gameObject.transform.GetComponent<BlockDestroy_CM>().flag = true;
                Destroy(other.transform.parent.gameObject);
            }
            else if (other.gameObject.name == "RestartBlock")
            {
                if (gameMgr != null) gameMgr.GameRestart();
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
