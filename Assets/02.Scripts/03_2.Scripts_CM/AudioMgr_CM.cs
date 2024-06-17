using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr_CM : MonoBehaviour
{
    private static AudioMgr_CM instance = null;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    public static AudioMgr_CM Instance
    {
        get
        {
            if (null == instance) return null;
            return instance;
        }
    }



    public AudioSource bgmSource; // Inspector에서 초기화
    public AudioSource clipSource; // Inspector에서 초기화
    public AudioClip bgm; // Inspector에서 초기화
    public AudioClip[] audioClips; // Inspector에서 초기화

    void Start()
    {
        PlayBGM();
    }

    void PlayBGM()
    {
        // bgm 재생        
        bgmSource.loop = true;
        bgmSource.clip = bgm;
    }
    
    public void PlaySFX(int i)
    {
        clipSource.PlayOneShot(audioClips[i]);
    }
}
