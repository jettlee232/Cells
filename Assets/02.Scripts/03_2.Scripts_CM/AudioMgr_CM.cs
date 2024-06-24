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

    // ***������� �̱��� ���� ��ũ��Ʈ
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
    // ***��������� �̱��� ���� ��ũ��Ʈ

    // ***������� �� �ε�Ǹ� �ڵ� ����ǰ� �ۼ��� ��ũ��Ʈ
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

        audioSrc.volume = PlayerPrefs.GetFloat("Volume", 0.5f); // �̰� PlayerPrefs�� ����, �⺻���� 0.5
        audioSrc.pitch = PlayerPrefs.GetFloat("Pitch", 1f); // �̰� PlayerPrefs�� ����, �⺻���� 1

        // Scene�� �ε����� ������Ʈ
        curSceneNum = scene.buildIndex;

        // Scene�� ���� �ٸ� ���� ���
        // PlayMusicByScene(curSceneNum); // Not Yet
    }
    // ***������� �� �ε�Ǹ� �ڵ� ����ǰ� �ۼ��� ��ũ��Ʈ

    // ***��ȣ�� ���� �ٸ� ���� ����, �� ȣ��ɶ� ���� ����Ǳ⵵ ��
    public void PlayMusicByScene(int scenenum)
    {
        if (scenenum >= 0 && scenenum < bgmClips.Length)
        {
            if (audioSrc.isPlaying) audioSrc.Stop(); // ���� ����ǰ� �ִ� ���� ������ ���߰�

            audioSrc.clip = bgmClips[scenenum]; // ���ο� ���� �ҽ����� �ְ� ������
            audioSrc.Play();
        }
    }

    // ***���� ü����
    public void ControllVolume(float vol)
    {
        audioSrc.volume = vol;

        // �ؿ� ������ �� ����ó��, ��� �׸��̱� ��
        if (audioSrc.volume < 0f) audioSrc.volume = 0f;
        else if (audioSrc.volume > 1f) audioSrc.volume = 1f;

        PlayerPrefs.SetFloat("Volume", audioSrc.volume); // �̰� PlayerPrefs�� ����, �⺻���� 0.5
    }

    // ***�Ͻ� ���� Ȥ�� ���
    public void PauseOrRestart()
    {
        if (audioSrc.isPlaying) audioSrc.Pause();
        else if (!audioSrc.isPlaying) audioSrc.Play();
    }

    // ***���ο� ���� �ֱ� (���� ���� ����, �ٷ� ������� ���� ����)
    public void LoadingNewMusic(AudioClip newclip, bool gonow)
    {
        audioSrc.clip = newclip;

        if (gonow) audioSrc.Play();
    }

    // ***���� �ӵ� ����, 0.2������� ���� (up�̸� ����, down�̸� ����)
    public void ControllMusicSpeedByBool(bool upOrDown)
    {
        if (upOrDown == true) audioSrc.pitch += 0.2f;
        else if (upOrDown == false) audioSrc.pitch -= 0.2f;

        if (audioSrc.pitch > 1f) audioSrc.pitch = 1f;
        else if (audioSrc.pitch < 0f) audioSrc.pitch = 0f;

        PlayerPrefs.SetFloat("Pitch", audioSrc.pitch); // �÷��̾� �������� ���� Pitch �� �����س���
    }

    // ***���� �ӵ� ����, ���ڷ� ����
    public void ControllMusicSpeedByFloat(float speed)
    {
        audioSrc.pitch = speed;

        PlayerPrefs.SetFloat("Pitch", audioSrc.pitch); // �÷��̾� �������� ���� Pitch �� �����س���
    }

    // ***SFX ��� (��ȣ�� ����ϴ� �Ŷ� �ٵ� ��� ���尡 ���� �������� ����ؾ� ��, double�� �ϴ� ������ ���̾�α� �ý��ۿ��� ȣ�� ����...)
    public void PlaySFXByInt(double d)
    {
        int i = (int)d;
        audioSrc.PlayOneShot(sfxClips[i]);
    }

    // ***SFC ��� (string���� ����ϴ� �Ŷ� ���� ���� �̸� ����ؾ� ��)
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
