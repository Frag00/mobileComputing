
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagement : MonoBehaviour
{
   public void LoadLevel()
    {
        SceneManager.LoadScene("Level2");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
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
}
