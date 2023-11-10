/*
 * Author: Alexander Kam
 * Date: 30-5-20
 * Licence: Unity Personal Editor Licence
 */
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    //Game Object References
    public GameObject PauseUI;
    public GameObject GuideUI;
    public GameObject BossUI;
    public GameObject PowerUI;
    public GameObject GameOverUI;
    public GameObject TextUI;
    public GameObject GameController;
    public GameController GC;
    public AudioMixer mixer;
    public Slider slider;

    public static bool paused = false;
    public static bool guideUp = true;
    bool bossUp = false;
    public bool gameOver = false;
    bool addEntry = false;
    public float gameOverTime = 2f;
    public TextMeshProUGUI finalPoints;
    public TextMeshProUGUI nameText;
    private bool actuallyGameOver;

    private void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController");
        BossUI = GameObject.FindGameObjectWithTag("BossUI");
        GC = GameController.GetComponent<GameController>();
        slider.value = PlayerPrefs.GetFloat("Volume");
        if(GC.level>1)
        {
            Debug.Log("Test");
            GuideUI.SetActive(false);
            guideUp = false;
        }
        addEntry = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !actuallyGameOver)
        {
            if (!paused)
            {
                Pause();
            }
            else
            {
                PauseUI.SetActive(false);
                Time.timeScale = 1f;
                paused = false;
                Debug.Log("Resuming the game...");
            }
        }

        if(Input.GetKeyDown(KeyCode.H) && !actuallyGameOver)
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
            if(BossUI != null)
                BossUI.SetActive(false);
            actuallyGameOver = true;
            GameOverUI.SetActive(true);
            finalPoints.text = "Final Points: " + GC.GetPoints().ToString();
            nameText.text = PlayerPrefs.GetString("Name");
            GuideUI.SetActive(false);
            TextUI.SetActive(false);
            PowerUI.SetActive(false);
            gameOverTime -= Time.deltaTime;
            //Add the highscore entry
            if(!addEntry)
            {
                Debug.Log("Add Entry");
                leaderBoard.AddLeaderBoard(GC.GetPoints(), PlayerPrefs.GetString("Name"));
                Debug.Log(PlayerPrefs.GetString("Name"));
                HighscoreTable.AddHighscoreEntry(GC.GetPoints(), PlayerPrefs.GetString("Name"));
                addEntry = true;
            }
            if(gameOverTime<=0)
            {
                Time.timeScale = 0f;
                gameOver = false;
            }
        }
    }

    //===============PROCEDURE===============//
    public void SetVolume()
    //Purpose:          Sets the volume of the slider and changes playerfers on slider value
    {
        float volume = slider.value;
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    //===============PROCEDURE===============//
    public void Restart()
    //Purpose:          Resets all important variables
    {
        FindObjectOfType<AudioManager>().Stop("GameOver");
        FindObjectOfType<AudioManager>().Stop("Boss");
        FindObjectOfType<AudioManager>().Play("RegularLevel");
        FindObjectOfType<AudioManager>().Play("ButtonPress");
        gameOverTime = 2f;
        GC.level = 1;
        Time.timeScale = 1f;
        GC.ResetPoints();
        GC.ChangeMultiplier(0);
        GC.ResetPowerUps();
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
        FindObjectOfType<AudioManager>().Play("ButtonPress");
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
        GC.ResetPowerUps();
        GC.ChangeMultiplier(0);
        FindObjectOfType<AudioManager>().Stop("RegularLevel");
        FindObjectOfType<AudioManager>().Stop("Boss");
        FindObjectOfType<AudioManager>().Stop("GameOver");
        FindObjectOfType<AudioManager>().Play("MainMenu");
        FindObjectOfType<AudioManager>().Play("ButtonPress");
        SceneManager.LoadScene("Menu");
        Debug.Log("Bringing to the menu...");
    }

    //===============PROCEDURE===============//
    public void Quit()
    //Purpose:          Quits the application if button is chosen
    {
        Debug.Log("Exiting the game...");
        FindObjectOfType<AudioManager>().Play("ButtonPress");
        Application.Quit();
    }
}
