using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMovePanel_Home : MonoBehaviour
{
    public void Call_Lobby() { SceneManager.LoadScene("02_Lobby"); }
    public void Call_CM_Tuto() { SceneManager.LoadScene("03_0_CM_Cutscenes"); }
    public void Call_CM() { SceneManager.LoadScene("03_2_CM"); }
    public void Call_SM() { SceneManager.LoadScene("04_StageMap"); }
    public void Call_Mito_Tuto() { SceneManager.LoadScene("05_0_Mito_Cutscene"); }
    public void Call_Mito() { SceneManager.LoadScene("05_2_Mito"); }
    public void Call_Lys_Tuto() { SceneManager.LoadScene("06_Lys_Cutscene"); }
    public void Call_Lys() { SceneManager.LoadScene("06_Lys"); }
    public void PanelClose() { this.gameObject.SetActive(false); }

}
