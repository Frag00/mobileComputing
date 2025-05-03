
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

    
}
