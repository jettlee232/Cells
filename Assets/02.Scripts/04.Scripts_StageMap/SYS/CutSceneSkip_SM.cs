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
                playableDirector.Evaluate(); // Ÿ�Ӷ����� ��� ���Ͽ� �̵��� �ð����� �ݿ�

                this.enabled = false;
            }
        }
    }
}
