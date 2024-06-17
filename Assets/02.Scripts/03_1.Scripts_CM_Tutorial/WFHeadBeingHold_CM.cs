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
        attachPos = GameObject.Find("WFHeadAttachPos"); // ���� ������ �޾ƿ���        
    }    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == attachPos && GetComponent<BNG.Grabbable>().BeingHeld == true) // ���� ������Ʈ�� ���� �����̶��
        {            
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().statusFlag = 1;

            transform.SetParent(other.gameObject.transform); // ���� ������Ʈ�� �� ������Ʈ�� �θ�� �����

            // �����ӿ�ũ �۵����� ������ ���� ���� ������ �Ͼ�� ���� ���� ���� ������Ʈ���� ��� �����صα�
            GetComponent<BNG.Grabbable>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<SphereCollider>().enabled = false;
            
            transform.position = other.gameObject.transform.position; // �� ������Ʈ�� ��ġ�� ���� ������Ʈ�� ��ġ�� �ٲٱ�
            transform.rotation = Quaternion.identity; // ���� �ʱ�ȭ

            // ���� �Ҽ��� ���ڶ��� ��ġ�� ���� ����ȭ�ϴ� �ڷ�ƾ ����
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
            transform.SetParent(attachPos.transform); // ���������� �׻� ������Ʈ�� �ڽ� ��ü�� ����������
            transform.position = attachPos.transform.position;
            transform.rotation = Quaternion.identity;
            yield return new WaitForSeconds(0.01f);
            //exitCnt += 0.01f;
        }
        Debug.Log("�ڷ�ƾ ����");

    }
}
