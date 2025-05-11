
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagement : MonoBehaviour
{
   public void LoadLevel()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadLastLevel()
    {
        SceneManager.LoadScene(3);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 0f;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Continue()
    {
        GameManager.instance.LoadAsync();
    }
}
