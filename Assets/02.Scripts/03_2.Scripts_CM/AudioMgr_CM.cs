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



    public AudioSource bgmSource; // Inspector���� �ʱ�ȭ
    public AudioSource clipSource; // Inspector���� �ʱ�ȭ
    public AudioClip bgm; // Inspector���� �ʱ�ȭ
    public AudioClip[] audioClips; // Inspector���� �ʱ�ȭ

    void Start()
    {
        PlayBGM();
    }

    void PlayBGM()
    {
        // bgm ���        
        bgmSource.loop = true;
        bgmSource.clip = bgm;
    }
    
    public void PlaySFX(int i)
    {
        clipSource.PlayOneShot(audioClips[i]);
    }
}
