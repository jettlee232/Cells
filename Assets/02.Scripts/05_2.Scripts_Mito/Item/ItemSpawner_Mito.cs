using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner_Mito : MonoBehaviour
{
    public GameObject item;

    public float spawnInterval = 5.0f;
    public int maxItemCnt;
    public int curItemCnt;

    void Start()
    {
        StartCoroutine(SpawnItemsPeriodically());
    }

    IEnumerator SpawnItemsPeriodically()
    {
        while (true)
        {
            SpawnItemInArea(transform);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 플레이어가 일정 범위 안에 있을때만 생성?

    // 플레이어가 일정 범위 안에 있을때만 아이템을 액티브?
    // LOD로 처리?

    void SpawnItemInArea(Transform area)
    {
        // 범위 내에서 랜덤 위치를 생성
        Vector3 randomPosition = new Vector3(
            Random.Range(area.position.x - area.localScale.x / 2, area.position.x + area.localScale.x / 2),
            Random.Range(area.position.y - area.localScale.y / 2, area.position.y + area.localScale.y / 2),
            Random.Range(area.position.z - area.localScale.z / 2, area.position.z + area.localScale.z / 2)
        );

        // 아이템 생성
        Instantiate(item, randomPosition, Quaternion.identity);

        // 오브젝트 풀링 테스트
        //var itemGo = ObjectPoolManager_Mito.instance.GetGo("Adenine");
        //itemGo.transform.position = randomPosition;
    }
}
