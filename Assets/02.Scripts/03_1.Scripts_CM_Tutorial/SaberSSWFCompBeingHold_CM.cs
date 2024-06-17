using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberSSWFCompBeingHold_CM : MonoBehaviour
{
    public GameObject parentObj;
    public Vector3 attachVec = new Vector3(0f, -1.05f, -1.7f);
    public Quaternion attachRot = Quaternion.Euler(0f, 180f, 0f);

    public bool isThisFirstAttach = false;
    public bool checkFlag = false;
    public GameObject attachPos;

    void Start()
    {
        parentObj = transform.parent.gameObject;
        attachPos = GameObject.Find("DoubleAttachPos"); // ���� ������ �޾ƿ���
        attachVec = new Vector3(0f, -1.05f, -1.7f);
        attachRot = Quaternion.Euler(0f, 180f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == attachPos && parentObj.GetComponent<BNG.Grabbable>().BeingHeld == true) // ���� ������Ʈ�� ���� �����̶��
        {
            Debug.Log("OnTrigger!!!");

            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().statusFlag = 3;
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().bc2.enabled = true;
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().bc3.enabled = true;

            parentObj.transform.SetParent(other.gameObject.transform); // ���� ������Ʈ�� �� ������Ʈ�� �θ�� �����

            parentObj.GetComponent<BNG.Grabbable>().enabled = false;
            parentObj.GetComponent<Rigidbody>().useGravity = false;
            parentObj.GetComponent<Rigidbody>().isKinematic = false;
            parentObj.GetComponent<BoxCollider>().enabled = false;

            parentObj.transform.position = attachVec;
            parentObj.transform.rotation = attachRot;

            if (isThisFirstAttach == false)
            {
                isThisFirstAttach = true;
                GameObject.Find("NarratorNPC").GetComponent<NarratorDialogueHub_CM_Tutorial>().StartCov_3();
            }

            checkFlag = true;
            StartCoroutine(HoldPos());
        }

        IEnumerator HoldPos()
        {
            while (checkFlag == true)
            {
                parentObj.transform.SetParent(attachPos.transform); // ���������� �׻� ������Ʈ�� �ڽ� ��ü�� ����������

                parentObj.transform.localPosition = attachVec;
                parentObj.transform.localRotation = attachRot;
                yield return new WaitForSeconds(0.01f);
            }
            Debug.Log("�ڷ�ƾ ����");
        }
    }
}
