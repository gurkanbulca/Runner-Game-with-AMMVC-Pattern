using System.IO;
using UnityEngine;

public class PlayerDataController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    private const string FileName = "PlayerData.txt";

    private void Awake()
    {
        LoadDataFromResource();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SaveDataToResource(playerData);
    }

#if UNITY_EDITOR
    private void OnApplicationQuit()
        {
            SaveDataToResource(playerData);
        }
#endif

    /// <summary>
    /// saves player data to json.
    /// </summary>
    /// <param name="playerData"></param>
    public static void SaveDataToResource(PlayerData playerData)
    {
        var json = JsonUtility.ToJson(playerData);
        var dataPath = UnityEngine.Application.persistentDataPath;

        File.WriteAllText(dataPath + FileName, json);
    }

    /// <summary>
    /// load from json and apply to player data scriptable object.
    /// </summary>
    private void LoadDataFromResource()
    {
        var dataPath = UnityEngine.Application.persistentDataPath;

        try
        {
            var json = File.ReadAllText(dataPath + FileName);
            JsonUtility.FromJsonOverwrite(json, playerData);
        }
        catch (FileNotFoundException)
        {
            Debug.Log("File not found: default data will be initialized.");
            LoadDefaultData();
        }
    }

    /// <summary>
    /// reset player data to default values
    /// </summary>
    private void LoadDefaultData()
    {
       playerData.ResetData();
    }
}