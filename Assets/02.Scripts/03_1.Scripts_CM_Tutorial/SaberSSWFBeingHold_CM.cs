using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberSSWFBeingHold_CM : MonoBehaviour
{
    public GameObject parentObj;
    public Vector3 attachVec;
    public Quaternion attachRot;

    public bool isThisFirstAttach = false;
    public bool checkFlag = false;
    public GameObject attachPos;

    void Start()
    {
        parentObj = transform.parent.gameObject;
        attachPos = GameObject.Find("SingleAttachPos"); // 장착 지점을 받아오기
        attachVec = new Vector3(0.8f, -3.4f, 0f);
        attachRot = Quaternion.Euler(0, 180f, -180f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == attachPos && parentObj.GetComponent<BNG.Grabbable>().BeingHeld == true) // 닿은 오브젝트가 장착 지점이라면
        {
            Debug.Log("OnTrigger!!!");

            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().statusFlag = 2;
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().bc1.enabled = true;

            parentObj.transform.SetParent(other.gameObject.transform); // 닿은 오브젝트를 이 오브젝트의 부모로 만들고

            parentObj.GetComponent<BNG.Grabbable>().enabled = false;
            parentObj.GetComponent<Rigidbody>().useGravity = false;
            parentObj.GetComponent<Rigidbody>().isKinematic = false;
            parentObj.GetComponent<BoxCollider>().enabled = false;

            parentObj.transform.position = attachVec;
            parentObj.transform.rotation = attachRot;

            if (isThisFirstAttach == false)
            {
                isThisFirstAttach = true;
                GameObject.Find("TutorialMgr").GetComponent<TutorialManager_CM>().MaketutorialObj_Double();
            }

            checkFlag = true;
            StartCoroutine(HoldPos());
        }
    }

    IEnumerator HoldPos()
    {
        while (checkFlag == true)
        {
            parentObj.transform.SetParent(attachPos.transform); // 강제적으로 항상 오브젝트를 자식 객체로 만들어버리기

            parentObj.transform.localPosition = attachVec;
            parentObj.transform.localRotation = attachRot;
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log("코루틴 종료");    
    }
}
