using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public GameObject PauseUI;
    public GameObject GuideUI;
    public GameObject GameOverUI;
    public GameObject TextUI;
    public GameObject GameController;
    public GameController GC;

    public static bool paused = false;
    public static bool guideUp = true;
    public bool gameOver = false;
    bool addEntry = false;
    public float gameOverTime = 2f;
    public TextMeshProUGUI finalPoints;

    private void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController");
        GC = GameController.GetComponent<GameController>();
        if(GC.level>1)
        {
            Debug.Log("Test");
            GuideUI.SetActive(false);
            guideUp = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
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

        if(Input.GetKeyDown(KeyCode.H) && !gameOver)
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
        
        if(gameOver)
        {
            if(GC.level %3==0)
            {
                GameObject BossUI = GameObject.FindGameObjectWithTag("BossUI");
                BossUI.SetActive(false);
            }
            GameOverUI.SetActive(true);
            finalPoints.text = "Final Points: " + GC.GetPoints().ToString();
            GuideUI.SetActive(false);
            TextUI.SetActive(false);
            gameOverTime -= Time.deltaTime;
            //Add an entry
            if(!addEntry)
            {
                //Add the highscore entry
                addEntry = true;
            }
            if(gameOverTime<=0)
            {
                Time.timeScale = 0f;
                gameOver = false;
            }
        }
    }

    public void Restart()
    {
        gameOverTime = 2f;
        GC.level = 1;
        Time.timeScale = 1f;
        GC.ResetPoints();
        GC.ChangeMultiplier(0);
        SceneManager.LoadScene(1);
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
        GC.level = 1;
        Time.timeScale = 1f;
        GC.ResetPoints();
        GC.ChangeMultiplier(0);
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
