using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberSSWFBeingHold_CM : MonoBehaviour
{
    public GameObject parentObj;

    public bool checkFlag = false;
    public GameObject attachPos;

    void Start()
    {
        parentObj = transform.parent.gameObject;
        attachPos = GameObject.Find("SaberWFSSAttachPos"); // 장착 지점을 받아오기        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == attachPos && parentObj.GetComponent<BNG.Grabbable>().BeingHeld == true) // 닿은 오브젝트가 장착 지점이라면
        {
            Debug.Log("OnTrigger!!!");

            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().statusFlag = 2;
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().bc1.enabled = true;

            //other.transform.parent.gameObject.GetComponent<BoxCollider>().center = new Vector3(0, -0.5f, 0);

            //other.transform.parent.gameObject.GetComponent<BoxCollider>().size = new Vector3(1, 1.5f, 1);
            //Debug.Log(other.transform.parent.GetChild(2).name);

            parentObj.transform.GetChild(0).GetComponent<SphereCollider>().enabled = false;
            //parentObj.GetComponent<ObjectBeingHeldOrNot_CM>().statusFlag = 2;

            parentObj.transform.SetParent(other.gameObject.transform); // 닿은 오브젝트를 이 오브젝트의 부모로 만들고

            parentObj.GetComponent<BNG.Grabbable>().enabled = false;
            parentObj.GetComponent<Rigidbody>().useGravity = false;
            parentObj.GetComponent<Rigidbody>().isKinematic = false;
            parentObj.GetComponent<BoxCollider>().enabled = false;

            parentObj.transform.position = other.gameObject.transform.position; // 이 오브젝트의 위치를 닿은 오브젝트의 위치로 바꾸기
            parentObj.transform.rotation = Quaternion.identity; // 각도 초기화

            checkFlag = true;

            
            StartCoroutine(HoldPos());
        }
    }

    IEnumerator HoldPos()
    {
        while (checkFlag == true)
        {
            parentObj.transform.SetParent(attachPos.transform); // 강제적으로 항상 오브젝트를 자식 객체로 만들어버리기
            //parentObj.transform.position = attachPos.transform.position;
            parentObj.transform.localPosition = new Vector3(0, attachPos.transform.position.y - 5, 0);
            parentObj.transform.localRotation = Quaternion.Euler(0, 0, 180);
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log("코루틴 종료");    
    }
}
