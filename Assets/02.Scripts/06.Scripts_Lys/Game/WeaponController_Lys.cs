using BNG;
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
    public GameObject grabber;
    public GameObject changeEffect;

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
            if (GameManager_Lys_Game.instance.ToolTip.activeSelf)
            {
                GameManager_Lys_Game.instance.HideToolTip();
                GameManager_Lys_Game.instance.ShowGunToolTip();
            }
            if (Gun) { UseRocket(); }
            else { UseGun(); }
        }

        oldAValue = AValue;
    }

    void UseGun()
    {
        Instantiate(changeEffect, rocket.transform.parent.transform.GetChild(0).transform.position, rocket.transform.rotation);
        grabber.GetComponent<Grabber>().ForceGrab = false;
        rocket.SetActive(false);
        gun.SetActive(true);
        grabber.GetComponent<Grabber>().ForceGrab = true;
        Gun = true;
    }

    void UseRocket()
    {
        Instantiate(changeEffect, gun.transform.parent.transform.GetChild(0).transform.position, gun.transform.rotation);
        grabber.GetComponent<Grabber>().ForceGrab = false;
        gun.SetActive(false);
        rocket.SetActive(true);
        grabber.GetComponent<Grabber>().ForceGrab = true;
        Gun = false;
    }

    public void HideWeapon()
    {
        Instantiate(changeEffect, gun.transform.parent.transform.GetChild(0).transform.position, gun.transform.rotation);
        grabber.GetComponent<Grabber>().ForceGrab = false;
        if (gun.activeSelf) { gun.SetActive(false); }
        if (rocket.activeSelf) { rocket.SetActive(false); }
    }

    public void ShowWeapon()
    {
        Instantiate(changeEffect, gun.transform.parent.transform.GetChild(0).transform.position, gun.transform.rotation);
        if (Gun)
        {
            gun.SetActive(true);
            grabber.GetComponent<Grabber>().ForceGrab = true;
        }
        else
        {
            rocket.SetActive(true);
            grabber.GetComponent<Grabber>().ForceGrab = true;
        }
    }
}
