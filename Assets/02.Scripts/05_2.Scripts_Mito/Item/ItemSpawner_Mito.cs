using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner_Mito : MonoBehaviour
{
    public GameObject itemPrefab; // 생성할 아이템의 프리팹
    public Transform itemParent;
    public int numberOfItems = 30; // 생성할 아이템의 개수
    public GameObject[] spawnAreas; // 스폰 영역 큐브들

    void Start()
    {
        SpawnItems();
    }

    // 아이템을 무작위 위치에 생성하는 메서드
    private void SpawnItems()
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // 스폰 영역 큐브들 내의 무작위 위치를 반환하는 메서드
    private Vector3 GetRandomSpawnPosition()
    {
        if (spawnAreas.Length == 0)
        {
            Debug.LogWarning("No spawn areas assigned.");
            return Vector3.zero;
        }

        // 랜덤으로 하나의 큐브를 선택
        GameObject selectedCube = spawnAreas[Random.Range(0, spawnAreas.Length)];

        // 큐브의 위치와 크기 가져오기
        Vector3 center = selectedCube.transform.position;
        Vector3 size = selectedCube.transform.localScale;

        // 큐브의 영역 내에서 랜덤 위치 계산
        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float y = Random.Range(center.y - size.y / 2, center.y + size.y / 2);
        float z = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        return new Vector3(x, y, z);
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
