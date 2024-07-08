using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager_Lobby : MonoBehaviour
{
    public GameObject[] interactables;
    public GameObject[] portals;
    public GameObject[] lights;

    #region ���̶���Ʈ
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

    #region Ȱ��ȭ�� ��Ż

    public void SetLight()
    {
        foreach (GameObject light in lights) { light.SetActive(true); }
    }

    #endregion
}
