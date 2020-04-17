using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseUI;
    public GameObject GuideUI;
    public GameObject GameController;
    public GameController GC;
    public static bool paused = false;
    public static bool guideUp = true;

    private void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController");
        GC = GameController.GetComponent<GameController>();
        if(GC.level>1)
        {
            GuideUI.SetActive(false);
            guideUp = false;
        }
    }

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

        if(Input.GetKeyDown(KeyCode.H))
        {
            if(guideUp)
            {
                GuideUI.SetActive(false);
                guideUp = false;
            }
            else
            {
                GuideUI.SetActive(true);
                guideUp = true;
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
