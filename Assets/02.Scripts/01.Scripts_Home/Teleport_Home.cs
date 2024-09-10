using BNG;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.XR;

public class Teleport_Home : MonoBehaviour
{
    public ScreenFader fader;
    // SYS Code
    public MyFader_CM myFader;
    private bool faderFlag = true; // true - SF, false - MF

    public float TeleportFadeSpeed = 10.0f;

    public LineRenderer teleportLine;
    public Transform teleportGoal;
    public AudioClip teleportSoundEffect;

    //private bool triggerPressed;
    public bool canTeleport = true;
    public float teleportCooldown = 1.0f; // 텔레포트 딜레이

    private Vector2 joystickInput;
    private bool isTeleporting = false;

    void Start()
    {
        fader = GameObject.FindWithTag("MainCamera").GetComponent<ScreenFader>();
        if (fader != null) faderFlag = true;
        myFader = GameObject.FindWithTag("MainCamera").GetComponent<MyFader_CM>();
        if (myFader != null) faderFlag = false;

        teleportLine = GetComponentInChildren<LineRenderer>();

        teleportLine.enabled = false;
        teleportGoal.gameObject.SetActive(false);
    }

    void Update()
    {
        if (teleportGoal != null)
        {
            teleportGoal.localPosition = teleportLine.GetPosition(1);
            //Debug.Log("텔포라인겟포지션1" +teleportLine.GetPosition(1));
        }

        //InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed);

        //if (triggerPressed && canTeleport)
        //{
        //    StartCoroutine(TeleportPlayer());
        //}

        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out joystickInput);

        if (!isTeleporting && Mathf.Abs(joystickInput.x) > 0.9f || Mathf.Abs(joystickInput.y) > 0.9f && canTeleport)
        {
            StartTeleportPreview();
        }

        if (isTeleporting && Mathf.Abs(joystickInput.x) < 0.1f && Mathf.Abs(joystickInput.y) < 0.1f)
        {
            StartCoroutine(TeleportPlayer());
        }
    }

    void StartTeleportPreview()
    {
        isTeleporting = true;
        teleportLine.enabled = true;
        teleportGoal.gameObject.SetActive(true);
    }

    IEnumerator TeleportPlayer()
    {
        isTeleporting = false;
        teleportLine.enabled = false;
        teleportGoal.gameObject.SetActive(false);

        if (teleportSoundEffect != null)
        {
            //AudioSource.PlayClipAtPoint(teleportSoundEffect, transform.root.GetChild(0).position);
            AudioMgr_CM.Instance.PlaySFXByInt(17);
        }

        if (faderFlag == true) BeforeTeleport();
        else BeforeTeleport_MF();

        yield return new WaitForSeconds(0.1f);

        //Debug.Log("Before : " + transform.root.GetChild(0).position);
        //Debug.Log("Before : " + teleportGoal.position);
        transform.root.GetChild(0).position = teleportGoal.position;
        //Debug.Log("After : " + transform.root.GetChild(0).position);
        //Debug.Log("After : " + teleportGoal.position);
        StartCoroutine(TeleportCooldown());

        yield return new WaitForSeconds(0.25f);

        if (faderFlag == true) AfterTeleport();
        else AfterTeleport_MF();
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

    public void BeforeTeleport_MF()
    {
        myFader.FadeInSpeed = TeleportFadeSpeed;
        myFader.DoFadeIn();
    }

    public void AfterTeleport_MF()
    {
        myFader.DoFadeOut();
    }
}