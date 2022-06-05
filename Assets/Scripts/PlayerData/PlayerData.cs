using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "PlayerData", fileName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public int currentLevelNumber = 1;
    public int currencyAmount;
    public int upgradeCost = 10;
    public int startingStackAmount;
    public int stackLimit = 18;
    public int levelCap = 3;

    /// <summary>
    /// reset saved data.
    /// </summary>
    public void ResetData()
    {
        currentLevelNumber = 1;
        currencyAmount = 0;
        upgradeCost = 10;
        startingStackAmount = 0;
        stackLimit = 18;
        levelCap = 3;
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(PlayerData))]
public class PlayerDataEditor : Editor
{
    /// <summary>
    /// Save: saves current values to the json.
    /// Load Default: reset data then save values to the json.
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Save"))
        {
            var playerData = target as PlayerData;
            PlayerDataController.SaveDataToResource(playerData);
        }
        if (GUILayout.Button("Load Default"))
        {
            var playerData = target as PlayerData;
            playerData.ResetData();
            PlayerDataController.SaveDataToResource(playerData);
        }
    }
}
#endif