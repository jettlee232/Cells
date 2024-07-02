using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner_Mito : MonoBehaviour
{
    public InsideMeshChecker_Mito insideMeshChecker;
    public GameObject itemPrefab; // ������ �������� ������
    public Transform itemParent;
    public int itemCount = 30; // ������ �������� ����
    //public GameObject[] spawnAreas; // ���� ���� ť���

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

    /* Cube - GPT
    // �������� ������ ��ġ�� �����ϴ� �޼���
    private void SpawnItems()
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // ���� ���� ť��� ���� ������ ��ġ�� ��ȯ�ϴ� �޼���
    private Vector3 GetRandomSpawnPosition()
    {
        if (spawnAreas.Length == 0)
        {
            Debug.LogWarning("No spawn areas assigned.");
            return Vector3.zero;
        }

        // �������� �ϳ��� ť�긦 ����
        GameObject selectedCube = spawnAreas[Random.Range(0, spawnAreas.Length)];

        // ť���� ��ġ�� ũ�� ��������
        Vector3 center = selectedCube.transform.position;
        Vector3 size = selectedCube.transform.localScale;

        // ť���� ���� ������ ���� ��ġ ���
        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float y = Random.Range(center.y - size.y / 2, center.y + size.y / 2);
        float z = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        return new Vector3(x, y, z);
    }
    */

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

    // �÷��̾ ���� ���� �ȿ� �������� ����?

    // �÷��̾ ���� ���� �ȿ� �������� �������� ��Ƽ��?
    // LOD�� ó��?

    void SpawnItemInArea(Transform area)
    {
        // ���� ������ ���� ��ġ�� ����
        Vector3 randomPosition = new Vector3(
            Random.Range(area.position.x - area.localScale.x / 2, area.position.x + area.localScale.x / 2),
            Random.Range(area.position.y - area.localScale.y / 2, area.position.y + area.localScale.y / 2),
            Random.Range(area.position.z - area.localScale.z / 2, area.position.z + area.localScale.z / 2)
        );

        // ������ ����
        Instantiate(item, randomPosition, Quaternion.identity);

        // ������Ʈ Ǯ�� �׽�Ʈ
        //var itemGo = ObjectPoolManager_Mito.instance.GetGo("Adenine");
        //itemGo.transform.position = randomPosition;
    }

    */
}
