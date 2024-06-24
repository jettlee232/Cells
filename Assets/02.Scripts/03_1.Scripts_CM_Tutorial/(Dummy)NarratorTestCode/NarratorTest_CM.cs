using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorTest_CM : MonoBehaviour
{
    public AudioSource narratorMgr;
    public AudioClip[] audioClips;

    private void Start()
    {
        narratorMgr = GetComponent<AudioSource>();
    }

    public void PlayingClips(int i)
    {
        narratorMgr.PlayOneShot(audioClips[i]);
    }
}
