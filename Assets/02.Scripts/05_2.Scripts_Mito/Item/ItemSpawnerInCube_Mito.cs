using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnerInCube_Mito : MonoBehaviour
{
    public GameObject itemPrefab; // ������ �������� ������
    public Transform itemParent;
    public int itemCount = 30; // ������ �������� ����
    public GameObject[] spawnAreas; // ���� ���� ť���

    void Start()
    {
        SpawnItems();
    }

    // �������� ������ ��ġ�� �����ϴ� �޼���
    private void SpawnItems()
    {
        for (int i = 0; i < itemCount; i++)
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
}
