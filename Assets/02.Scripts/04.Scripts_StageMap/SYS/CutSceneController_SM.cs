using DG.Tweening.Core.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutSceneController_SM : MonoBehaviour
{    
    public GameObject entireGame;
    public GameObject cutScene;
    public Image blackPanel;

    [Header("Player & NPC")]
    public Transform playerPos;
    public SelectDialogue_StageMap npcDialogueSystem;
    public Transform[] spawnPos;
    public LaserPointer_StageMap laserPointer;

    public void EndCutSceneLoadGame()
    {
        cutScene.SetActive(false);
        entireGame.SetActive(true);
        
        StartCoroutine(ChangeBlackPanel());
    }

    IEnumerator ChangeBlackPanel()
    {
        float alpha = 1.0f;
        while (true)
        {
            blackPanel.color = new Color(blackPanel.color.r, blackPanel.color.g, blackPanel.color.b, alpha);
            yield return null;
            alpha -= 0.01f;
            if (alpha <= 0f)
            {                
                break;
            }                
        }

        Destroy(cutScene);

        // New
        blackPanel.gameObject.SetActive(false);
    }

    public void LoadFromCM()
    {
        cutScene.SetActive(true);
        entireGame.SetActive(false);        
    }

    public void LoadFromOtherScene(int spawnPosNum)
    {
        npcDialogueSystem.enabled = false;
        laserPointer.enabled = true;

        entireGame.SetActive(true);
        cutScene.SetActive(false);

        playerPos.transform.position = spawnPos[spawnPosNum].position;
        playerPos.transform.rotation = spawnPos[spawnPosNum].localRotation;

        StartCoroutine(ChangeBlackPanel());
    }

    // New
    public void LoadScene(string cutSceneName)
    {
        StartCoroutine(SceneLoading(cutSceneName));
        AudioMgr_CM.Instance.AudioFade();
    }

    IEnumerator SceneLoading(string cutSceneName)
    {
        blackPanel.gameObject.SetActive(true);

        float alpha = 0f;
        while (true)
        {
            blackPanel.color = new Color(blackPanel.color.r, blackPanel.color.g, blackPanel.color.b, alpha);
            yield return null;
            alpha += 0.01f;
            if (alpha >= 1f)
            {
                break;
            }
        }

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(cutSceneName);
    }
}
