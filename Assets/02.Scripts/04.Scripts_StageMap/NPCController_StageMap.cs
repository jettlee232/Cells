using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController_StageMap : MonoBehaviour
{
    private GameObject NPCTalkPanel;

    [Header("Settings")]
    public string name;
    public string des;
    public Sprite img;
    public string sceneName;

    public void SetNPCTalk()
    {
        //GameManager_StageMap.instance.SetSelectable(false);
        UIManager_StageMap.instance.SetNPCPanel(des, name, img, sceneName);
        GameManager_StageMap.instance.DisableMove();
        GameManager_StageMap.instance.RemoveSelect();
    }
}
