using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance = null; // 싱글톤화용 인스턴스 변수
    private int curSceneNum; // 현재 씬 번호

    [Header("Audio Source")]
    public AudioSource audioSrc; // 배경 음악 오디오 소스    

    [Header("BGM List")]
    public AudioClip[] bgmClips; // 씬에 따라 다른 bgm 리스트

    [Header("SFX List")]
    public AudioClip[] sfxClips; // 효과음 리스트

    // ***여기부터 싱글톤 목적 스크립트
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

    private void Start()
    {

    }

    public static AudioManager Instance
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
    // ***여기까지가 싱글톤 목적 스크립트

    // ***여기부터 씬 로드되면 자동 실행되게 작성한 스크립트
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
        audioSrc = GetComponent<AudioSource>();

        audioSrc.volume = PlayerPrefs.GetFloat("Volume", 0.5f); // 이건 PlayerPrefs로 저장, 기본값은 0.5
        audioSrc.pitch = PlayerPrefs.GetFloat("Pitch", 0.5f); // 이건 PlayerPrefs로 저장, 기본값은 0.5

        // Scene의 인덱스를 업데이트
        curSceneNum = scene.buildIndex;

        // Scene에 따라 다른 음악 재생
        PlayMusicByScene(curSceneNum);
    }
    // ***여기부터 씬 로드되면 자동 실행되게 작성한 스크립트

    // ***번호에 따라 다른 음악 실행, 씬 호출될때 마다 실행되기도 함
    public void PlayMusicByScene(int scenenum)
    {
        if (scenenum >= 0 && scenenum < bgmClips.Length)
        {
            if (audioSrc.isPlaying) audioSrc.Stop(); // 지금 실행되고 있는 음악 강제로 멈추고

            audioSrc.clip = bgmClips[scenenum]; // 새로운 음악 소스에다 넣고 돌리기
            audioSrc.Play();
        }
    }

    // ***볼륨 체인지
    public void ControllVolume(float vol)
    {
        audioSrc.volume = vol;

        // 밑에 두줄은 걍 예외처리, 없어도 그만이긴 함
        if (audioSrc.volume < 0f) audioSrc.volume = 0f;
        else if (audioSrc.volume > 1f) audioSrc.volume = 1f;

        PlayerPrefs.SetFloat("Volume", audioSrc.volume); // 이건 PlayerPrefs로 저장, 기본값은 0.5
    }

    // ***일시 정지 혹은 재생
    public void PauseOrRestart()
    {
        if (audioSrc.isPlaying) audioSrc.Pause();
        else if (!audioSrc.isPlaying) audioSrc.Play();
    }

    // ***새로운 음악 넣기 (새로 넣을 음악, 바로 재생할지 말지 결정)
    public void LoadingNewMusic(AudioClip newclip, bool gonow)
    {
        audioSrc.clip = newclip;

        if (gonow) audioSrc.Play();
    }

    // ***음악 속도 조절, 0.2배속으로 증감 (up이면 증가, down이면 감소)
    public void ControllMusicSpeedByBool(bool upOrDown)
    {
        if (upOrDown == true) audioSrc.pitch += 0.2f;
        else if (upOrDown == false) audioSrc.pitch -= 0.2f;

        if (audioSrc.pitch > 1f) audioSrc.pitch = 1f;
        else if (audioSrc.pitch < 0f) audioSrc.pitch = 0f;

        PlayerPrefs.SetFloat("Pitch", audioSrc.pitch); // 플레이어 프렙스로 현재 Pitch 값 저장해놓기
    }

    // ***음악 속도 조절, 숫자로 지정
    public void ControllMusicSpeedByFloat(float speed)
    {
        audioSrc.pitch = speed;

        PlayerPrefs.SetFloat("Pitch", audioSrc.pitch); // 플레이어 프렙스로 현재 Pitch 값 저장해놓기
    }

    // ***SFX 재생 (번호로 재생하는 거라 다들 몇번 사운드가 무슨 역할인지 기억해야 함, double로 하는 이유는 다이얼로그 시스템에서 호출 쉽게...)
    public void PlaySFXByInt(double d)
    {
        int i = (int)d;
        audioSrc.PlayOneShot(sfxClips[i]);
    }

    // ***SFC 재생 (string으로 재생하는 거라 사운드 파일 이름 기억해야 함)
    public void PlaySFXByString(string name)
    {
        for (int i = 0; i < sfxClips.Length; i++)
        {
            if (sfxClips[i].name == name)
            {
                audioSrc.PlayOneShot(sfxClips[i]);
            }
        }
    }
}
