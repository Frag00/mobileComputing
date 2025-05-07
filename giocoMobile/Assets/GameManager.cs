
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public bool isGameActive = true;
    public static GameManager instance;
    public PlayerScript Player;
    public SceneData SceneData;
    public SceneLoader SceneLoader;


    private bool _isSaving;
    private bool _isLoading;
    private void Start()
    {
        if (Player == null)
        {
            FindObjectOfType<PlayerScript>();
        }
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if(Player == null)
            {
              Player =  FindObjectOfType<PlayerScript>();
            }
            if (SceneData == null)
            {
                SceneData = FindObjectOfType<SceneData>();
            }
            if (SceneLoader == null)
            {
                SceneLoader = FindObjectOfType<SceneLoader>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !_isSaving)
        {
            SaveAsync();
        }
        if (Input.GetKeyDown(KeyCode.P) && !_isLoading)
        {
            LoadAsync();
        }
    }


    private async void SaveAsync()
    {
        _isSaving = true;
        await SaveSystem.SaveAsynchronously();
        _isSaving = false;
    }
    private async void LoadAsync()
    {
        _isLoading = true;
        await SaveSystem.LoadAsync();
        _isLoading = false;
    }

    private void OnEnable()
{
    SceneManager.sceneLoaded += OnSceneLoaded;
}

private void OnDisable()
{
    SceneManager.sceneLoaded -= OnSceneLoaded;
}

private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    Player = FindObjectOfType<PlayerScript>();
}
}
