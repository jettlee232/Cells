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
        grabbable = GetComponent<BNG.Grabbable>(); // ���۰� ���ÿ� ���� �� ���ӿ�����Ʈ ���� Grabbable ��ũ��Ʈ ����
        rb = GetComponent<Rigidbody>(); // Rigidbody�� kinematic�� �����ϱ� ���� Rigidbody ������Ʈ ����
        rb.useGravity = true;
        rb.isKinematic = true;        
    }

   
    private void Update()
    {
        if (grabbable.BeingHeld == true && isHeld == false) // ���� ���ӿ�����Ʈ�� ���� ���°� �Ǿ��ٸ�
        {
            isHeld = true;
            rb.isKinematic = false; // kinematic ����
        }

        if (transform.position.y < revortPos) // ���� �������ų� ������ ���¶��
        {
            if (transform.childCount > 0) // �ڽ�(ģ���� �Ӹ� ���� ��ġ)�� �ִٸ�
            {
                if (transform.GetChild(0).childCount > 0) // �ڽ��� �ڽ�(�پ��ִ� ģ���� �Ӹ�)�� �ִٸ�
                {
                    if (transform.GetChild(0).GetChild(0).GetComponent<WFHeadBeingHold_CM>() == true) // ���������� ��ġ ���� ���ѳ��� �÷��װ� �۵� ���̶��
                    {
                        GameObject go = transform.GetChild(0).GetChild(0).gameObject;

                        go.GetComponent<WFHeadBeingHold_CM>().checkFlag = false; // �÷��� �������Ѽ� ������ ��ġ ���� ����
                        go.transform.SetParent(null); // �θ� ������ �ٽ� ���� ���·� ����                        

                        // �ٸ� �ڵ嵵 ���󺹱� ���Ѽ� ���� ��ȣ�ۿ� �����ϵ���
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

            
            // kinematic�� �ް�, ���� ���� ����Ʈ�� ���ƿ��� ������ ������� ����� (������ �ʿ��ϴٸ� �ٸ� ���� �ֵ���)
            rb.isKinematic = true;
            transform.position = objRespawnPoint.position;
            transform.rotation = Quaternion.identity;
            isHeld = false;
        }
    }
}
