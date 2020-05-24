using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/*****************************************************************************/
/* Implementation of Persistent Data across multiple scenes using the        */
/* Singleton Pattern                                                         */
/*****************************************************************************/

public class PersistentSettings : MonoBehaviour
{
    // Singleton's instance
    public static PersistentSettings Instance { get; private set; }

    // Persistent Data accessible using PersistentSettings.Instance

    public KeyCode forwardKey, backwardKey, leftKey, rightKey;
    public bool playerLoose;
    public string endMessage;
    public AudioSource music;
    public float winTime;
    public string playerName;
    public HiScore hiScores = new HiScore();

    const string filename = "/savefile.save";


    void Awake()
    {
        // Creation on the Singleton's instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            print(Application.persistentDataPath);
            LoadGame();
        }
        else
        // Ensures its uniqueness
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        // Data saving mechanism

        SaveFile sf = new SaveFile();
        sf.hiScores = hiScores;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + filename);
        bf.Serialize(file, sf);
        file.Close();
    }

    void LoadGame()
    {
        // Data retrieving mechanism

        if(!File.Exists(Application.persistentDataPath + filename))
        {
            Debug.Log("Savefile does not exist");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + filename, FileMode.Open);
        SaveFile sf = (SaveFile)bf.Deserialize(file);
        file.Close();
        hiScores = sf.hiScores;
    }
}

/*****************************************************************************/
/* Data Container for the saved values                                       */
/*****************************************************************************/

[System.Serializable]
public class SaveFile
{
    // Only high scores are saved currently
    public HiScore hiScores = new HiScore();
}

/*****************************************************************************/
/* Class used to contain 10 high scores                                      */
/* Name and time are initialised to "" and 999                               */
/* GetScore(index) retrieves the value of the time as a formatted string,    */
/*     Default: ---                                                          */
/* GetName(index) retrieves the name at index position                       */
/*     Default: ---                                                          */
/* InsertPlayer() inserts the player score and name at the right position    */
/*****************************************************************************/

[System.Serializable]
public class HiScore
{
    public static int maxScores = 10;
    public string[] name = new string[maxScores];
    public float[] time = new float[maxScores];


    public  HiScore()
    {
        for (int i = 0; i < 10; i++)
        {
            name[i] = "";
            time[i] = 999f;
        }
    }

    public string GetScore(int index)
    {
        if (index < maxScores && index >= 0)
        {
            // time is formatted with 1 trailing digit
            return (time[index] == 999f) ? "---" : String.Format("{0:N1}",
                PersistentSettings.Instance.hiScores.time[index]);
        }
        else
        {
            // Default value if out of boundaries
            return "---";
        }
    }

    public string GetName(int index)
    {
        if (index < maxScores && index >= 0)
        {
            return (name[index] == "") ? "---" : name[index];
        }
        else
        {
            // Default value if out of boundaries
            return "---";
        }
    }

    public void InsertPlayer()
    {
        // Compare winTime with previous high scores
        int index = 0;
        while(PersistentSettings.Instance.winTime > time[index])
        {
            index++;
        }
        // Right position found
        // Shift down following high scores (last one is lost)
        for (int i = maxScores - 1; i > index; i--)
        {
            PersistentSettings.Instance.hiScores.time[i] =
                PersistentSettings.Instance.hiScores.time[i - 1];
            PersistentSettings.Instance.hiScores.name[i] =
                PersistentSettings.Instance.hiScores.name[i - 1];
        }
        // Insert the new name and score in position
        PersistentSettings.Instance.hiScores.time[index] =
            PersistentSettings.Instance.winTime;
        PersistentSettings.Instance.hiScores.name[index] =
            PersistentSettings.Instance.playerName;

    }

}