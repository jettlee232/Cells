using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager_Lobby : MonoBehaviour
{
    public GameObject[] interactables;
    public GameObject[] portals;
    public GameObject[] lights;

    #region 하이라이트
    public void GlowStart()
    {
        foreach (GameObject inter in interactables)
        {
            inter.GetComponent<Interactables_Lobby>().GlowStart();
        }
    }

    public void GlowEnd()
    {
        foreach (GameObject inter in interactables)
        {
            inter.GetComponent<Interactables_Lobby>().GlowEnd();
        }
    }
    #endregion

    #region 3DText
    /*
    public void HideText(GameObject go)
    {
        foreach (GameObject portal in portals)
        {
            if (portal == go) { portal.transform.GetChild(0).GetComponent<TextObject_Lobby>().HideText(); }
        }
    }

    public void HideTextExclude(GameObject go)
    {
        foreach (GameObject portal in portals)
        {
            if (portal != go) { portal.transform.GetChild(0).GetComponent<TextObject_Lobby>().HideText(); }
            else { portal.transform.GetChild(0).GetComponent<TextObject_Lobby>().ShowText(); }
        }
    }

    public void HideTextAll()
    {
        foreach (GameObject portal in portals) { portal.transform.GetChild(0).GetComponent<TextObject_Lobby>().HideText(); }
    }
    */
    #endregion

    #region 활성화된 포탈

    public void SetLight()
    {
        foreach (GameObject light in lights) { light.SetActive(true); }
    }

    #endregion
}
