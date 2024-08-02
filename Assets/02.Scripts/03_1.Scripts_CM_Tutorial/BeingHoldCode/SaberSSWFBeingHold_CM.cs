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
    public Transform attachPos;

    public GameObject[] attachPosLight;
    public GameObject otherSide;

    ObjectBeingHeldOrNot_CM ocm;

    void Start()
    {
        parentObj = transform.parent.gameObject;
        //attachPos = GameObject.Find("SingleAttachPos");
        attachVec = new Vector3(0.8f, -3.4f, 0f);
        attachRot = Quaternion.Euler(0, 180f, -180f);

        ocm = GameObject.Find("Phospholipid_Tail_CM").GetComponent<ObjectBeingHeldOrNot_CM>();
        ocm.TurnEffect(1, true);
        ocm.TurnEffect(2, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "SingleAttachPos" && parentObj.GetComponent<BNG.Grabbable>().BeingHeld == true)
        {
            otherSide.SetActive(false);

            ObjectBeingHeldOrNot_CM  parents = other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>();

            parents.statusFlag = 2;
            parents.bc1.enabled = true;
            parents.TurnEffect(false);
            //parents.DescPanelONOff(false);

            if (effectFlag == false)
            {
                effectFlag = true;
                parents.TurnAttachEffect(1);
            }
            parents.TurnEffect(3, true);
            //parents.TurnEffect(4, true);

            parents.toolTipPanels[0].GetComponent<Tooltip>().TooltipOff();
            parents.MadeToolTip(1);
            //parents.MadeToolTip(2);

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

                TutorialManager_CM tutoMgr = GameObject.FindGameObjectWithTag("GameController").GetComponent<TutorialManager_CM>();
                tutoMgr.MaketutorialObj_Double();
                tutoMgr.NewFollow(1, 0);
            }



            checkFlag = true;
            Transform par = parents.gameObject.transform;
            for (int i = 0 ; i < par.childCount; i++)
            {
                if (par.GetChild(i).name == "SingleAttachPos")
                {
                    if (par.GetChild(i).GetComponent<AttachPosID_CM>().ID == 0)
                    {
                        attachPos = par.GetChild(i);
                    }
                }
            }

            for (int i = 0; i < attachPosLight.Length; i++) attachPosLight[i].SetActive(false);

            StartCoroutine(HoldPos());
        }
    }

    IEnumerator HoldPos()
    {
        while (checkFlag == true)
        {
            parentObj.transform.SetParent(attachPos);

            parentObj.transform.localPosition = attachVec;
            parentObj.transform.localRotation = attachRot;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
