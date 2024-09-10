using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGetStarEffect_Home : MonoBehaviour
{
    public Transform starWatch;
    float moveDuration = 1.5f;

    private ParticleSystem particleSystem;

    private void Start()
    {
        DOTween.Init();
        particleSystem = GetComponent<ParticleSystem>();

        starWatch = GameObject.FindGameObjectWithTag("Watch").transform;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = starWatch.position;

        transform.DOMove(endPosition, moveDuration).OnComplete(OnParticleReachedEnd);
    }

    private void OnParticleReachedEnd()
    {
        if (particleSystem != null)
        {
            Destroy(gameObject);
        }
    }
}
