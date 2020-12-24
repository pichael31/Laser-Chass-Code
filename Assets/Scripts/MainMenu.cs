using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void PlayHuman()
    {
        PlayerPrefs.SetString("Enemy", "Player");
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game");
    }

    public void PlayAI()
    {
        PlayerPrefs.SetString("Enemy", "AI");
        SceneManager.LoadScene("Game");
    }

    public void MakeLikeATree()
    {
        Application.Quit();
    }


    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Instructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void Options()
    {
        SceneManager.LoadScene("Options");
    }
}
