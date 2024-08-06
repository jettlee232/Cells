using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
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

    private bool flag_audioFade = false;
    private float audioVol_origin;

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
        flag_audioFade = false;

        audioSrc = GetComponent<AudioSource>();
        audioSrc.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        audioSrc.pitch = PlayerPrefs.GetFloat("Pitch", 1f);

        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f); // New Code - For SFX Volume        

        // 이전 씬 이름 업데이트
        previousSceneName = currentSceneName;        
        
        // 현재 씬 이름 업데이트
        currentSceneName = scene.name;
       
        curSceneNum = scene.buildIndex;

        bool musicOnGoingFlag = CheckPrevSceneForUnSelectMusic();
        if (musicOnGoingFlag == true) PlayMuysicByScene(currentSceneName); // PlayMusicByScene(curSceneNum)

        StartCoroutine(ExecuteAfterSceneLoad()); // 코루틴을 통해 약간의 지연 후 명령 실행
    }

    // New SYS Code - Not Using Anymore
    public void PlayMusicByScene(int scenenum)
    {
        if (scenenum >= 0 && scenenum < bgmClips.Length)
        {
            if (audioSrc.isPlaying) audioSrc.Stop();

            audioSrc.clip = bgmClips[scenenum];
            audioSrc.Play();
        }
    }

    // New SYS Code
    public void PlayMuysicByScene(string scenename)
    {
        switch (scenename)
        {
            case "02_0_Lobby_Cutscenes1":
            case "02_0_Lobby_Cutscenes2":
            case "02_Lobby":
                if (audioSrc.isPlaying) audioSrc.Stop();
                audioSrc.clip = bgmClips[1];
                audioSrc.Play();
                break;
            case "03_0_CM_Cutscenes":
            case "03_1_CM_Tutorial":
            case "03_2_CM":
                if (audioSrc.isPlaying) audioSrc.Stop();
                audioSrc.clip = bgmClips[2];
                audioSrc.Play();
                break;
            case "04_StageMap":
                if (audioSrc.isPlaying) audioSrc.Stop();
                audioSrc.clip = bgmClips[3];
                audioSrc.Play();
                break;
            case "05_0_Mito_Cutscene":
            case "05_1_Mito_Tutorial":
            case "05_2_Mito":
                if (audioSrc.isPlaying) audioSrc.Stop();
                audioSrc.clip = bgmClips[4];
                audioSrc.Play();
                break;
            case "06_Lys":
            case "06_Lys_Cutscene":
            case "06_Lys_Tutorial":
                if (audioSrc.isPlaying) audioSrc.Stop();
                audioSrc.clip = bgmClips[5];
                audioSrc.Play();
                break;
            default:
                Debug.Log("No Scene Name");
                break;
        }
    }

    // New SYS Code
    bool CheckPrevSceneForUnSelectMusic()
    {
        if (currentSceneName == "02_Lobby" && previousSceneName == "02_0_Lobby_Cutscenes1") return false;
        else if (currentSceneName == "02_0_Lobby_Cutscenes2" && previousSceneName == "02_Lobby") return false;
        else if (currentSceneName == "02_Lobby" && previousSceneName == "02_0_Lobby_Cutscenes2") return false;
        else if (currentSceneName == "03_1_CM_Tutorial" && previousSceneName == "03_0_CM_Cutscenes") return false;
        else if (currentSceneName == "03_2_CM" && previousSceneName == "03_1_CM_Tutorial") return false;
        else if (currentSceneName == "03_1_CM_Tutorial" && previousSceneName == "03_0_CM_Cutscenes") return false;
        else if (currentSceneName == "05_1_Mito_Tutorial" && previousSceneName == "05_0_Mito_Cutscene") return false;
        else if (currentSceneName == "05_2_Mito" && previousSceneName == "05_1_Mito_Tutorial") return false;
        else if (currentSceneName == "06_Lys_Tutorial" && previousSceneName == "06_Lys_Cutscene") return false;
        else if (currentSceneName == "06_Lys" && previousSceneName == "06_Lys_Tutorial") return false;

        return true;
    }

    public void ControllVolume(float vol)
    {
        audioSrc.volume = vol;

        if (audioSrc.volume < 0f) audioSrc.volume = 0f;
        else if (audioSrc.volume > 1f) audioSrc.volume = 1f;

        PlayerPrefs.SetFloat("Volume", audioSrc.volume);
        audioVol_origin = audioSrc.volume;
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
        CutSceneController_SM csc;

        if (currentSceneName == "04_StageMap")
        {
            // GameManager 오브젝트가 준비될 때까지 대기
            GameObject gameManager = null;
            while (gameManager == null)
            {
                gameManager = GameObject.Find("CutSceneMgr");
                yield return null; // 매 프레임마다 체크
            }
            csc = gameManager.GetComponent<CutSceneController_SM>();
            SM_SceneLoadMech(csc); // 특정 조건에 따른 명령 실행
        }
    }

    // New SYS Code
    public void SM_SceneLoadMech(CutSceneController_SM csControl)
    {
        if (previousSceneName == "03_2_CM")
        {
            csControl.LoadFromCM();            
        }
        else if (previousSceneName == "05_2_Mito" || previousSceneName == "05_1_Mito_Tutorial" || previousSceneName == "05_0_Mito_Cutscene")
        {
            csControl.LoadFromOtherScene(0);
        }
        else if (previousSceneName == "06_Lys" || previousSceneName == "06_Lys_Cutscene" || previousSceneName == "06_Lys_Tutorial")
        {
            csControl.LoadFromOtherScene(1);
        }
    }

    // New SYS Code
    public void AudioFade()
    {
        StartCoroutine(AudioFadeCoroutine());
    }

    // New SYS Code
    private IEnumerator AudioFadeCoroutine()
    {
        audioVol_origin = audioSrc.volume;
        flag_audioFade = true;
        while (flag_audioFade)
        {            
            audioSrc.volume -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        audioSrc.volume = audioVol_origin;
    }
}
