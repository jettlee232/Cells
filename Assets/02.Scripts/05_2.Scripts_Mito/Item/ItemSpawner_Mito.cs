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
}
