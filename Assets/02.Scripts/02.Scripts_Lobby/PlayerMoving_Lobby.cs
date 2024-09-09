using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using static PixelCrushers.DialogueSystem.SequencerShortcuts;
using static UnityEngine.GraphicsBuffer;

public class PlayerMoving_Lobby : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float downSpeed = 2f;
    public float rotateSpeed = 50f;
    public float groundCheck = 0.02f;
    public float maxSlopeAngle = 45f;
    public Transform dirStandard;

    private Rigidbody rb;

    private Vector2 XZMove = Vector2.zero;
    private Vector2 XRotate = Vector2.zero;
    private Quaternion nowTrans;

    private int groundLayer;
    private RaycastHit slopeHit;

    UnityEngine.XR.InputDevice right;
    UnityEngine.XR.InputDevice left;

    public bool snapturn = false;

    public float minInput = 0.6f; // 이건 역치입니다... 어느정도 조이스틱 움직임이 들어와야 회전할지

    private bool rotateCoroutineY = false;
    private bool rotateCoroutineX = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
    }

    void Update()
    {
        left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        GetRotateY();
        rb.velocity = GameManager_Lobby.instance.PlayerMove() ? GetDown() + GetMove() : Vector3.zero;
    }

    private Vector3 GetDown()
    {
        if (CheckSlope() || CheckGround()) { return Vector3.zero; }
        else { return Vector3.down * downSpeed; }
    }

    private Vector3 GetMove()
    {
        left.TryGetFeatureValue(CommonUsages.primary2DAxis, out XZMove);

        Quaternion guideRot = Quaternion.Euler(0, dirStandard.eulerAngles.y, 0);
        Vector3 moveDir = new Vector3(XZMove.x, 0f, XZMove.y);
        moveDir = guideRot * moveDir;
        if (CheckSlope()) { return AdjustSlope(moveDir) * moveSpeed; }
        else { return moveDir * moveSpeed; }
    }

    /*
    private void GetRotate()
    {
        right.TryGetFeatureValue(CommonUsages.primary2DAxis, out XRotate);

        nowTrans = transform.rotation * Quaternion.Euler(0f, XRotate.x * rotateSpeed * Time.deltaTime, 0f);
        transform.rotation = nowTrans;
    }
    */

    private void GetRotateY()
    {

        right.TryGetFeatureValue(CommonUsages.primary2DAxis, out XRotate);

        if (snapturn)
        {
            if (!rotateCoroutineY && (XRotate.x >= minInput || XRotate.x <= -minInput)) { rotateCoroutineY = true; StartCoroutine(rotateY(XRotate.x)); }
            else { return; }
        }
        else
        {
            nowTrans = transform.rotation * Quaternion.Euler(0f, XRotate.x * rotateSpeed * Time.deltaTime, 0f);
            transform.rotation = nowTrans;
        }
    }
    IEnumerator rotateY(float x)
    {
        if (x >= 0f) { transform.rotation = transform.rotation * Quaternion.Euler(0f, 30f, 0f); }
        else { transform.rotation = transform.rotation * Quaternion.Euler(0f, -30f, 0f); }
        yield return new WaitForSeconds(0.3f);
        rotateCoroutineY = false;
    }

    public void RotateUp() { rotateSpeed += 10f; }
    public void RotateDown()
    {
        if (rotateSpeed > 10f) { rotateSpeed -= 10f; }
    }

    private bool CheckGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, groundCheck, groundLayer)) { return true; }
        else { return false; }
    }
    private bool CheckSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out slopeHit, groundCheck, groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if (angle != 0f && angle <= maxSlopeAngle) { return true; }
        }
        return false;
    }
    private Vector3 AdjustSlope(Vector3 dir)
    {
        return Vector3.ProjectOnPlane(dir, slopeHit.normal).normalized;
    }
}
