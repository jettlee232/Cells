using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberWFTutorial_CM : MonoBehaviour
{
    public TutorialManager_CM tutoMgr;

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
        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.25f, layerMask))
        {
            if (hit.collider.CompareTag("WFBlock"))
            {
                Debug.Log("����_WF");
                //tutoMgr.CorrectAnswer(hit.collider.gameObject);

                tutoMgr.CorrectAnswer(hit.collider.gameObject);
                Destroy(hit.collider.gameObject);

                // ȿ���� ���⿡��
                Instantiate(rightAnsTextEffect, hit.collider.gameObject.transform.position, Quaternion.Euler(0, hit.collider.gameObject.transform.rotation.y + 270f, 0));
            }
            else if (hit.collider.CompareTag("SSBlock"))
            {
                Debug.Log("����_WF");
                tutoMgr.WrongAnswer(hit.collider.gameObject);

                Destroy(hit.collider.gameObject);

                // ȿ���� ���⿡��
                Instantiate(wrongAnsTextEffect, hit.collider.gameObject.transform.position, Quaternion.Euler(0, hit.collider.gameObject.transform.rotation.y + 270f, 0));
            }
        }
        //previousPos = transform.position;
    }
}
