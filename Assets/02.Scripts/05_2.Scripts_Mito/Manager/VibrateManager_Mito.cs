using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VibrateManager_Mito : MonoBehaviour
{
    public static VibrateManager_Mito Instance { get; private set; }

    InputDevice left;
    InputDevice right;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    // 진동은 이렇게 써야한다
    public void VibrateBothHands()
    {
        HapticCapabilities capabilities;
        if (left.TryGetHapticCapabilities(out capabilities))
            if (capabilities.supportsImpulse)
                left.SendHapticImpulse(0, 0.5f, 1.0f);

        if (right.TryGetHapticCapabilities(out capabilities))
            if (capabilities.supportsImpulse)
                right.SendHapticImpulse(0, 0.5f, 1.0f);
    }

    public void ShortVibrateBothHands()
    {
        HapticCapabilities capabilities;
        if (left.TryGetHapticCapabilities(out capabilities))
            if (capabilities.supportsImpulse)
                left.SendHapticImpulse(0, 0.25f, 0.5f);

        if (right.TryGetHapticCapabilities(out capabilities))
            if (capabilities.supportsImpulse)
                right.SendHapticImpulse(0, 0.25f, 0.5f);
    }

    /* Oculus
    public void VibrateBothHands()
    {
        StartCoroutine(VibrateController(1.5f, 0.3f, 0.3f, OVRInput.Controller.LTouch));
        StartCoroutine(VibrateController(1.5f, 0.3f, 0.3f, OVRInput.Controller.RTouch));
    }

    IEnumerator VibrateController(float waitTime, float frequency, float amplitude, OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        yield return new WaitForSeconds(waitTime);
        OVRInput.SetControllerVibration(0, 0, controller);
    }
    */
}
