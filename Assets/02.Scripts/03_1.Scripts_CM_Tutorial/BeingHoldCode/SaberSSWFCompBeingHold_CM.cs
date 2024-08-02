using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberSSWFCompBeingHold_CM : MonoBehaviour
{
    public GameObject parentObj;
    public Vector3 attachVec;
    public Quaternion attachRot;

    public bool isThisFirstAttach = false;
    public bool checkFlag = false;
    private bool effectFlag = false;
    public Transform[] attachPos;
    public GameObject attachPosLight;

    ObjectBeingHeldOrNot_CM ocm;

    void Start()
    {
        parentObj = transform.parent.gameObject;
        //attachPos[0] = GameObject.Find("DoubleAttachPos0");
        //attachPos[1] = GameObject.Find("DoubleAttachPos1");

        ocm = GameObject.Find("Phospholipid_Tail_CM").GetComponent<ObjectBeingHeldOrNot_CM>();
        //ocm.TurnEffect(2, true);

        attachRot = Quaternion.Euler(0f, 180f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // *****
        if (other.gameObject.name == "DoubleAttachPos0" && parentObj.GetComponent<BNG.Grabbable>().BeingHeld == true) 
        {
            ObjectBeingHeldOrNot_CM parents = other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>();
            parents.statusFlag = 3;
            parents.bc2.enabled = true;
            parents.bc3.enabled = true;
            parents.TurnEffect(false);
            //parents.DescPanelONOff(false);

            if (effectFlag == false)
            {
                effectFlag = true;
                parents.TurnAttachEffect(2);
            }
            
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
                GameObject.Find("NarratorNPC").GetComponent<NarratorDialogueHub_CM_Tutorial>().StartCov_3();
                parents.toolTipPanels[1].GetComponent<Tooltip>().TooltipOff();
                //parents.toolTipPanels[2].GetComponent<Tooltip>().TooltipOff();
            }

            attachPosLight.SetActive(false);

            checkFlag = true;
            attachPos[0] = other.transform;
            StartCoroutine(HoldPos(attachPos[0].transform));
        }

        // *****
        /*
        if (other.gameObject == attachPos[1] && parentObj.GetComponent<BNG.Grabbable>().BeingHeld == true)
        {
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().statusFlag = 3;
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().bc2.enabled = true;
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().bc3.enabled = true;
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().TurnEffect(false);
            if (effectFlag == false)
            {
                effectFlag = true;
                other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().TurnAttachEffect(2);
            }

            parentObj.transform.SetParent(other.gameObject.transform);

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
                other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().toolTipPanels[1].GetComponent<Tooltip_CM>().TooltipOff();
                other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().toolTipPanels[2].GetComponent<Tooltip_CM>().TooltipOff();
            }

            checkFlag = true;
            StartCoroutine(HoldPos(attachPos[1].transform));
        }
        */

        IEnumerator HoldPos(Transform trns)
        {
            while (checkFlag == true)
            {
                parentObj.transform.SetParent(trns);

                parentObj.transform.localPosition = attachVec;
                parentObj.transform.localRotation = attachRot;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
