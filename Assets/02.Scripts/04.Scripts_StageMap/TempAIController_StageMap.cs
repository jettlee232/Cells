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
    }
}
