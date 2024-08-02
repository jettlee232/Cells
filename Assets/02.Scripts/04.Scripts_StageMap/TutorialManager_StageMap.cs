using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager_StageMap : MonoBehaviour
{
    public GameObject clearEffect;
    private GameObject[] Goals;
    private bool[] goalAchieve;
    private int spareCount = 0;
    private int oldCount = 0;

    // SYS Code
    public Tooltip[] tooltips;
    public LaserPointer_StageMap laserPointer;

    // SYS Code
    [Header("NPCs")]
    public Animator[] npcAnims;
    public float loopInterval = 1.5f;
    public GameObject subtitleBug;

    private void Start()
    {
        Goals = new GameObject[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Goals[i] = this.transform.transform.GetChild(i).gameObject;
            Goals[i].GetComponent<TutorialController_StageMap>().GetNum(i);
        }
        goalAchieve = new bool[this.transform.childCount];
        spareCount = this.transform.childCount;
        oldCount = spareCount;
        for (int i = 0; i < this.transform.childCount; i++) { goalAchieve[i] = false; }
        ShowGoals(false);

        // SYS Code
        InitNPCAnimation();
    }

    // SYS Code
    public void StartTutorial() 
    {
        //StartCoroutine(cTutorial()); 
        ShowGoals(true);        
    }

    
    IEnumerator cTutorial()
    {
        ShowGoals(true);
        while (true)
        {
            spareCount = 0;
            foreach (bool achieve in goalAchieve) { if (!achieve) { spareCount++; } }            
            if (spareCount == 0) { UIManager_StageMap.instance.ChangeQuest("NPC에게 돌아가자!"); break; }
            if (oldCount != spareCount) { UIManager_StageMap.instance.ChangeQuest("비행을 하면서 " + spareCount.ToString() + "개의 타겟을 찾아보자!"); }
            oldCount = spareCount;
            yield return null;
        }
        GameManager_StageMap.instance.ClearTutorial();
        ShowGoals(false);
    }
    
    // SYS Code
    public void ShowGoals(bool show)
    {
        foreach (GameObject goal in Goals) { goal.SetActive(show); }

        // SYS Code
        GameObject go = GameObject.Find("Custom Dialogue UI");
        GameObject NPCSubtitle = go.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        GameObject SubtitleText = NPCSubtitle.transform.GetChild(0).gameObject;
        GameObject ContinueBtn = NPCSubtitle.transform.GetChild(1).gameObject;

        // SYS Code
        go.SetActive(true);
        NPCSubtitle.SetActive(true);
        SubtitleText.SetActive(true);
        ContinueBtn.SetActive(true);
    }

    public void GetAchieved(int num) { goalAchieve[num] = true; }
    public GameObject GetEffect() { return clearEffect; }

    // SYS Code
    public void NewTooltip(int index, string content) 
    {
        tooltips[index].gameObject.SetActive(true);
        tooltips[index].TooltipOn(content); 
    }
    public void TooltipOver(int index) 
    { 
        tooltips[index].TooltipOff();
    }

    // SYS Code
    public void LaserPointerONOFF(bool flag)
    {
        laserPointer.enabled = flag;
    }

    // SYS Code
    public void ShowingTooltipAnims(int hand, int index)
    {
        tooltips[hand].ShowingTooltipAnims(index);
    }
    public void UnShowingTooltipAnims(int hand)
    {
        tooltips[hand].UnShowingTooltipAnims();
    }

    // SYS Code
    void InitNPCAnimation()
    {
        npcAnims[0].Play("Treading");
        npcAnims[1].Play("Stabbing2");
        npcAnims[2].Play("Run Forward");
        npcAnims[3].Play("Falling");
    }

    // SYS Code
    public void ReturnGoalAchieve()
    {
        int cnt = 0;
        for (int i = 0; i < goalAchieve.Length; i++)
        {
            if (goalAchieve[i] == true) cnt++;
            tooltips[0].TooltipTextChange("남은 타겟 : " + (3 - cnt) + "개");
        }

        if (cnt == 3)
        {
            tooltips[0].ChangeSprite(0);
            tooltips[0].TooltipTextChange("NPC에게 돌아가자!");
            GameManager_StageMap.instance.ClearTutorial();
            ShowGoals(false);
        }                       
    }

    // SYS Code
    public void TooltipSpriteChange(int hand, int sprite)
    {
        tooltips[hand].ChangeSprite(sprite);
    }
}
