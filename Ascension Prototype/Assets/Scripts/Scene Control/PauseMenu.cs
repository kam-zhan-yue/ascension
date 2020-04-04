using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseUI;
    public static bool paused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

    }

    //===============PROCEDURE===============//
    void Pause()
    //Purpose:          Stops time and shows pause window
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
        Debug.Log("Pausing the game...");
    }

    //===============PROCEDURE===============//
    public void Resume()
    //Purpose:          Resumes time and closes pause window if button is chosen
    {
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        Debug.Log("Resuming the game...");
    }

    //===============PROCEDURE===============//
    public void LoadMenu()
    //Purpose:          Loads the menu scene if button is chosen
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
        Debug.Log("Bringing to the menu...");
    }

    //===============PROCEDURE===============//
    public void Quit()
    //Purpose:          Quits the application if button is chosen
    {
        Debug.Log("Exiting the game...");
        Application.Quit();
    }
}
