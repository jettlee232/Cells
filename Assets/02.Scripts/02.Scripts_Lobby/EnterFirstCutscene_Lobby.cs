using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterFirstCutscene_Lobby : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("Lobby", 0);
    }
}
