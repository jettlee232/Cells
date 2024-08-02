using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioMgr_CM : MonoBehaviour
{
    private static AudioMgr_CM instance = null;
    private int curSceneNum;

    [Header("Audio Source")]
    public AudioSource audioSrc;

    [Header("BGM List")]
    public AudioClip[] bgmClips;

    [Header("SFX List")]
    public AudioClip[] sfxClips;
    public float sfxVolume; // New Code - For SFX Volume

    [Header("Scene Names")]
    public string previousSceneName;
    public string currentSceneName;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject); // Maybe Later?
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        // 초기화
        previousSceneName = "None"; // 최초 실행시 이전 씬은 없음
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    public static AudioMgr_CM Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("1. curSceneName : " + currentSceneName + " / previousSceneName : " + previousSceneName);

        // 이전 씬 이름 업데이트
        previousSceneName = currentSceneName;

        // 현재 씬 이름 업데이트
        currentSceneName = scene.name;

        audioSrc = GetComponent<AudioSource>();

        audioSrc.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        audioSrc.pitch = PlayerPrefs.GetFloat("Pitch", 1f);

        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f); // New Code - For SFX Volume

        curSceneNum = scene.buildIndex;

        PlayMusicByScene(curSceneNum); // Not Yet

        Debug.Log("2. curSceneName : " + currentSceneName + " / previousSceneName : " + previousSceneName);
        StartCoroutine(ExecuteAfterSceneLoad()); // 코루틴을 통해 약간의 지연 후 명령 실행
    }
    public void PlayMusicByScene(int scenenum)
    {
        if (scenenum >= 0 && scenenum < bgmClips.Length)
        {
            if (audioSrc.isPlaying) audioSrc.Stop();

            audioSrc.clip = bgmClips[scenenum];
            audioSrc.Play();
        }
    }

    public void ControllVolume(float vol)
    {
        audioSrc.volume = vol;

        if (audioSrc.volume < 0f) audioSrc.volume = 0f;
        else if (audioSrc.volume > 1f) audioSrc.volume = 1f;

        PlayerPrefs.SetFloat("Volume", audioSrc.volume);
    }

    public void ControllSFXVolume(float vol) // New Code - For SFX Volume
    {
        sfxVolume = vol;

        if (sfxVolume < 0f) sfxVolume = 0f;
        else if (sfxVolume > 1f) sfxVolume = 1f;

        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void PauseOrRestart()
    {
        if (audioSrc.isPlaying) audioSrc.Pause();
        else if (!audioSrc.isPlaying) audioSrc.Play();
    }

    public void LoadingNewMusic(AudioClip newclip, bool gonow)
    {
        audioSrc.clip = newclip;

        if (gonow) audioSrc.Play();        
    }

    public void ControllMusicSpeedByBool(bool upOrDown)
    {
        if (upOrDown == true) audioSrc.pitch += 0.2f;
        else if (upOrDown == false) audioSrc.pitch -= 0.2f;

        if (audioSrc.pitch > 1f) audioSrc.pitch = 1f;
        else if (audioSrc.pitch < 0f) audioSrc.pitch = 0f;

        PlayerPrefs.SetFloat("Pitch", audioSrc.pitch);
    }

    public void ControllMusicSpeedByFloat(float speed)
    {
        audioSrc.pitch = speed;

        PlayerPrefs.SetFloat("Pitch", audioSrc.pitch);
    }

    public void PlaySFXByInt(double d)
    {
        int i = (int)d;
        audioSrc.PlayOneShot(sfxClips[i], sfxVolume); // New Code - For SFX Volume
    }

    public void PlaySFXByString(string name)
    {
        for (int i = 0; i < sfxClips.Length; i++)
        {
            if (sfxClips[i].name == name)
            {
                audioSrc.PlayOneShot(sfxClips[i], sfxVolume); // New Code - For SFX Volume
            }
        }
    }

    // New SYS Code
    private IEnumerator ExecuteAfterSceneLoad()
    {
        Debug.Log("In Coroutine");
        CutSceneController_SM csc;

        if (currentSceneName == "04_StageMap")
        {
            Debug.Log("In 04");

            // GameManager 오브젝트가 준비될 때까지 대기
            GameObject gameManager = null;
            while (gameManager == null)
            {
                gameManager = GameObject.Find("CutSceneMgr");
                yield return null; // 매 프레임마다 체크
            }
            Debug.Log("Found CSM");
            csc = gameManager.GetComponent<CutSceneController_SM>();
            SM_SceneLoadMech(csc); // 특정 조건에 따른 명령 실행
        }
    }

    // New SYS Code
    public void SM_SceneLoadMech(CutSceneController_SM csControl)
    {
        if (previousSceneName == "03_2_CM")
        {
            // 03_SM에서 04.CM으로 전환 시 실행할 명령
            Debug.Log("From 03");
            csControl.LoadFromCM();
        }
        else if (previousSceneName == "05_2_Mito")
        {
            // 05_MT에서 04.CM으로 전환 시 실행할 명령
            Debug.Log("From 05");
            csControl.LoadFromMito();
        }
    }    
}
