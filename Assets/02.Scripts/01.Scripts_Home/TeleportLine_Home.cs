using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TeleportLine_Home : MonoBehaviour
{
    //Teleport_Home teleport_Home;
    //public GameObject teleportGoal;
    //public Material[] teleportMaterial;

    public float maxLineLength = 10f; // 텔레포트 최대 길이
    public float minLineLength = 1f; // 텔레포트 최소 길이
    private float currentLineLength = 1f; // 현재 텔레포트 길이

    private Vector2 joystickInput;
    public float lengthStep = 0.5f; // 라인 길이 증가/감소 스텝
    private bool canChangeLength = true;
    public float changeDelay = 0.1f; // 길이 변경 후 딜레이
    private bool leftTriggerPressed;
    private bool rightTriggerPressed;

    public LineRenderer teleportLine;
    public CapsuleCollider capsuleCollider;

    void Start()
    {
        //teleport_Home = GetComponentInParent<Teleport_Home>();
        //teleportGoal = transform.Find("TeleportGoal").gameObject;

        teleportLine = GetComponent<LineRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        capsuleCollider.center = new Vector3(0, 0, teleportLine.GetPosition(1).z / 2.0f);
        capsuleCollider.height = teleportLine.GetPosition(1).z;
    }

    void Update()
    {
        /*
        // 왼쪽 조이스틱 입력 값을 가져옴
        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out joystickInput);

        // 조이스틱 좌우 이동으로 라인 길이 변경
        if (canChangeLength)
        {
            if (joystickInput.x > 0.1f)
            {
                ChangeLineLength(lengthStep);
            }
            else if (joystickInput.x < -0.1f)
            {
                ChangeLineLength(-lengthStep);
            }
        }
        */

        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.triggerButton, out leftTriggerPressed);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out rightTriggerPressed);

        if (canChangeLength)
        {
            if (leftTriggerPressed)
            {
                ChangeLineLength(-lengthStep);
            }
            else if (rightTriggerPressed)
            {
                ChangeLineLength(lengthStep);
            }
        }

        currentLineLength = Mathf.Clamp(currentLineLength, minLineLength, maxLineLength);

        teleportLine.SetPosition(0, transform.localPosition);
        teleportLine.SetPosition(1, new Vector3(0, 0, currentLineLength));

        capsuleCollider.center = new Vector3(0, 0, teleportLine.GetPosition(1).z / 2.0f);
        capsuleCollider.height = teleportLine.GetPosition(1).z;
    }

    void ChangeLineLength(float length)
    {
        currentLineLength += length; // 길이 변경
        StartCoroutine(LineLengthChangeCooldown());
    }

    IEnumerator LineLengthChangeCooldown()
    {
        canChangeLength = false;
        yield return new WaitForSeconds(changeDelay);
        canChangeLength = true;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    teleportGoal.GetComponent<MeshRenderer>().material = teleportMaterial[0];
    //    teleport_Home.canTeleport = false;
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    teleportGoal.GetComponent<MeshRenderer>().material = teleportMaterial[1];
    //    teleport_Home.canTeleport = true;
    //}
}
