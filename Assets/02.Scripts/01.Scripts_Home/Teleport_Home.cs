using BNG;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.XR;

public class Teleport_Home : MonoBehaviour
{
    public ScreenFader fader;
    public float TeleportFadeSpeed = 10.0f;

    public LineRenderer teleportLine;
    public Transform teleportGoal;
    public AudioClip teleportSoundEffect;
    
    private bool triggerPressed;
    public bool canTeleport = true;
    public float teleportCooldown = 1.0f; // 텔레포트 딜레이

    void Start()
    {
        fader = GameObject.FindWithTag("MainCamera").GetComponent<ScreenFader>();
        teleportLine = GetComponentInChildren<LineRenderer>();
    }

    void Update()
    {
        if (teleportGoal != null)
        {
            teleportGoal.localPosition = teleportLine.GetPosition(1);
            //Debug.Log("텔포라인겟포지션1" +teleportLine.GetPosition(1));
        }

        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed);

        if (triggerPressed && canTeleport)
        {
            StartCoroutine(TeleportPlayer());
        }
    }

    IEnumerator TeleportPlayer()
    {
        if (teleportSoundEffect != null)
            AudioSource.PlayClipAtPoint(teleportSoundEffect, transform.root.GetChild(0).position);

        BeforeTeleport();

        yield return new WaitForSeconds(0.1f);

        //Debug.Log("Before : " + transform.root.GetChild(0).position);
        //Debug.Log("Before : " + teleportGoal.position);
        transform.root.GetChild(0).position = teleportGoal.position;
        //Debug.Log("After : " + transform.root.GetChild(0).position);
        //Debug.Log("After : " + teleportGoal.position);
        StartCoroutine(TeleportCooldown());

        yield return new WaitForSeconds(0.25f);

        AfterTeleport();
    }

    IEnumerator TeleportCooldown()
    {
        canTeleport = false;
        yield return new WaitForSeconds(teleportCooldown);
        canTeleport = true;
    }

    public void BeforeTeleport()
    {
        fader.FadeInSpeed = TeleportFadeSpeed;
        fader.DoFadeIn();
    }

    public void AfterTeleport()
    {
        fader.DoFadeOut();
    }

}