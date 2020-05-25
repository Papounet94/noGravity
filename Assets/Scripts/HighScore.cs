using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    public InputField playerName;

    private void Start()
    {
        // Ensures that the caret is inside the name field
        playerName.ActivateInputField();
    }

    public void GetPlayerName()
    {
        // Callback for the Done button
        // Retrieves the name entered
        PersistentSettings.Instance.playerName = playerName.text;
        // Insert playerName and score at the right place into the hiScore array
        PersistentSettings.Instance.hiScores.InsertPlayer();
        // Saves the High scores array
        PersistentSettings.Instance.SaveGame();
        // Launches End Scene
        SceneManager.LoadScene("End");
    }
}
