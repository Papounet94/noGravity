using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    public InputField playerName;

    private void Start()
    {
        playerName.ActivateInputField();
    }

    public void GetPlayerName()
    {
        PersistentSettings.Instance.playerName = playerName.text;
        // Insert playerName and score at the right place into the hiScore array
        PersistentSettings.Instance.hiScores.InsertPlayer();
        PersistentSettings.Instance.SaveGame();
        SceneManager.LoadScene("End");
    }
}
