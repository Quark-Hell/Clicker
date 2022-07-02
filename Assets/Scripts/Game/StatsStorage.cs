using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

public class StatsStorage : MonoBehaviour
{
    public Text[] ScoreText = new Text[9];
    public Text BetterGameText;

    private int[] Score = new int[9];
    private int BetterGame;

    public void SaveGame(int score)
    {
        //Displacement value
        for (byte i = (byte)(Score.Length - 1); i > 0; i--)
        {
            Score[i] = Score[i - 1];
        }
        //Set latest score
        Score[0] = score;

        //Set better score
        if (score > BetterGame)
        {
            BetterGame = score;
        }

        //Save in file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/SaveData.dat");
        SaveData data = new SaveData();
        data.Score = Score;
        data.BetterGame = BetterGame;
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            Score = data.Score;
            BetterGame = data.BetterGame;

            for (byte i = 0; i < Score.Length; i++)
            {
                ScoreText[i].text = i + 1 + "." + Score[i];
            }
            BetterGameText.text = "Better Game: " + BetterGame;
        }
    }

    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/SaveData.dat");
            Score = new int[9];
            BetterGame = 0;
        }
    }
}

[Serializable]
class SaveData
{
    public int[] Score = new int[9];
    public int BetterGame;
}
