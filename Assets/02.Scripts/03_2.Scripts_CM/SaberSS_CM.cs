using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaberSS_CM : MonoBehaviour
{
    public GameManager_CM gameMgr;

    public LayerMask layerMask;
    private Vector3 previousPos;

    public GameObject rightAnsTextEffect;
    public GameObject wrongAnsTextEffect;

    void Start()
    {

    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1, layerMask))
        {
            if (hit.collider.CompareTag("SSBlock"))
            {
                gameMgr.Scoreup();
                Destroy(hit.collider.gameObject);

                // 효과는 여기에다
                Instantiate(rightAnsTextEffect, hit.collider.gameObject.transform.position, Quaternion.identity);
            }
            else if (hit.collider.CompareTag("WFBlock"))
            {
                gameMgr.ScoreDown();
                Destroy(hit.collider.gameObject);

                // 효과는 여기에다
                Instantiate(wrongAnsTextEffect, hit.collider.gameObject.transform.position, Quaternion.identity);
            }
            else if (hit.collider.CompareTag("RestartBlock"))
            {
                gameMgr.GameRestart(); // 게임 재시작
            }
            else if (hit.collider.CompareTag("ExitBlock"))
            {
                //SceneManager.LoadScene(2);
            }
        }
        //previousPos = transform.position;
    }
}
