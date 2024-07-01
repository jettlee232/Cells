using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMenu_Lobby : MonoBehaviour
{
    public string nextSceneName;
    public string alertName;
    public string alertDescription;

    public string GetName() { return alertName; }
    public string GetDescription() { return alertDescription; }
    public string GetSceneName() { return nextSceneName; }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager_Lobby.instance.GetWarpable()) { return; }
        else
        {
            GameManager_Lobby.instance.GetUIPointer().GetComponent<LaserPointer_Lobby>().DestroyDescription();
            if (other.gameObject.CompareTag("Player")) { UIManager_Lobby.instance.SetAlert(this.gameObject); }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) { UIManager_Lobby.instance.HideAlert(); }
    }
}
