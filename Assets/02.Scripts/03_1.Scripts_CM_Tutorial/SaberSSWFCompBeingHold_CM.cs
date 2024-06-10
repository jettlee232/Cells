using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberSSWFCompBeingHold_CM : MonoBehaviour
{
    public GameObject parentObj;

    public bool checkFlag = false;
    public GameObject attachPos;

    void Start()
    {
        parentObj = transform.parent.gameObject;
        attachPos = GameObject.Find("SaberWFSSCompleteAttachPos"); // ���� ������ �޾ƿ���        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == attachPos && parentObj.GetComponent<BNG.Grabbable>().BeingHeld == true) // ���� ������Ʈ�� ���� �����̶��
        {
            Debug.Log("OnTrigger!!!");

            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().statusFlag = 3;
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().bc2.enabled = true;
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().bc3.enabled = true;

            //other.transform.parent.gameObject.GetComponent<BoxCollider>().center = new Vector3(0, -0.5f, 0);
            //other.transform.parent.gameObject.GetComponent<BoxCollider>().size = new Vector3(1, 1.5f, 1);

            parentObj.transform.SetParent(other.gameObject.transform); // ���� ������Ʈ�� �� ������Ʈ�� �θ�� �����

            parentObj.GetComponent<BNG.Grabbable>().enabled = false;
            parentObj.GetComponent<Rigidbody>().useGravity = false;
            parentObj.GetComponent<Rigidbody>().isKinematic = false;
            parentObj.GetComponent<BoxCollider>().enabled = false;

            parentObj.transform.position = other.gameObject.transform.position; // �� ������Ʈ�� ��ġ�� ���� ������Ʈ�� ��ġ�� �ٲٱ�
            parentObj.transform.rotation = Quaternion.identity; // ���� �ʱ�ȭ

            checkFlag = true;
            StartCoroutine(HoldPos());
        }

        IEnumerator HoldPos()
        {
            while (checkFlag == true)
            {
                parentObj.transform.SetParent(attachPos.transform); // ���������� �׻� ������Ʈ�� �ڽ� ��ü�� ����������

                //parentObj.transform.position = attachPos.transform.position;
                parentObj.transform.localPosition = new Vector3(attachPos.transform.position.x - 0.75f, 0, 0);
                parentObj.transform.localRotation = Quaternion.identity;
                //parentObj.transform.localRotation = Quaternion.Euler(0, 0, 180);
                yield return new WaitForSeconds(0.01f);
            }
            Debug.Log("�ڷ�ƾ ����");
        }
    }
}
