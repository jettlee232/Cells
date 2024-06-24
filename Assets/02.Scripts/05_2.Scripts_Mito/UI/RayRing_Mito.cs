using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayRing_Mito : MonoBehaviour
{
    public Color RestingColor = Color.gray;
    public Color ValidSnapColor = Color.green;

    public Text ringText;
    public bool validSnap = false;

    void Start()
    {
        ringText = GetComponent<Text>();
    }

    void Update()
    {
        validSnap = checkIsValidRaySnap();

        ringText.color = validSnap ? ValidSnapColor : RestingColor;
    }

    public bool checkIsValidRaySnap()
    {
        if (GetComponentInParent<GrabbablesInTrigger>().ClosestGrabbable != null)
            return true;
        else
            return false;
    }
}
