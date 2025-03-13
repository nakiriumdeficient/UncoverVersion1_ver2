using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine.UI;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/save.dat";

    public static void SaveGame(Vector3 position, int level, int experience)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);

        SaveData data = new SaveData(position, level, experience);
        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Game Saved!");
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("No save file found!");
            return null;
        }
    }
}