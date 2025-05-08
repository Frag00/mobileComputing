using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewOrContinueMenu : MonoBehaviour
{
    public static bool GameIsInSavingsMenu=true;
    public GameObject SavingsMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (!GameIsInSavingsMenu)
        {
            SavingsMenuUI.SetActive(false);
        }
    }

    

    public void Resume()
    {
        SavingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsInSavingsMenu = false;
    }

    public void Continue()
    {
        GameManager.instance.LoadAsync();
        GameIsInSavingsMenu = false;
        SavingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

}
