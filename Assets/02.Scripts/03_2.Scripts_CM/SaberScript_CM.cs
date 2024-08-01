using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
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

    public BNG.Grabbable grabbable;

    [Header("Audio Manager_Singleton")]
    public AudioMgr_CM audioMgr;

    void Start()
    {
        int curSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (curSceneIndex == 3)
            tutoMgr = GameObject.FindGameObjectWithTag("GameController").GetComponent<TutorialManager_CM>();
        if (curSceneIndex == 4)
            gameMgr = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager_CM>();

        playerTrns = GameObject.FindGameObjectWithTag("MainCamera").transform;

        audioMgr = AudioMgr_CM.Instance; // Not Yet - After Merging or Something Next Time

        prevPos = transform.position;
        StartCoroutine(CheckSpeed());

        if (grabbable != null)
        {
            StartCoroutine(CheckGrab());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canHit)
        {
            if (other.gameObject.tag == rightTagName)
            {
                if (other.GetComponent<BlockDestroy_CM>().ReturnFlag() == true)
                {
                    other.GetComponent<BlockDestroy_CM>().UnDestoryableBySaber();
                    other.GetComponent<BlockDestroy_CM>().DestroyedByOther();

                    if (gameMgr != null)
                    {
                        gameMgr.Scoreup();
                        gameMgr.BlueFade();
                    }
                    else if (gameMgr == null)
                    {
                        tutoMgr.CorrectAnswer(other.transform.parent.gameObject);
                    }

                    GameObject go = Instantiate(rightAnsTextEffect, other.gameObject.transform.position, Quaternion.identity);
                    Vector3 oppositeDirection = go.transform.position - playerTrns.position;
                    go.transform.rotation = Quaternion.LookRotation(oppositeDirection);

                    audioMgr.PlaySFXByInt(10); // SFX Code - Test Lately

                    Destroy(other.transform.parent.gameObject);
                }
            }
            else if (other.gameObject.tag == wrongTagName)
            {
                if (other.GetComponent<BlockDestroy_CM>().ReturnFlag() == true)
                {
                    other.GetComponent<BlockDestroy_CM>().UnDestoryableBySaber();
                    other.GetComponent<BlockDestroy_CM>().DestroyedByOther();

                    if (gameMgr != null)
                    {
                        gameMgr.ScoreDown(50);
                        gameMgr.RedFade();
                    }
                    else if (gameMgr == null)
                    {
                        tutoMgr.WrongAnswer(other.transform.parent.gameObject);
                    }

                    GameObject go = Instantiate(wrongAnsTextEffect, other.gameObject.transform.position, Quaternion.identity);
                    Vector3 oppositeDirection = go.transform.position - playerTrns.position;
                    go.transform.rotation = Quaternion.LookRotation(oppositeDirection);

                    audioMgr.PlaySFXByInt(11); // SFX Code - Test Lately

                    Destroy(other.transform.parent.gameObject);
                }
            }
            else if (other.gameObject.name == "RestartBlock")
            {
                if (gameMgr != null) gameMgr.GameRestart();
            }
            else if (other.gameObject.name == "ExitBlock")
            {
                if (gameMgr != null) gameMgr.DoLoadNewScene();
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

    IEnumerator CheckGrab()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.25f);
            if (grabbable.BeingHeld == true)
            {
                tutoMgr.PhosGrabCnt();
                break;
            }
        }
    }
}
