using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelLookPlayer_StageMap : MonoBehaviour
{
    [Header("�÷��̾ �ٶ��� ���� ���ϴ� ��")]
    public RectTransform panelRectTrns;
    public Transform playerTrns;
    Vector3 directionToPlayer;
    Vector3 oppositeDirection;
    Quaternion lookRotation;


    private void Start()
    {
        if (playerTrns == null) playerTrns = GameObject.FindGameObjectWithTag("MainCamera").transform;  
    }

    void Update()
    {
        LookingPlayer();
    }

    void LookingPlayer()
    {
        directionToPlayer = playerTrns.position - transform.position;
        oppositeDirection = -directionToPlayer;
        lookRotation = Quaternion.LookRotation(oppositeDirection);

        panelRectTrns.rotation = lookRotation;
    }
}
