using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneManager_MitoCutscene : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public string nextSceneName;

    void Start()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped += OnPlayableDirectorStopped;
        }
        else
        {
            Debug.LogError("PlayableDirector�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    void OnDestroy()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }
    }

    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
