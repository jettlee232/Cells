using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager_StageMap : MonoBehaviour
{
    public GameObject clearEffect;
    private GameObject[] Goals;
    private bool[] goalAchieve;
    private bool isClear = false;

    private void Start()
    {
        Goals = new GameObject[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Goals[i] = this.transform.transform.GetChild(i).gameObject;
            Goals[i].GetComponent<TutorialController_StageMap>().GetNum(i);
        }
        goalAchieve = new bool[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++) { goalAchieve[i] = false; }
        ShowGoals(false);
    }

    public void StartTutorial() { StartCoroutine(cTutorial()); }

    IEnumerator cTutorial()
    {
        ShowGoals(true);
        while (true)
        {
            isClear = true;
            foreach (bool achieve in goalAchieve) { if (!achieve) { isClear = false; break; } }
            if (isClear) { break; }
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
