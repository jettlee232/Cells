using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController_Lys : MonoBehaviour
{
    TutorialManager_Lys tutorialManager;
    string type;
    public GameObject dieEffect;

    private void Start()
    {
        tutorialManager = GameManager_Lys.instance.GetTutorialManager().GetComponent<TutorialManager_Lys>();
        type = gameObject.GetComponent<EnemyType_Lys>().GetType();
    }

    public void Die()
    {
        Instantiate(dieEffect, gameObject.transform.position, Quaternion.identity);
        switch (type)
        {
            case "DP":
                tutorialManager.plusDP();
                tutorialManager.EnemyDies(this.gameObject);
                break;
            case "CD":
                tutorialManager.plusCD();
                tutorialManager.EnemyDies(this.gameObject);
                break;
            case "ES":
                tutorialManager.plusEs();
                tutorialManager.EnemyDies(this.gameObject);
                break;
            default: break;
        }
    }
}
