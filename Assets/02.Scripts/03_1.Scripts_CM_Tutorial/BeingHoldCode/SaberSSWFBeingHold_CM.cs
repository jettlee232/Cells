using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberSSWFBeingHold_CM : MonoBehaviour
{
    public GameObject parentObj;
    public Vector3 attachVec;
    public Quaternion attachRot;

    public bool isThisFirstAttach = false;
    public bool checkFlag = false;
    private bool effectFlag = false;
    public GameObject attachPos;

    ObjectBeingHeldOrNot_CM ocm;

    void Start()
    {
        parentObj = transform.parent.gameObject;
        attachPos = GameObject.Find("SingleAttachPos"); 
        attachVec = new Vector3(0.8f, -3.4f, 0f);
        attachRot = Quaternion.Euler(0, 180f, -180f);

        ocm = GameObject.Find("Phospholipid_Tail_CM").GetComponent<ObjectBeingHeldOrNot_CM>();
        ocm.TurnEffect(1, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == attachPos && parentObj.GetComponent<BNG.Grabbable>().BeingHeld == true) 
        {
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().statusFlag = 2;
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().bc1.enabled = true;
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().TurnEffect(false);
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().DescPanelONOff(false);

            if (effectFlag == false)
            {
                effectFlag = true;
                other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().TurnAttachEffect(1);
            }
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().TurnEffect(2, true);
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().TurnEffect(3, true);

            parentObj.transform.SetParent(other.gameObject.transform); 

            parentObj.GetComponent<BNG.Grabbable>().enabled = false;
            parentObj.GetComponent<Rigidbody>().useGravity = false;
            parentObj.GetComponent<Rigidbody>().isKinematic = false;
            parentObj.GetComponent<BoxCollider>().enabled = false;
            parentObj.GetComponent<ObjectBeingHeldOrNot_CM>().DestroyDescPanel();

            parentObj.transform.position = attachVec;
            parentObj.transform.rotation = attachRot;

            if (isThisFirstAttach == false)
            {
                isThisFirstAttach = true;
                GameObject.Find("TutorialMgr").GetComponent<TutorialManager_CM>().MaketutorialObj_Double();

                // 퀘스트 패널 텍스트 변경                
                GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(2).GetComponent<QuestPanel_CM>().ChangeText("인지질끼리 양옆으로 연결해보자!");
                GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(3).GetComponent<RulePanel_CM>().ChangeText("인지질끼리 양옆으로 연결하는 법");
                GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(3).GetComponent<RulePanel_CM>().ChangeImg("f2");
            }

            checkFlag = true;
            StartCoroutine(HoldPos());
        }
    }

    IEnumerator HoldPos()
    {
        while (checkFlag == true)
        {
            parentObj.transform.SetParent(attachPos.transform);

            parentObj.transform.localPosition = attachVec;
            parentObj.transform.localRotation = attachRot;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
