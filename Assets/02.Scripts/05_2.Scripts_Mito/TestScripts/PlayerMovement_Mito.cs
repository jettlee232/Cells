using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Mito : MonoBehaviour
{
    #region 테스트용
    public float speed = 5.0f; // 이동 속도
    public float rotationSpeed = 720.0f; // 회전 속도 (도/초)
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        float moveDirectionY = Input.GetAxis("Vertical"); // 전진/후진 입력 (위/아래 방향키)
        Vector3 move = transform.forward * moveDirectionY * speed * Time.deltaTime;
        characterController.Move(move);
    }

    void Rotate()
    {
        float rotationY = Input.GetAxis("Horizontal"); // 회전 입력 (좌/우 방향키)
        transform.Rotate(0, rotationY * rotationSpeed * Time.deltaTime, 0);
    }
    #endregion
}
