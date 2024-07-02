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
    }

    public void StartTutorial() { StartCoroutine(cTutorial()); }

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

    public void ShowGoals(bool show)
    {
        foreach (GameObject goal in Goals) { goal.SetActive(show); }
    }

    public void GetAchieved(int num) { goalAchieve[num] = true; }
    public GameObject GetEffect() { return clearEffect; }
}
