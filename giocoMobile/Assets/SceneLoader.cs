using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] public SceneDataSO[] _SceneDataSOArray;
    private Dictionary<string, int> _sceneIdToIndexMap = new Dictionary<string, int>();

   private void Start()
    {
        GameManager.instance.SceneLoader = this;
        PopulateSceneMappings();
    }

    private void PopulateSceneMappings()
    {
        foreach (var SceneDataSO in _SceneDataSOArray)
        {
            _sceneIdToIndexMap[SceneDataSO.UniqueName] = SceneDataSO.SceneId;
        }
    }

    public void LoadSceneByIndex(string SavedSceneId)  
    {
       if( _sceneIdToIndexMap.TryGetValue(SavedSceneId, out int sceneIndex))
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError($"No Scene Found for Index : {SavedSceneId}");
        }
    }

    public async Task LoadSceneByIndexAsync(string SavedSceneId)
    {
        if (_sceneIdToIndexMap.TryGetValue(SavedSceneId, out int sceneIndex))
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                    break;
                }
                await Task.Yield();
            }
        }
        else { Debug.LogError($"No Scene Found for Index : {SavedSceneId}"); }
    }
}
