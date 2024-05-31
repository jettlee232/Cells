using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectBeingHeldOrNot_CM : MonoBehaviour
{
    public float revortPos;

    public BNG.Grabbable grabbable;
    public Rigidbody rb;
    public Transform objRespawnPoint;

    public bool isHeld = false;

    void Start()
    {
        grabbable = GetComponent<BNG.Grabbable>(); // 시작과 동시에 같은 현 게임오브젝트 내의 Grabbable 스크립트 참조
        rb = GetComponent<Rigidbody>(); // Rigidbody의 kinematic을 제어하기 위해 Rigidbody 컴포넌트 참조
        rb.useGravity = true;
        rb.isKinematic = true;        
    }

   
    private void Update()
    {
        if (grabbable.BeingHeld == true && isHeld == false) // 현재 게임오브젝트를 집은 상태가 되었다면
        {
            isHeld = true;
            rb.isKinematic = false; // kinematic 해제
        }

        if (transform.position.y < revortPos) // 땅에 떨어지거나 떨어진 상태라면
        {
            if (transform.childCount > 0) // 자식(친수성 머리 장착 위치)이 있다면
            {
                if (transform.GetChild(0).childCount > 0) // 자식의 자식(붙어있는 친수성 머리)이 있다면
                {
                    if (transform.GetChild(0).GetChild(0).GetComponent<WFHeadBeingHold_CM>() == true) // 강제적으로 위치 고정 시켜놓는 플래그가 작동 중이라면
                    {
                        GameObject go = transform.GetChild(0).GetChild(0).gameObject;

                        go.GetComponent<WFHeadBeingHold_CM>().checkFlag = false; // 플래그 해제시켜서 강제적 위치 고정 해제
                        go.transform.SetParent(null); // 부모 버려서 다시 원래 상태로 복구                        

                        // 다른 코드도 원상복구 시켜서 별개 상호작용 가능하도록
                        go.GetComponent<SphereCollider>().enabled = true;                        
                        go.GetComponent<Rigidbody>().useGravity = true;
                        go.GetComponent<BNG.Grabbable>().enabled = true;
                        go.GetComponent<Rigidbody>().isKinematic = true;
                        go.transform.position = go.GetComponent<ObjectBeingHeldOrNot_CM>().objRespawnPoint.position;
                        go.transform.rotation = Quaternion.identity;
                        go.GetComponent<ObjectBeingHeldOrNot_CM>().isHeld = false;
                    }
                }
            }

            
            // kinematic도 받고, 원래 스폰 포인트로 돌아오고 각도도 원래대로 만들기 (각도는 필요하다면 다른 값을 주도록)
            rb.isKinematic = true;
            transform.position = objRespawnPoint.position;
            transform.rotation = Quaternion.identity;
            isHeld = false;
        }
    }
}
