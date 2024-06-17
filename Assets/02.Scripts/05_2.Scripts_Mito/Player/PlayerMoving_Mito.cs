using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using static PixelCrushers.DialogueSystem.SequencerShortcuts;
using static UnityEngine.GraphicsBuffer;

public class PlayerMoving_Mito : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float upSpeed = 5f;
    public float downSpeed = 2f;
    public float rotateValue = 30f;
    public float groundCheck = 0.02f;
    public float maxSlopeAngle = 45f;
    public bool flyable = true;
    public Transform dirStandard;

    private Rigidbody rb;

    private bool goUp = false;
    private Vector2 XZMove = Vector2.zero;
    private Vector2 XRotate = Vector2.zero;
    private bool rotateCoroutine;

    private int groundLayer;
    private RaycastHit slopeHit;

    UnityEngine.XR.InputDevice right;
    UnityEngine.XR.InputDevice left;

    public GameObject inventoryUI; // �κ��丮 UI ����
    public Transform cameraTransform; // �÷��̾� ī�޶� ����
    private bool isInventoryUIActive = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rotateCoroutine = false;
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
    }

    void Update()
    {
        GetRotate();
        rb.velocity = GetUp() + GetMove();

    }

    // �κ��丮 UI ��ġ�� ������Ʈ�ϴ� �޼���
    private void UpdateInventoryUIPosition()
    {
        if (inventoryUI.activeSelf)
        {
            Vector3 uiPosition = cameraTransform.position + cameraTransform.forward * 2f + cameraTransform.up * -0.5f; // �÷��̾� �տ� 2���� ��ġ, ��¦ �Ʒ���
            inventoryUI.transform.position = uiPosition;
            inventoryUI.transform.rotation = Quaternion.LookRotation(cameraTransform.forward); // �÷��̾ ���� �������� ȸ��
        }
    }

    private Vector3 GetUp()
    {
        if (flyable)
        {
            // �� ���� �κ� Start���� ������ �۵� �� �ؿ� ��ġ �ű�� X... left�� ��������
            left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            // ���� ��ư �ٲٸ� ����ã�⿡�� �� �� ��������...?? �޼����� �ٲٽ� �Ÿ� GetMove �κ� �����ϼ���
            left.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out goUp);
        }
        
        if (goUp) { return Vector3.up * upSpeed; }
        else
        {
            if (CheckSlope()) { return Vector3.zero; }
            else { return Vector3.down * downSpeed; }
        }
    }

    private Vector3 GetMove()
    {
        left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        left.TryGetFeatureValue(CommonUsages.primary2DAxis, out XZMove);

        Quaternion guideRot = Quaternion.Euler(0, dirStandard.eulerAngles.y, 0);
        Vector3 moveDir = new Vector3(XZMove.x, 0f, XZMove.y);
        moveDir = guideRot * moveDir;
        if (CheckSlope()) { return AdjustSlope(moveDir) * moveSpeed; }
        else { return moveDir * moveSpeed; }
    }

    private void GetRotate()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.primary2DAxis, out XRotate);

        if (!rotateCoroutine && (XRotate.x >= 0.2f || XRotate.x <= -0.2f)) { rotateCoroutine = true; StartCoroutine(cRotate(XRotate.x)); }
        else { return; }
    }

    IEnumerator cRotate(float x)
    {
        if (x >= 0f) { transform.rotation = transform.rotation * Quaternion.Euler(0f, rotateValue, 0f); }
        else { transform.rotation = transform.rotation * Quaternion.Euler(0f, -rotateValue, 0f); }
        yield return new WaitForSeconds(0.3f);
        rotateCoroutine = false;
    }

    //private bool CheckGround()
    //{
    //    if (Physics.Raycast(transform.position, Vector3.down, groundCheck, groundLayer)) { return true; }
    //    else { return false; }
    //}
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
