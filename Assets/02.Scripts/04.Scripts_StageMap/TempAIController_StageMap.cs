using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TempAIController_StageMap : MonoBehaviour
{
    UnityEngine.XR.InputDevice left;
    private bool triggered = false;
    private bool oldTriggered = false;

    public GameObject ToggleObject;

    private bool triggered_menu = false;
    private bool oldTriggered_menu = false;

    public GameObject MenuObject;

    void Update()
    {
        left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        left.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out triggered);

        if (triggered && !oldTriggered)
        {
            if (!ToggleObject.activeSelf) { ToggleObject.SetActive(true); }
            else { ToggleObject.SetActive(false); }
        }

        oldTriggered = triggered;


        left.TryGetFeatureValue(UnityEngine.XR.CommonUsages.menuButton, out triggered_menu);

        if (triggered_menu && !oldTriggered_menu)
        {
            if (!MenuObject.activeSelf) { MenuObject.SetActive(true); }
            else { MenuObject.SetActive(false); }
        }

        oldTriggered_menu = triggered_menu;
    }
}