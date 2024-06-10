using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMenu_Lobby : MonoBehaviour
{
    public string nextSceneName;
    public string alertName;
    public string alertDescription;
    private Vector3 moveDir = Vector3.zero;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public string getName() { return alertName; }
    public string getDescription() { return alertDescription; }
    public string getSceneName() { return nextSceneName; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            moveDir = rb.velocity.normalized;
            //other.GetComponent<PlayerMoving_Lobby>().enabled = false;
            UIManager_Lobby.instance.SetAlert(this.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) { UIManager_Lobby.instance.HideAlert(); }
    }
}
