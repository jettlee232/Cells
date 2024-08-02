using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class Cutscene_Lys : MonoBehaviour
{
    public bool isButtonPressed = false;
    public Image black;
    UnityEngine.XR.InputDevice right;

    private void OnEnable()
    {
        StartCoroutine(MoveScene());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (right.isValid)
        {
            right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);
        }

        if (isButtonPressed == true)
        {
            SceneManager.LoadScene("06_Lys_Tutorial");
        }
    }

    IEnumerator MoveScene()
    {
        yield return new WaitForSeconds(1f);
        while(true)
        {
            if (black.color.a >= 0.999f) { SceneManager.LoadScene("06_Lys_Tutorial"); }
            yield return null;
        }
    }
}
