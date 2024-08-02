using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFHeadBeingHold_CM : MonoBehaviour
{
    public bool isInzizilMakeIt = false;
    public bool checkFlag = false;
    public GameObject attachPos;
    NarratorDialogueHub_CM_Tutorial narrator = null;

    [Header("StartConv_3's Coroutine Wait Time : 10 Seconds")]
    public float waitTime = 1f;

    void Start()
    {
        attachPos = GameObject.Find("HeadAttachPos");
        narrator = GameObject.Find("NarratorNPC").GetComponent<NarratorDialogueHub_CM_Tutorial>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == attachPos && GetComponent<BNG.Grabbable>().BeingHeld == true)
        {
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().TurnAttachEffect(0);
            other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().statusFlag = 1;
            //other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().DescPanelONOff(false);

            transform.SetParent(other.gameObject.transform);

            GetComponent<BNG.Grabbable>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<SphereCollider>().enabled = false;

            transform.position = other.gameObject.transform.position;
            transform.rotation = Quaternion.identity;

            checkFlag = true;
            StartCoroutine(HoldPos());

            if (isInzizilMakeIt == false)
            {
                isInzizilMakeIt = true;
                gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().TurnLayerToDefault();
                other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().TurnLayerToDefault();
                StartCoroutine(StartConv2After10Sec());

                gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().DestroyDescPanel();
                other.transform.parent.gameObject.GetComponent<ObjectBeingHeldOrNot_CM>().DestroyDescPanel();
            }
        }
    }

    IEnumerator HoldPos()
    {
        while (checkFlag == true)
        {
            transform.SetParent(attachPos.transform);
            transform.position = attachPos.transform.position;
            transform.rotation = Quaternion.identity;
            yield return new WaitForSeconds(0.01f);
        }

    }

    IEnumerator StartConv2After10Sec()
    {
        yield return new WaitForSeconds(waitTime);

        narrator.StartCov_2();
    }
}
