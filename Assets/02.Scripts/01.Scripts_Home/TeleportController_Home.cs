using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController_Home : MonoBehaviour
{
    public GameObject fixedTeleport;
    public GameObject offSwitch;
    public GameObject onSwitch;

    void Start()
    {
        offSwitch = transform.Find("Switch_Off").gameObject;
        onSwitch = transform.Find("Switch_On").gameObject;
    }

    void Update()
    {
        if (onSwitch.activeSelf)
            fixedTeleport.SetActive(true);
        else
            fixedTeleport.SetActive(false);
    }
}
