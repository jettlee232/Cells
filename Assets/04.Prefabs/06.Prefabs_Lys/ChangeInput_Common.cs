using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeInput_Common : MonoBehaviour
{
    public string objectNameToFind = "EventSystem";
    public float time = 0.5f;

    void OnEnable()
    {
        Invoke("ChangeInput", time);
    }

    void ChangeInput()
    {
        GameObject key = GameObject.Find(objectNameToFind);

        if (key != null)
        {
            key.GetComponent<VRUISystem>().ControllerInput.Clear();
            key.GetComponent<VRUISystem>().ControllerInput.Add(ControllerBinding.AButton);
        }
    }
}
