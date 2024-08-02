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
        // �ʱ�ȭ
        previousSceneName = "None"; // ���� ����� ���� ���� ����
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

        // ���� �� �̸� ������Ʈ
        previousSceneName = currentSceneName;

        // ���� �� �̸� ������Ʈ
        currentSceneName = scene.name;

        audioSrc = GetComponent<AudioSource>();

        audioSrc.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        audioSrc.pitch = PlayerPrefs.GetFloat("Pitch", 1f);

        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f); // New Code - For SFX Volume

        curSceneNum = scene.buildIndex;

        PlayMusicByScene(curSceneNum); // Not Yet

        Debug.Log("2. curSceneName : " + currentSceneName + " / previousSceneName : " + previousSceneName);
        StartCoroutine(ExecuteAfterSceneLoad()); // �ڷ�ƾ�� ���� �ణ�� ���� �� ��� ����
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

            // GameManager ������Ʈ�� �غ�� ������ ���
            GameObject gameManager = null;
            while (gameManager == null)
            {
                gameManager = GameObject.Find("CutSceneMgr");
                yield return null; // �� �����Ӹ��� üũ
            }
            Debug.Log("Found CSM");
            csc = gameManager.GetComponent<CutSceneController_SM>();
            SM_SceneLoadMech(csc); // Ư�� ���ǿ� ���� ��� ����
        }
    }

    // New SYS Code
    public void SM_SceneLoadMech(CutSceneController_SM csControl)
    {
        if (previousSceneName == "03_2_CM")
        {
            // 03_SM���� 04.CM���� ��ȯ �� ������ ���
            Debug.Log("From 03");
            csControl.LoadFromCM();
        }
        else if (previousSceneName == "05_2_Mito")
        {
            // 05_MT���� 04.CM���� ��ȯ �� ������ ���
            Debug.Log("From 05");
            csControl.LoadFromMito();
        }
    }    
}
