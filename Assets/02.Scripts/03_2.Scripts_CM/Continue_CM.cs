using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Continue_CM : MonoBehaviour
{
    public GameManager_CM gameMgr;

    UnityEngine.XR.InputDevice right;
    UnityEngine.XR.InputDevice left;

    private bool activateFlag = false;
    private bool l_X = false;
    private bool l_Y = false;
    private bool l_Pt = false;
    private bool l_St = false;
    private bool r_A = false;
    private bool r_B = false;
    private bool r_Pt = false;
    private bool r_St = false;

    private void Start()
    {
        Invoke("ChangeFlag", 6f);
    }

    void Update()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        right.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out l_X);
        right.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out l_Y);
        right.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out l_Pt);
        right.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryTouch, out l_St);
        left.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out r_A);
        left.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out r_B);
        left.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out r_Pt);
        left.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryTouch, out r_St);

        if (activateFlag == true)
        {
            if (l_X || l_Y || l_Pt || l_St || r_A || r_B || r_Pt || r_St)
            {
                gameMgr.PopupTutoPanel();
                Invoke("gameMgr.GameStart", 2f);
                this.enabled = false;
            }
            if (Input.GetKeyDown(KeyCode.Space)) // Test¿ë
            {
                gameMgr.PopupTutoPanel();
                Invoke("gameMgr.GameStart", 2f);
                this.enabled = false;
            }
        }        
    }

    void ChangeFlag()
    {
        activateFlag = true;
    }
}
