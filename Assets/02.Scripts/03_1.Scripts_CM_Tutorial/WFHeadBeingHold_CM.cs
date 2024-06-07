using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFHeadBeingHold_CM : MonoBehaviour
{
    public bool checkFlag = false;
    public GameObject attachPos;

    public bool firstAttach = false;

    void Start()
    {
        attachPos = GameObject.Find("WFHeadAttachPos"); // 장착 지점을 받아오기        
    }    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == attachPos && GetComponent<BNG.Grabbable>().BeingHeld == true) // 닿은 오브젝트가 장착 지점이라면
        {            
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().statusFlag = 1;

            transform.SetParent(other.gameObject.transform); // 닿은 오브젝트를 이 오브젝트의 부모로 만들고

            // 프레임워크 작동원리 때문에 물리 연산 오류가 일어나니 물리 연산 관련 컴포넌트들은 잠시 해제해두기
            GetComponent<BNG.Grabbable>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<SphereCollider>().enabled = false;
            
            transform.position = other.gameObject.transform.position; // 이 오브젝트의 위치를 닿은 오브젝트의 위치로 바꾸기
            transform.rotation = Quaternion.identity; // 각도 초기화

            // 이제 소수성 끝자락의 위치를 강제 동기화하는 코루틴 실행
            checkFlag = true;
            StartCoroutine(HoldPos());

            if (firstAttach == false)
            {
                firstAttach = true;
                GetComponent<ObjectBeingHeldOrNot_CM>().tutoMgr.narrator.StartCov_Third();
            }            
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
