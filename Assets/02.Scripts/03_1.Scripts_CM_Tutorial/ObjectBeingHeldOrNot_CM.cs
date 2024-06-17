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

        grabbable = GetComponent<BNG.Grabbable>(); // ���۰� ���ÿ� ���� �� ���ӿ�����Ʈ ���� Grabbable ��ũ��Ʈ ����
        rb = GetComponent<Rigidbody>(); // Rigidbody�� kinematic�� �����ϱ� ���� Rigidbody ������Ʈ ����
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
        if (grabbable.BeingHeld == true && isHeld == false) // ���� ���ӿ�����Ʈ�� ���� ���°� �Ǿ��ٸ�
        {
            isHeld = true;
            rb.isKinematic = false; // kinematic ����

            // 2 - �� ������Ʈ�� ��ġ�� ��ϵ� ���� �� ���� ���� -> (�ƿ� ó�� ���� ���¶�� 2��° ��� ��ϵ� ���)
            if (isThisTail == true && tutoMgr.firstGrab == false)
            {
                tutoMgr.firstGrab = true;
            }
            else if (isThisHead == true && tutoMgr.firstGrab == false)
            {
                tutoMgr.firstGrab = true;
            }
        }

        if (transform.position.y < revortPos) // ���� �������ų� ������ ���¶��
        {
            if (transform.childCount > 0) // �ڽ��� �ִٸ�
            {
                if (statusFlag == 1) // ģ���� �Ӹ��� �ڽ����� �����Ǿ� �ִٸ�
                {
                    //if (transform.GetChild(0).GetChild(0).GetComponent<WFHeadBeingHold_CM>() == true) // ���������� ��ġ ���� ���ѳ��� �÷��װ� �۵� ���̶��
                    {
                        GameObject go = transform.GetChild(0).GetChild(0).gameObject; // ģ���� �Ӹ� ������Ʈ��

                        go.GetComponent<WFHeadBeingHold_CM>().checkFlag = false; // �÷��� �������Ѽ� ������ ��ġ ���� ����
                        go.transform.SetParent(null); // �θ� ������ �ٽ� ���� ���·� ���� - �̰� ������ ��ũ �۵����� ������...                       

                        // �ٸ� ���� ���� ���� �ڵ嵵 ���󺹱� ���Ѽ� ���� ��ȣ�ۿ� �����ϵ���
                        go.GetComponent<SphereCollider>().enabled = true;                        
                        go.GetComponent<Rigidbody>().useGravity = true;
                        go.GetComponent<BNG.Grabbable>().enabled = true;
                        go.GetComponent<Rigidbody>().isKinematic = true;

                        go.transform.position = go.GetComponent<ObjectBeingHeldOrNot_CM>().objRespawnPoint1.position;
                        go.transform.rotation = Quaternion.identity;
                        go.GetComponent<ObjectBeingHeldOrNot_CM>().isHeld = false;
                    }
                }
                else if (statusFlag == 2) // ������ ������ ������ ���� ���¶��
                {                    
                    objRespawnPoint1.position = objRespawnPoint2.position;
                    objSpawnRotate = Quaternion.Euler(90, 0, 0);                 
                }
                else if (statusFlag == 3) // ������ �������� ������ ���� ���� ���¶��
                {
                    objRespawnPoint1.position = new Vector3(objRespawnPoint2.position.x, objRespawnPoint2.position.y, objRespawnPoint2.position.z);
                    objSpawnRotate = Quaternion.Euler(90, 90, 0);    
                }
            }
            
            // kinematic�� �ް�, ���� ���� ����Ʈ�� ���ƿ��� ������ ������� ����� (������ �ʿ��ϴٸ� �ٸ� ���� �ֵ���)
            rb.isKinematic = true;
            transform.position = objRespawnPoint1.position;
            transform.rotation = objSpawnRotate;
            isHeld = false;

            if (statusFlag == 1) statusFlag = 0;
        }
    }
}
