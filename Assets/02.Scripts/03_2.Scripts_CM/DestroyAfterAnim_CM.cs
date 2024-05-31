using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnim_CM : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    
    public void DestroyGameObject()
    {
        Destroy(transform.parent.gameObject);
    }

}
