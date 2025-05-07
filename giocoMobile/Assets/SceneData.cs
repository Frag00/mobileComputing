using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneData : MonoBehaviour
{

    public SceneDataSO Data;
    // Start is called before the first frame update
   private void Awake()
    {
        GameManager.instance.SceneData = this;
    }

    #region Save and Load

    public void Save(ref SceneSaveData data)
    {
        data.SceneId = Data.UniqueName;
    }

    public void Load(SceneSaveData data)
    {
        GameManager.instance.SceneLoader.LoadSceneByIndex(data.SceneId);
    }

    public async Task LoadAsync(SceneSaveData asyncData)
    {
        await GameManager.instance.SceneLoader.LoadSceneByIndexAsync(asyncData.SceneId);
    }

    public Task WaitForSceneToBeFullyLoaded()
    {
        TaskCompletionSource<bool> taskCompletion = new TaskCompletionSource<bool>();

        UnityEngine.Events.UnityAction<Scene, LoadSceneMode> sceneLoaderHandler = null;

        sceneLoaderHandler = (scene, mode) =>
        {
            taskCompletion.SetResult(true);
            SceneManager.sceneLoaded -= sceneLoaderHandler;
        };

        SceneManager.sceneLoaded += sceneLoaderHandler;
        return taskCompletion.Task;
    }



    #endregion
}

[System.Serializable]
public struct SceneSaveData
{
    public string SceneId;
}
