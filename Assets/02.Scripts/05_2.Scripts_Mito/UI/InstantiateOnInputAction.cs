using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InstantiateOnInputAction : MonoBehaviour
{
    public InputActionReference InputAction = default;
    public GameObject InstantiateObject = default;
    public Transform InstantiatePos;

    private void OnEnable()
    {
        InputAction.action.performed += ToggleActive;
    }

    private void OnDisable()
    {
        InputAction.action.performed -= ToggleActive;
    }

    public void ToggleActive(InputAction.CallbackContext context)
    {
        if (InstantiateObject)
        {
            Instantiate(InstantiateObject, InstantiatePos);
        }
    }
}
