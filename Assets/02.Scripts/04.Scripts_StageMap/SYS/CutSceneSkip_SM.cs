using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Playables;


public class CutSceneSkip_SM : MonoBehaviour
{
    public PlayableDirector playableDirector;

    public float moveTime = 0f;

    UnityEngine.XR.InputDevice right;
    private bool isButtonPressed = false;
    private bool locking = false;

    void Update()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        right.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out isButtonPressed);
        if (isButtonPressed) 
        { 
            if (locking == false)
            {
                locking = true;

                playableDirector.time = moveTime;
                playableDirector.Evaluate(); // 타임라인을 즉시 평가하여 이동한 시간으로 반영

                this.enabled = false;
            }
        }
    }
}
