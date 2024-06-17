using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Mito : MonoBehaviour
{
    #region �׽�Ʈ��
    public float speed = 5.0f; // �̵� �ӵ�
    public float rotationSpeed = 720.0f; // ȸ�� �ӵ� (��/��)
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
        float moveDirectionY = Input.GetAxis("Vertical"); // ����/���� �Է� (��/�Ʒ� ����Ű)
        Vector3 move = transform.forward * moveDirectionY * speed * Time.deltaTime;
        characterController.Move(move);
    }

    void Rotate()
    {
        float rotationY = Input.GetAxis("Horizontal"); // ȸ�� �Է� (��/�� ����Ű)
        transform.Rotate(0, rotationY * rotationSpeed * Time.deltaTime, 0);
    }
    #endregion
}
