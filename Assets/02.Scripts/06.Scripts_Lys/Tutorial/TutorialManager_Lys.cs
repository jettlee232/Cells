using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager_Lys : MonoBehaviour
{
    [Header("기본 세팅")]
    private GameObject NPC;
    private GameObject player;
    public Transform playerPos;
    public GameObject UIPointer;
    public GameObject gun;
    public GameObject rocket;

    [Header("설명용 적")]
    public GameObject tempDP;
    public GameObject tempCD;
    public GameObject tempES;
    public Vector3[] movePos;        // 0이 중앙, 1이 왼쪽, 2가 오른쪽이도록 지정
    public GameObject tempShowEffect;
    public float moveTime = 1f;

    [Header("양학용 적")]
    public GameObject[] enemies;
    public int tuto1Goal = 5;
    public int tuto2Goal = 5;
    public float respawnTimer = 1f;
    public GameObject respawnEffect;

    private int killDP = 0;
    private int killCD = 0;
    private int killES = 0;
    private int killAll = 0;
    private List<GameObject> deadEnemies;

    void Start()
    {
        InitTemp();
        InitEnemyCount();
        NPC = GameManager_Lys.instance.GetNPC();
        player = GameManager_Lys.instance.GetPlayer();
        deadEnemies = new List<GameObject>();
        HideAllEnemies();
        gun.SetActive(false);
        rocket.SetActive(false);
    }

    #region 플레이어 관련
    public void movePlayer()
    {
        player.GetComponent<PlayerMoving_Lys>().enabled = false;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.position = playerPos.position;
        player.transform.rotation = playerPos.rotation;
    }
    public void UseGun()
    {
        if (rocket.activeSelf) { rocket.SetActive(false); }
        gun.SetActive(true);
    }
    public void UseRocket()
    {
        if (gun.activeSelf) { gun.SetActive(false); }
        rocket.SetActive(true);
    }
    #endregion

    #region 임시로 하나씩 보여주는 거
    public void InitTemp()
    {
        tempDP.SetActive(false);
        tempCD.SetActive(false);
        tempES.SetActive(false);
    }
    public void ShowExample1()
    {
        TempShowEffect();
        tempDP.SetActive(true);
        tempDP.transform.position = movePos[0];
    }
    public void ShowExample2()
    {
        TempShowEffect();
        tempCD.SetActive(true);
        tempCD.transform.position = movePos[0];
        StartCoroutine(moveTemp(tempDP, 1));
    }
    public void ShowExample3()
    {
        TempShowEffect();
        tempES.SetActive(true);
        tempES.transform.position = movePos[0];
        StartCoroutine(moveTemp(tempCD, 2));
    }
    public void TempShowEffect() { Instantiate(tempShowEffect, movePos[0], Quaternion.identity); }
    IEnumerator moveTemp(GameObject go, int dest)
    {
        float elapsedTime = 0f;
        Vector3 startPos = go.transform.position;
        Vector3 destPos = movePos[dest];

        while (elapsedTime < moveTime)
        {
            go.transform.position = Vector3.Lerp(startPos, destPos, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        go.transform.position = destPos;
    }
    #endregion

    #region 샌드백
    public void InitEnemyCount()
    {
        killDP = 0;
        killCD = 0;
        killES = 0;
        killAll = 0;
    }
    
    public void plusDP() { killDP++; killAll++; }
    public void plusCD() { killCD++; killAll++; }
    public void plusEs() { killES++; killAll++; }

    public void ShowAllEnemies() { foreach (GameObject enemy in enemies) { enemy.SetActive(true); } }
    public void HideAllEnemies() { foreach (GameObject enemy in enemies) { enemy.SetActive(false); } }

    public void EnemyDies(GameObject enemy)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemy == enemies[i]) { StartCoroutine(cDieAndRespawn(enemies[i])); }
        }
    }
    IEnumerator cDieAndRespawn(GameObject go)
    {
        deadEnemies.Add(go);
        go.SetActive(false);
        yield return new WaitForSeconds(respawnTimer);
        deadEnemies.Remove(go);
        go.SetActive(true);
        Instantiate(respawnEffect, go.transform.position, Quaternion.identity);
    }


    public void GunTutorial_Lys() { StartCoroutine(cTutorial1()); }
    IEnumerator cTutorial1()
    {
        GameManager_Lys.instance.HideLine();
        UseGun();
        ShowAllEnemies();
        while (true)
        {
            if (killAll >= tuto1Goal)
            {
                NPC.GetComponent<SelectDialogue_Lys>().ActivateDST3();
                break;
            }
            else { yield return null; }
        }
        EndTutorial();
    }

    public void RocketTutorial_Lys() { StartCoroutine(cTutorial2()); }
    IEnumerator cTutorial2()
    {
        UseRocket();
        ShowAllEnemies();
        while (true)
        {
            if (killAll >= tuto2Goal)
            {
                NPC.GetComponent<SelectDialogue_Lys>().ActivateDST4();
                break;
            }
            else { yield return null; }
        }
        EndTutorial();
    }

    void EndTutorial()
    {
        StopAllCoroutines();
        HideAllEnemies();
        InitEnemyCount();
    }
    #endregion
}
