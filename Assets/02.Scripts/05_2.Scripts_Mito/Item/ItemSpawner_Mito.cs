using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner_Mito : MonoBehaviour
{
    public InsideMeshChecker_Mito insideMeshChecker;
    public GameObject itemPrefab; // 생성할 아이템의 프리팹
    public Transform itemParent;
    public int itemCount = 30; // 생성할 아이템의 개수
    //public GameObject[] spawnAreas; // 스폰 영역 큐브들

    void Start()
    {
        SpawnItems();
    }

    void SpawnItems()
    {
        int spawnedItems = 0;

        while (spawnedItems < itemCount)
        {
            Vector3 randomPosition = GetRandomPositionWithinColliders();

            if (insideMeshChecker.IsPointInside(randomPosition))
            {
                Instantiate(itemPrefab, randomPosition, Quaternion.identity);
                spawnedItems++;
            }
        }
    }

    Vector3 GetRandomPositionWithinColliders()
    {
        MeshCollider[] colliders = insideMeshChecker.meshColliders;
        MeshCollider randomCollider = colliders[Random.Range(0, colliders.Length)];

        Bounds bounds = randomCollider.bounds;
        Vector3 randomPosition;

        randomPosition.x = Random.Range(bounds.min.x, bounds.max.x);
        randomPosition.y = Random.Range(bounds.min.y, bounds.max.y);
        randomPosition.z = Random.Range(bounds.min.z, bounds.max.z);

        return randomPosition;
    }

    /* Test
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

    */
}
