using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_Home : MonoBehaviour
{
    public void OnClickStartBtn() { SceneManager.LoadScene("02_Lobby"); }
    //public void OnClickOptionBtn() { SceneManager.LoadScene(sceneName); }
    //public void OnClickQuitBtn() { SceneManager.LoadScene(sceneName); }
}
