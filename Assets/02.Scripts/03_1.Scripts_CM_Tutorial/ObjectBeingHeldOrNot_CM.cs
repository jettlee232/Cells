using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBeingHeldOrNot_CM : MonoBehaviour
{
    public TutorialManager_CM tutoMgr;

    public float revortPos = 0.2f;

    public BNG.Grabbable grabbable;
    public Rigidbody rb;
    public Transform objRespawnPoint1;    
    public Transform objRespawnPoint2;
    public Quaternion objSpawnRotate;
    
    public bool isHeld = false;

    public int statusFlag = 0;

    [Header("Box Colliders")]
    public bool isthisMainFlag;
    public BoxCollider bc1;
    public BoxCollider bc2;
    public BoxCollider bc3;

    [Header("SaberSS or HeadWF Flag")]
    public bool isThisTail = false;
    public bool isThisHead = false;
    public bool isThisSingle = false;
    public bool isThisDouble = false;
    public bool firstGrab = false;

    void Start()
    {
        tutoMgr = GameObject.Find("TutorialMgr").GetComponent<TutorialManager_CM>();

        grabbable = GetComponent<BNG.Grabbable>(); // 시작과 동시에 같은 현 게임오브젝트 내의 Grabbable 스크립트 참조
        rb = GetComponent<Rigidbody>(); // Rigidbody의 kinematic을 제어하기 위해 Rigidbody 컴포넌트 참조
        rb.useGravity = true;
        rb.isKinematic = true;

        if (isThisTail == true)
        {
            objRespawnPoint1 = tutoMgr.spawnPos_Tail;
        }
        else if (isThisHead == true)
        {
            objRespawnPoint1 = tutoMgr.spawnPos_Head;
        }
        else if (isThisSingle == true)
        {
            objRespawnPoint1 = tutoMgr.spawnPos_Single;
        }
        else if (isThisDouble == true)
        {
            objRespawnPoint1 = tutoMgr.spawnPos_Double;
        }
        else
        {
            Debug.Log("Fucking Error");
        }

        objRespawnPoint2 = tutoMgr.spawnPos_Single;

        objSpawnRotate = Quaternion.identity;

        if (isthisMainFlag == true)
        {
            bc1.enabled = false;
            bc2.enabled = false;
            bc3.enabled = false;
        }
    }

   
    private void Update()
    {
        if (grabbable.BeingHeld == true && isHeld == false) // 현재 게임오브젝트를 집은 상태가 되었다면
        {
            isHeld = true;
            rb.isKinematic = false; // kinematic 해제

            // 2 - 각 오브젝트를 터치할 블록들 생성 및 연습 지시 -> (아예 처음 잡은 상태라면 2번째 대사 목록들 출력)
            if (isThisTail == true && tutoMgr.firstGrab == false)
            {
                tutoMgr.firstGrab = true;
            }
            else if (isThisHead == true && tutoMgr.firstGrab == false)
            {
                tutoMgr.firstGrab = true;
            }
        }

        if (transform.position.y < revortPos) // 땅에 떨어지거나 떨어진 상태라면
        {
            if (transform.childCount > 0) // 자식이 있다면
            {
                if (statusFlag == 1) // 친수성 머리가 자식으로 장착되어 있다면
                {
                    //if (transform.GetChild(0).GetChild(0).GetComponent<WFHeadBeingHold_CM>() == true) // 강제적으로 위치 고정 시켜놓는 플래그가 작동 중이라면
                    {
                        GameObject go = transform.GetChild(0).GetChild(0).gameObject; // 친수성 머리 오브젝트를

                        go.GetComponent<WFHeadBeingHold_CM>().checkFlag = false; // 플래그 해제시켜서 강제적 위치 고정 해제
                        go.transform.SetParent(null); // 부모 버려서 다시 원래 상태로 복구 - 이건 프레임 워크 작동원리 때문에...                       

                        // 다른 물리 연산 관련 코드도 원상복구 시켜서 별개 상호작용 가능하도록
                        go.GetComponent<SphereCollider>().enabled = true;                        
                        go.GetComponent<Rigidbody>().useGravity = true;
                        go.GetComponent<BNG.Grabbable>().enabled = true;
                        go.GetComponent<Rigidbody>().isKinematic = true;

                        go.transform.position = go.GetComponent<ObjectBeingHeldOrNot_CM>().objRespawnPoint1.position;
                        go.transform.rotation = Quaternion.identity;
                        go.GetComponent<ObjectBeingHeldOrNot_CM>().isHeld = false;
                    }
                }
                else if (statusFlag == 2) // 인지질 이중층 구조를 만든 상태라면
                {                    
                    objRespawnPoint1.position = objRespawnPoint2.position;
                    objSpawnRotate = Quaternion.Euler(90, 0, 0);                 
                }
                else if (statusFlag == 3) // 인지질 이중층을 쌍으로 만든 최종 형태라면
                {
                    objRespawnPoint1.position = new Vector3(objRespawnPoint2.position.x, objRespawnPoint2.position.y, objRespawnPoint2.position.z);
                    objSpawnRotate = Quaternion.Euler(90, 90, 0);    
                }
            }
            
            // kinematic도 받고, 원래 스폰 포인트로 돌아오고 각도도 원래대로 만들기 (각도는 필요하다면 다른 값을 주도록)
            rb.isKinematic = true;
            transform.position = objRespawnPoint1.position;
            transform.rotation = objSpawnRotate;
            isHeld = false;

            if (statusFlag == 1) statusFlag = 0;
        }
    }
}
