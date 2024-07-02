using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InstantiateTween_CM : MonoBehaviour
{
    private Vector3 initialScale;

    public float rotSpeed = 360f;
    public float minRotSpeed = 90f;

    public float moveDuration = 2.5f;
    private float moveDistance = 5f; // 이동 거리

    public bool moveTowardsPlayerFlag = true;
    public bool grabbale = true;
    public bool leftOrRight = false;
    private float gap = 0.3f;

    [Header("Grabbable Enable Or Not")]
    public BNG.Grabbable grab;
    public SaberScript_CM saberScript;

    private bool tweenStopFlag = false;
    private Tween moveTween;
    private Tween scaleTween;
    private Tween rotateTween;


    void Start()
    {
        if (saberScript != null) saberScript.enabled = false;
        GoTween();
    }

    public void GoTween()
    {
        tweenStopFlag = false;
        if (moveTowardsPlayerFlag == true) MoveTowardsPlayer();

        BNG.Grabbable grab = GetComponent<BNG.Grabbable>();
        if (grabbale == true)
        {
            if (grab == null) grab = transform.GetChild(0).GetComponent<BNG.Grabbable>();
            grab.enabled = false;
        }


        initialScale = transform.localScale;
        if (moveTowardsPlayerFlag == false && grabbale == false)
        {
            initialScale = new Vector3(0.25f, 0.25f, 0.25f);
        }

        transform.localScale = Vector3.one * 0.001f;
        scaleTween = transform.DOScale(initialScale, 1.5f).SetEase(Ease.OutCubic);

        rotateTween = transform.DORotate(new Vector3(0f, 360f, 0f), 2f, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad)
            .OnUpdate(() =>
            {
                float currentSpeed = Mathf.Lerp(rotSpeed, minRotSpeed, transform.rotation.eulerAngles.y / 360f);
                transform.Rotate(Vector3.up * Time.deltaTime * currentSpeed);
            })
            .OnComplete(() =>
            {
                if (grabbale == true) grab.enabled = true;
                if (saberScript != null) saberScript.enabled = true;
            });
    }

    void MoveTowardsPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("MainCamera");
        if (player != null)
        {
            Vector3 targetPosition = player.transform.position;
            if (leftOrRight == false) targetPosition = new Vector3(targetPosition.x - gap, targetPosition.y, targetPosition.z);
            else targetPosition = new Vector3(targetPosition.x + gap, targetPosition.y, targetPosition.z);
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            Vector3 targetPositionAlongDirection = transform.position + moveDirection * moveDistance;

            moveTween = transform.DOMove(targetPositionAlongDirection, moveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {

                });
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerArea") && tweenStopFlag == false)
        {
            tweenStopFlag = true;
            moveTween.Kill();
        }
    }

    public bool IsThisRemade()
    {
        if (moveTween != null && rotateTween != null && scaleTween != null)
        {
            Debug.Log("True");
            return true;
        }
        else
        {
            Debug.Log("False");
            return false;
        }

    }
}
