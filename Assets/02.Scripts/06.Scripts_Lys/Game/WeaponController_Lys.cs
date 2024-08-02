using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class WeaponController_Lys : MonoBehaviour
{
    UnityEngine.XR.InputDevice right;
    private bool AValue = false;
    private bool oldAValue = false;
    public bool Gun = true;     // true 이면 총, false 이면 로켓런처
    public GameObject gun;
    public GameObject rocket;

    private void Start()
    {
        gun.SetActive(true);
        rocket.SetActive(false);
        Gun = true;
    }

    void Update()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        right.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out AValue);

        if (AValue && !oldAValue)
        {
            if (GameManager_Lys_Game.instance.ToolTip.activeSelf) { GameManager_Lys_Game.instance.HideToolTip(); }
            if (Gun) { UseRocket(); }
            else { UseGun(); }
        }

        oldAValue = AValue;
    }

    void UseGun()
    {
        rocket.SetActive(false);
        gun.SetActive(true);
        Gun = true;
    }
    
    void UseRocket()
    {
        gun.SetActive(false);
        rocket.SetActive(true);
        Gun = false;
    }
}
