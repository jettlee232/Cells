using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController_StageMap : MonoBehaviour
{
    private int num;
    private GameObject tutorialManager;
    private GameObject effect;

    public GameManager_StageMap gameMgr;

    private void Start()
    {
        tutorialManager = gameObject.transform.parent.gameObject;
        effect = tutorialManager.GetComponent<TutorialManager_StageMap>().GetEffect();
    }

    public void GetNum(int number) { num = number; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 direction = (other.transform.position - transform.position).normalized;
            Instantiate(effect, other.ClosestPointOnBounds(transform.position), Quaternion.LookRotation(direction));
            tutorialManager.GetComponent<TutorialManager_StageMap>().GetAchieved(num);

            // SYS Code
            tutorialManager.GetComponent<TutorialManager_StageMap>().ReturnGoalAchieve();

            this.gameObject.SetActive(false);
        }
    }
}
