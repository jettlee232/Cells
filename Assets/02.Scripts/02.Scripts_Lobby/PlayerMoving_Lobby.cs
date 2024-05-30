using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class PlayerMoving_Lobby : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float upSpeed = 5f;
    public float downSpeed = 2f;
    public float rotateValue = 30f;
    public Transform dirStandard;

    private Rigidbody rb;

    private bool goUp = false;
    private Vector2 XZMove = Vector2.zero;
    private Vector2 XRotate = Vector2.zero;
    private bool rotateCoroutine;

    UnityEngine.XR.InputDevice right;
    UnityEngine.XR.InputDevice left;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rotateCoroutine = false;
    }

    void Update()
    {
        getRotate();
        rb.velocity = (getUp() + getMove());
    }

    private Vector3 getUp()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out goUp);

        if (goUp) { return Vector3.up * upSpeed; }
        else { return Vector3.down * downSpeed; }
    }

    private Vector3 getMove()
    {
        left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        left.TryGetFeatureValue(CommonUsages.primary2DAxis, out XZMove);

        Quaternion guideRot = Quaternion.Euler(0, dirStandard.eulerAngles.y, 0);
        Vector3 moveDir = new Vector3(XZMove.x, 0f, XZMove.y);
        moveDir = guideRot * moveDir;
        return moveDir * moveSpeed;
    }

    private void getRotate()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.primary2DAxis, out XRotate);

        if (!rotateCoroutine && (XRotate.x >= 0.2f || XRotate.x <= -0.2f)) { rotateCoroutine = true; StartCoroutine(rotate(XRotate.x)); }
        else { return; }
    }

    IEnumerator rotate(float x)
    {
        if (x >= 0f) { transform.rotation = transform.rotation * Quaternion.Euler(0f, rotateValue, 0f); }
        else { transform.rotation = transform.rotation * Quaternion.Euler(0f, -rotateValue, 0f); }
        yield return new WaitForSeconds(0.3f);
        rotateCoroutine = false;
    }
}
