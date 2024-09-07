using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using BNG;

public class PlayerNetworkSetup_MT : MonoBehaviourPunCallbacks
{

    public GameObject BNGrig;
    public GameObject avatar;

    public GameObject AvatarHeadGameobject;
    public GameObject AvatarBodyGameobject;


    // Start is called before the first frame update
    void Start()
    {
        //Setup the player

        if (photonView.IsMine)
        {
            //The player is local
            BNGrig.SetActive(true);
            //avatar.SetActive(false);

            //gameObject.GetComponent<MovementController>().enabled = true;
            //gameObject.GetComponent<AvatarInputConverter>().enabled = true;
            gameObject.GetComponent<BNGPlayerController>().enabled = true;
            //gameObject.GetComponent<ChangeInput_Common>().enabled = true;
            gameObject.GetComponent<PlayerMoving_MT>().enabled = true;

            //SetLayerRecursively(AvatarHeadGameobject, 11);
            //SetLayerRecursively(AvatarBodyGameobject, 12);
        }
        else
        {
            //The player is remote
            BNGrig.transform.GetChild(0).gameObject.SetActive(false);

            gameObject.GetComponent<BNGPlayerController>().enabled = false;
            //gameObject.GetComponent<ChangeInput_Common>().enabled = false;
            gameObject.GetComponent<PlayerMoving_MT>().enabled = false;

            //gameObject.GetComponent<BNGPlayerController>().enabled = false;
            //gameObject.GetComponent<ChangeInput_Common>().enabled = false;
            //gameObject.GetComponent<PlayerMoving_MT>().enabled = false;

            //Remote players can be seen by the local player
            //So, we set the avatar head and body to Default layer
            //SetLayerRecursively(AvatarHeadGameobject, 0);
            //SetLayerRecursively(AvatarBodyGameobject, 0);
        }
    }

    void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}
