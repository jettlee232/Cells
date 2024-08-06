using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldUI_Mito : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject player;
    public float maxDistance = 5f;

    public TMP_Text textField;
    public TMP_InputField inputField;
    public GameObject mainImage;

    private Transform originalParent;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isDetached = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (targetObject != null)
        {
            originalParent = targetObject.transform.parent;
            originalPosition = targetObject.transform.localPosition;
            originalRotation = targetObject.transform.localRotation;
        }
    }

    private void Update()
    {
        if (targetObject != null)
        {
            if (targetObject.activeSelf && !isDetached)
            {
                DetachFromParent();
            }
            else if (!targetObject.activeSelf && isDetached)
            {
                ReattachToParent();
            }

            if (isDetached)
            {
                CheckDistance();
            }
        }
    }

    private void DetachFromParent()
    {
        targetObject.transform.SetParent(null);

        if (mainImage != null)
        {
            textField.text = "";
            inputField.text = "";
            mainImage.SetActive(true);
        }

        isDetached = true;
    }

    private void ReattachToParent()
    {
        targetObject.transform.SetParent(originalParent);
        targetObject.transform.localPosition = originalPosition;
        targetObject.transform.localRotation = originalRotation;

        if (mainImage != null)
        {
            textField.text = "";
            inputField.text = "";
        }

        isDetached = false;
    }

    private void CheckDistance()
    {
        float distance = Vector3.Distance(player.transform.position, targetObject.transform.position);
        if (distance > maxDistance)
        {
            targetObject.SetActive(false);
            ReattachToParent();
        }
    }
}
