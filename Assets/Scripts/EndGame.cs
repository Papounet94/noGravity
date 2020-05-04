using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    private Image background;
    public GameObject winImage, looseImage;
    public Text message;

    void Start()
    {
        background = GameObject.Find("Background").GetComponent<Image>();

        if (PersistentSettings.Instance.playerLoose)
        {
            background.color = new Color(1f, 0f, 0f); // Pure red
            winImage.SetActive(false);
            looseImage.SetActive(true);
            message.text = PersistentSettings.Instance.endMessage;
            message.color = new Color(1, 1f, 1f);
        }
        else
        {
            background.color = new Color(0.2f, 0.9f, 1.0f); // Cyanish color
            winImage.SetActive(true);
            looseImage.SetActive(false);
            message.text = "You have reached the target! You are safe now.";
            message.color = new Color(0f, 0f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
