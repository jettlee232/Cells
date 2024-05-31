using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFHeadBeingHold_CM : MonoBehaviour
{
    public bool checkFlag = false;
    public GameObject attachPos;

    void Start()
    {
        attachPos = GameObject.Find("WFHeadAttachPos"); // 장착 지점을 받아오기        
    }    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == attachPos && GetComponent<BNG.Grabbable>().BeingHeld == true) // 닿은 오브젝트가 장착 지점이라면
        {
            Debug.Log("OnTrigger!!!");

            transform.SetParent(other.gameObject.transform); // 닿은 오브젝트를 이 오브젝트의 부모로 만들고

            GetComponent<BNG.Grabbable>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<SphereCollider>().enabled = false;
            
            transform.position = other.gameObject.transform.position; // 이 오브젝트의 위치를 닿은 오브젝트의 위치로 바꾸기
            transform.rotation = Quaternion.identity; // 각도 초기화

            checkFlag = true;
            StartCoroutine(HoldPos());
        }
    }

    IEnumerator HoldPos()
    {
        //float exitCnt = 0f;

        while (checkFlag == true)
        {
            transform.SetParent(attachPos.transform); // 강제적으로 항상 오브젝트를 자식 객체로 만들어버리기
            transform.position = attachPos.transform.position;
            transform.rotation = Quaternion.identity;
            yield return new WaitForSeconds(0.01f);
            //exitCnt += 0.01f;
        }
        Debug.Log("코루틴 종료");

    }
}
