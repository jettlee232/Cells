using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButtonMove_Lys_Game : MonoBehaviour
{
    public Vector3 targetPos;
    private float duration = 1f;
    public string scene;

    void Start()
    {
        StartCoroutine(MoveToPosition(transform.position, targetPos, duration));
    }

    IEnumerator MoveToPosition(Vector3 start, Vector3 end, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            GameManager_Lys_Game.instance.MoveScene(scene);
        }
    }
}
