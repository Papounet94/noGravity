﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    private Image background;
    public GameObject winImage, looseImage;
    public Text message;

    public GameObject highScores;
    private GameObject[] names;
    private GameObject[] times;

    void Start()
    {
        background = GameObject.Find("Background").GetComponent<Image>();

        // Retrieves the GameObjects containing the fields to display
        names = GameObject.FindGameObjectsWithTag("PlayerName");
        times = GameObject.FindGameObjectsWithTag("PlayerScore");

        if (PersistentSettings.Instance.playerLoose)
        {
            // Player has lost
            background.color = new Color(1f, 0f, 0f); // Pure red
            winImage.SetActive(false);
            looseImage.SetActive(true);
            message.text = PersistentSettings.Instance.endMessage;
            message.color = new Color(1, 1f, 1f);
        }
        else
        {
            // Player has won
            background.color = new Color(0.2f, 0.9f, 1.0f); // Cyanish color
            winImage.SetActive(true);
            looseImage.SetActive(false);
            message.text = string.Format("You have reached the target in {0:N1} s! You are safe now.",
                PersistentSettings.Instance.winTime);
            message.color = new Color(0f, 0f, 0f);
        }
        DisplayHighScores();
    }

    void DisplayHighScores()
    {
        // Put the high scores in the UI
        for (int index = 0; index < HiScore.maxScores; index++)
        {
            names[index].GetComponent<Text>().text = PersistentSettings.Instance.hiScores.GetName(index);
            times[index].GetComponent<Text>().text = PersistentSettings.Instance.hiScores.GetScore(index);
        }
    }
}
