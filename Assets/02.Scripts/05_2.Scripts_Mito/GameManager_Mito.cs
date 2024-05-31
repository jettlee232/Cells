using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_Mito : MonoBehaviour
{

    void Start()
    {
        Time.fixedDeltaTime = (Time.timeScale / UnityEngine.XR.XRDevice.refreshRate);
    }

    //void Update()
    //{

    //}

}
