using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSettings : MonoBehaviour
{
    public static PersistentSettings Instance { get; private set; }

    public KeyCode forwardKey, backwardKey, leftKey, rightKey;
    public bool playerLoose;
    public string endMessage;
    public AudioSource music;
        
void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}