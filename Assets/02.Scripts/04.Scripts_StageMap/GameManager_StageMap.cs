using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GameManager_StageMap : MonoBehaviour
{
    UnityEngine.XR.InputDevice right;
    UnityEngine.XR.InputDevice left;

    private bool showUI = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void getUp()
    {
        left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        left.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out showUI);

        if (showUI) {  }
        else {  }
    }
}
