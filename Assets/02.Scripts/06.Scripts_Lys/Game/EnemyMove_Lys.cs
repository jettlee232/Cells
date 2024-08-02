using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMove_Lys : MonoBehaviour
{
    private GameObject player;
    private GameObject playerHp;
    private float duration = 7f;
    private float amplitude = 1f; // 코사인 그래프의 진폭 (가로 진동의 크기)
    private float frequency = 1f; // 코사인 그래프의 주파수 (진동의 빈도)
    private Vector3 startPoint = Vector3.zero;
    private float EPDistance;

    void Start()
    {
        player = GameManager_Lys_Game.instance.GetPlayerCam();
        playerHp = GameManager_Lys_Game.instance.GetPlayer();
        if (player != null) { transform.LookAt(player.transform.position); }
        SetStartPos(transform.position);
        EPDistance = GameManager_Lys_Game.instance.GetEPDistance();
        duration = Random.Range(GameManager_Lys_Game.instance.GetMinEnemyComeTimer(), GameManager_Lys_Game.instance.GetMaxEnemyComeTimer());

        switch (Random.Range(1, 4))
        {
            case 1: // 그냥 오기
                StartCoroutine(Pattern1());
                break;
            case 2: // 좌우
                StartCoroutine(Pattern2());
                break;
            case 3: // 상하
                StartCoroutine(Pattern3());
                break;
            default:
                StartCoroutine(Pattern1());
                break;
        }
    }

    public void SetStartPos(Vector3 pos)
    {
        startPoint = pos;
    }

    public void Die()
    {
        StopAllCoroutines();
        // 죽는 모션
        Destroy(gameObject, 1f);
    }

    IEnumerator Pattern1()
    {
        while (startPoint == Vector3.zero) { yield return new WaitForSeconds(0.05f); }

        float elapsedTime = 0f;
        Vector3 direction = (player.transform.position - startPoint).normalized;
        float distance = Vector3.Distance(startPoint, player.transform.position);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float timer = elapsedTime / duration;

            float linearPosition = Mathf.Lerp(0, distance, timer);
            Vector3 newPosition = startPoint + direction * linearPosition;
            transform.position = newPosition;

            if (Vector3.Distance(newPosition, player.transform.position) <= EPDistance) { break; }

            yield return null;
        }

        // 만약 플레이어와 닿았다면?
        playerHp.GetComponent<PlayerHPController_Lys_Game>().Hitted();
        Destroy(gameObject, 0.1f);

        //transform.position = player.transform.position;
    }

    IEnumerator Pattern2()
    {
        amplitude = Random.Range(0.3f, 1f);
        frequency = Random.Range(4f, 10f);

        while (startPoint == Vector3.zero) { yield return new WaitForSeconds(0.05f); }

        float elapsedTime = 0f;
        Vector3 direction = (player.transform.position - startPoint).normalized;
        float distance = Vector3.Distance(startPoint, player.transform.position);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float timer = elapsedTime / duration;

            float linearPosition = Mathf.Lerp(0, distance, timer);
            float sineOffset = Mathf.Sin(timer * Mathf.PI * frequency) * amplitude;

            Vector3 newPosition = startPoint + direction * linearPosition;
            newPosition.x += sineOffset;
            transform.position = newPosition;

            if (Vector3.Distance(newPosition, player.transform.position) <= EPDistance) { break; }

            yield return null;
        }

        // 만약 플레이어와 닿았다면?
        playerHp.GetComponent<PlayerHPController_Lys_Game>().Hitted();
        Destroy(gameObject, 0.1f);

        //transform.position = player.transform.position;
    }

    IEnumerator Pattern3()
    {
        amplitude = Random.Range(0.3f, 1f);
        frequency = Random.Range(4f, 10f);

        while (startPoint == Vector3.zero) { yield return new WaitForSeconds(0.05f); }

        float elapsedTime = 0f;
        Vector3 direction = (player.transform.position - startPoint).normalized;
        float distance = Vector3.Distance(startPoint, player.transform.position);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float timer = elapsedTime / duration;

            float linearPosition = Mathf.Lerp(0, distance, timer);
            float sineOffset = Mathf.Sin(timer * Mathf.PI * frequency) * amplitude;

            Vector3 newPosition = startPoint + direction * linearPosition;
            newPosition.y += sineOffset;
            transform.position = newPosition;

            if (Vector3.Distance(newPosition, player.transform.position) <= EPDistance) { break; }

            yield return null;
        }

        // 만약 플레이어와 닿았다면?
        playerHp.GetComponent<PlayerHPController_Lys_Game>().Hitted();
        Destroy(gameObject, 0.1f);

        //transform.position = player.transform.position;
    }
}
