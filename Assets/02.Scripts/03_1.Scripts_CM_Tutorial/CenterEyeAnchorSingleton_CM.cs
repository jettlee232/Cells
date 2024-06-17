using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterEyeAnchorSingleton_CM : MonoBehaviour
{
    private static CenterEyeAnchorSingleton_CM instance;
    public static CenterEyeAnchorSingleton_CM Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CenterEyeAnchorSingleton_CM>();
            }
            return instance;
        }
    }

    public Camera centerEyeAnchor;

    void Awake()
    {
        centerEyeAnchor = GetComponent<Camera>();
    }

    public Camera GetCameraComponent()
    {
        return centerEyeAnchor;
    }

}
