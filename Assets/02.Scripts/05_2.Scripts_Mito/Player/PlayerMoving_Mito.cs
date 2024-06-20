using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class PlayerMoving_Mito : MonoBehaviour
{
    public bool flyable = true;

    public float startTimer = 0.4f; // 속도가 0일 때부터 최대 속도까지 빨라질 때 걸리는 시간 (정지 -> 이동 관성)
    public float finishTimer = 1f; // 속도가 최대일 때부터 0까지 느려질 때 걸리는 시간 (이동 -> 정지 관성)
    public float moveSpeed = 10f;    // 이동 속도 (전후좌우)
    public float upSpeed = 10f;      // 최대 이동 속도 (상승)
    private float nowUpSpeed = 0f;
    public float downSpeed = 6f;    // 최대 이동 속도 (하강)
    private float nowDownSpeed = 0f;
    public float rotateSpeed = 15f; // 시야 회전 속도
    public float updownLimit = 45f; // 키를 통해 시야를 어디까지 올리고 내릴 수 있는지
    public float resetTimer = 1f; // 얼마나 오래동안 버튼 눌러야 시점 영점 조절되는지
    private Quaternion nowTrans;
    private float yPressedTime = 0f;
    private float yReleasedTime = 0f;
    private bool pressedB = false;
    private bool oldPressedB = false;

    private float groundCheck = 0.02f;
    private float maxSlopeAngle = 45f;
    public Transform dirStandard;   // 여기에 센터아이 앵커
    public Transform trackingSpace; // 여기에 TrackingSpace

    private Rigidbody rb;

    private bool goUp = false;
    private bool oldUp = false;
    private bool goDown = false;
    private bool oldDown = false;
    private bool reset = false;
    private bool oldReset = false;
    private Vector2 XZMove = Vector2.zero;
    private Vector2 XRotate = Vector2.zero;

    private int groundLayer;
    private RaycastHit slopeHit;

    UnityEngine.XR.InputDevice right;
    UnityEngine.XR.InputDevice left;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
    }

    void Update()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        GetRotateY();
        //GetRotateX();
        CheckFlyable();
        rb.velocity = flyable ? GetUp() + GetDown() + GetMove() : GetMove();
        ResetRot();
    }

    #region 상승
    private Vector3 GetUp()
    {
        oldUp = goUp;

        right.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out goUp);

        if (goUp && !oldUp) { StartCoroutine(cStartUp()); }
        if (!goUp && oldUp) { StartCoroutine(cFinishUp()); }
        return new Vector3(0f, nowUpSpeed, 0f);
    }
    IEnumerator cStartUp()
    {
        float nowSpeed = nowUpSpeed;
        float totalTimer = ((upSpeed - nowSpeed) / upSpeed) * startTimer;
        float timer = 0f;

        if (nowSpeed != upSpeed)
        {
            while (true)
            {
                if (!goUp || !flyable) { yield break; }
                if (upSpeed - nowUpSpeed <= 0.1f) { nowUpSpeed = upSpeed; yield break; }
                timer += Time.deltaTime;
                nowUpSpeed = Mathf.Lerp(nowSpeed, upSpeed, timer / totalTimer);
                yield return null;
            }
        }
    }
    IEnumerator cFinishUp()
    {
        float nowSpeed = nowUpSpeed;
        float totalTimer = (nowSpeed / upSpeed) * finishTimer;
        float timer = 0f;

        if (nowSpeed != 0f)
        {
            while (true)
            {
                if (goUp || !flyable) { yield break; }
                if (nowUpSpeed <= 0.1f) { nowUpSpeed = 0f; yield break; }
                timer += Time.deltaTime;
                nowUpSpeed = Mathf.Lerp(nowSpeed, 0f, timer / totalTimer);
                yield return null;
            }
        }
    }
    #endregion

    #region 하강
    private Vector3 GetDown()
    {
        if (CheckGround()) { return Vector3.zero; }
        oldDown = goDown;

        left.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out goDown);

        if (goDown && !oldDown) { StartCoroutine(cStartDown()); }
        if (!goDown && oldDown) { StartCoroutine(cFinishDown()); }
        return new Vector3(0f, -nowDownSpeed, 0f);
    }
    IEnumerator cStartDown()
    {
        float nowSpeed = nowDownSpeed;
        float totalTimer = ((downSpeed - nowSpeed) / downSpeed) * startTimer;
        float timer = 0f;

        if (nowSpeed != downSpeed)
        {
            while (true)
            {
                if (!goDown || !flyable) { yield break; }
                if (downSpeed - nowDownSpeed <= 0.1f) { nowDownSpeed = downSpeed; yield break; }
                timer += Time.deltaTime;
                nowDownSpeed = Mathf.Lerp(nowSpeed, downSpeed, timer / totalTimer);
                yield return null;
            }
        }
    }
    IEnumerator cFinishDown()
    {
        float nowSpeed = nowDownSpeed;
        float totalTimer = (nowSpeed / downSpeed) * finishTimer;
        float timer = 0f;

        if (nowSpeed != 0f)
        {
            while (true)
            {
                if (goDown || !flyable) { yield break; }
                if (nowDownSpeed <= 0.1f) { nowDownSpeed = 0f; yield break; }
                timer += Time.deltaTime;
                nowDownSpeed = Mathf.Lerp(nowSpeed, 0f, timer / totalTimer);
                yield return null;
            }
        }
    }
    #endregion

    #region 앞뒤양옆
    private Vector3 GetMove()
    {
        left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        left.TryGetFeatureValue(CommonUsages.primary2DAxis, out XZMove);

        Quaternion guideRot = Quaternion.Euler(0, dirStandard.eulerAngles.y, 0);
        Vector3 moveDir = new Vector3(XZMove.x, 0f, XZMove.y);
        moveDir = guideRot * moveDir;
        return moveDir * moveSpeed;
    }
    #endregion

    #region 회전
    private void GetRotateY()
    {
        right.TryGetFeatureValue(CommonUsages.primary2DAxis, out XRotate);

        nowTrans = transform.rotation * Quaternion.Euler(0f, XRotate.x * rotateSpeed * Time.deltaTime, 0f);
        transform.rotation = nowTrans;
    }

    private void GetRotateX()
    {
        right.TryGetFeatureValue(CommonUsages.primary2DAxis, out XRotate);

        Vector3 currentEulerAngles = trackingSpace.rotation.eulerAngles;
        float newXRotation = currentEulerAngles.x - XRotate.y * rotateSpeed * Time.deltaTime;
        if (newXRotation > 180f) newXRotation -= 360f;
        newXRotation = Mathf.Clamp(newXRotation, -updownLimit, updownLimit);
        Quaternion newRotation = Quaternion.Euler(newXRotation, currentEulerAngles.y, currentEulerAngles.z);
        trackingSpace.rotation = newRotation;
    }

    private void ResetRot()
    {
        oldReset = reset;

        left.TryGetFeatureValue(CommonUsages.secondaryButton, out reset);

        if (!oldReset && reset) { yPressedTime = Time.time; }
        else if (oldReset && !reset)
        {
            yReleasedTime = Time.time;
            if (yReleasedTime - yPressedTime >= resetTimer)
            {
                Vector3 currentEulerAngles = trackingSpace.rotation.eulerAngles;

                float newXRotation = 0f;
                float newYRotation = currentEulerAngles.y + XRotate.x * rotateSpeed * Time.deltaTime;
                float newZRotation = currentEulerAngles.z;

                Quaternion newRotation = Quaternion.Euler(newXRotation, newYRotation, newZRotation);
                trackingSpace.rotation = newRotation;
            }
        }
    }
    #endregion

    #region 기타
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
    #endregion

    public void CheckFlyable()
    {
        oldPressedB = pressedB;

        right.TryGetFeatureValue(CommonUsages.secondaryButton, out pressedB);
        if (!flyable && (pressedB && !oldPressedB)) { flyable = true; }
        else if (flyable && (pressedB && !oldPressedB)) { flyable = false; }
    }
}