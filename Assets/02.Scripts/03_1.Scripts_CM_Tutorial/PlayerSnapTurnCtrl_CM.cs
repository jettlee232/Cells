using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSnapTurnCtrl_CM : MonoBehaviour
{
    public PlayerRotation playerRotation;

    void Start()
    {
        playerRotation = GetComponent<PlayerRotation>();
    }

    public void OnSnapTurn()
    {
        playerRotation.RotationType = RotationMechanic.Snap;
    }

    public void OnSmoothTurn()
    {
        playerRotation.RotationType = RotationMechanic.Smooth;
    }
}
