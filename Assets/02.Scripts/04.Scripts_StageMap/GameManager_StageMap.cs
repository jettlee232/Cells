using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GameManager_StageMap : MonoBehaviour
{
    UnityEngine.XR.InputDevice right;
    UnityEngine.XR.InputDevice left;

    private bool showUI = false;
    private bool oldShowUI = false;
    private GameObject[] Organelles;

    void Start()
    {
        Organelles = GameObject.FindGameObjectsWithTag("Organelle_SM");
    }

    void Update()
    {
        showOrganelleUI();
    }

    private void showOrganelleUI()
    {
        left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        left.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out showUI);

        if (showUI && !oldShowUI)
        {
            foreach (GameObject organelle in Organelles)
            {
                organelle.GetComponent<OrganelleController_StageMap>().fShowUI();
            }
            oldShowUI = true;
        }
        if (!showUI && oldShowUI)
        {
            foreach (GameObject organelle in Organelles)
            {
                organelle.GetComponent<OrganelleController_StageMap>().fHideUI();
            }
            oldShowUI = false;
        }
    }
}
