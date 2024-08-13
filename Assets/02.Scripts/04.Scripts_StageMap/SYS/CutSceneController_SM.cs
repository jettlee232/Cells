using DG.Tweening.Core.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutSceneController_SM : MonoBehaviour
{
    public CutSceneSkip_SM cutSceneSkipScript;

    public void EndCutSceneLoadGame()
    {
        cutSceneSkipScript.enabled = false;
        SceneManager.LoadScene("04_StageMap");
    }
}