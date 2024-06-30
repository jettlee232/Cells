using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TextObject_Lobby : MonoBehaviour
{
    private GameObject player;
    private bool isShow = false;

    void Start()
    {
        player = GameManager_Lobby.instance.GetPlayer();
        this.gameObject.SetActive(false);
    }

    public void ShowText()
    {
        isShow = true;
        StartCoroutine(FollowPlayer());
    }
    public void HideText()
    {
        isShow = false;
    }

    IEnumerator FollowPlayer()
    {
        while (true)
        {
            if (!isShow) { break; }
            Vector3 direction = player.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.down);
            yield return null;
        }
    }
}
