using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using BNG;

public class PlayerNetworkSetup_MT : MonoBehaviourPunCallbacks
{
    public GameObject BNGrig;
    public GameObject avatar;

    int myBodyLayer;

    void Start()
    {
        myBodyLayer = LayerMask.NameToLayer("MyBody");

        if (photonView.IsMine)
        {
            BNGrig.SetActive(true);
            gameObject.GetComponent<BNGPlayerController>().enabled = true;
            gameObject.GetComponent<PlayerMoving_MT>().enabled = true;
            gameObject.GetComponent<AvatarInputConverter_MT>().enabled = true;
            gameObject.GetComponent<FadeInOut_MT>().enabled = true;

            gameObject.transform.GetChild(1).GetChild(0).gameObject.layer = myBodyLayer;
            gameObject.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.layer = myBodyLayer;
            gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject.layer = myBodyLayer;

            gameObject.transform.GetChild(1).GetChild(1).gameObject.layer = myBodyLayer;
            gameObject.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.layer = myBodyLayer;
        }
        else
        {
            //BNGrig.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
