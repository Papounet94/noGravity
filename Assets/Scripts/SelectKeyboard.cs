using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectKeyboard : MonoBehaviour
{
    public Text forwardText;
    public Text leftText;
    public Button startButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton.enabled = false;
        PersistentSettings.Instance.music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWASD()
    {
        leftText.text = "A";
        forwardText.text = "W";
        PersistentSettings.Instance.forwardKey = KeyCode.W;
        PersistentSettings.Instance.backwardKey = KeyCode.S;
        PersistentSettings.Instance.leftKey = KeyCode.A;
        PersistentSettings.Instance.rightKey = KeyCode.D;

        startButton.enabled = true;
    }

    public void SetZQSD()
    {
        leftText.text = "Q";
        forwardText.text = "Z";
        PersistentSettings.Instance.forwardKey = KeyCode.Z;
        PersistentSettings.Instance.backwardKey = KeyCode.S;
        PersistentSettings.Instance.leftKey = KeyCode.Q;
        PersistentSettings.Instance.rightKey = KeyCode.D;

        startButton.enabled = true;
    }
}
