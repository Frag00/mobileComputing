using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public class SaveSystem
{
    private static SaveData _savedata = new SaveData();

    [System.Serializable]
    public struct SaveData
    {
        public PlayerSaveData PlayerData;
        public SceneSaveData SceneData;
    }


    #region Save Async

    public static async Task SaveAsynchronously()
    {
        await SaveAsync();
    }

    private static async Task SaveAsync()
    {
        HandleSaveData();

        await File.WriteAllTextAsync(SaveFileName(), JsonUtility.ToJson(_savedata, true));
    } 

    #endregion



    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + "/save" + ".save";
        return saveFile;
    }

    public static void Save()
    {
        HandleSaveData();
        File.WriteAllText(SaveFileName(),JsonUtility.ToJson(_savedata,true));
    }

    public static void HandleSaveData()
    {
        GameManager.instance.Player.Save(ref _savedata.PlayerData);
        GameManager.instance.SceneData.Save(ref _savedata.SceneData);
    }



    public static async Task LoadAsync()
    {
        string saveContent = File.ReadAllText(SaveFileName());
        _savedata = JsonUtility.FromJson<SaveData>(saveContent);

        await HandleLoadDataAsync();
    }

    private static async Task HandleLoadDataAsync()
    {
        await GameManager.instance.SceneData.LoadAsync(_savedata.SceneData);

        await GameManager.instance.SceneData.WaitForSceneToBeFullyLoaded();

        GameManager.instance.Player.Load(_savedata.PlayerData);
    }



    public static void Load()
    {
        string saveContent = File.ReadAllText(SaveFileName());
        _savedata = JsonUtility.FromJson<SaveData>(saveContent);
        HandleLoadData();
    }

    public static void HandleLoadData()
    {
        
        GameManager.instance.SceneData.Load(_savedata.SceneData);
        GameManager.instance.Player.Load(_savedata.PlayerData);
    }
}
