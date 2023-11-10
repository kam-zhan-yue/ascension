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

public class MainMenu : MonoBehaviour
{
    public string playerName;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI placeHolder;
    public AudioMixer mixer;
    public Slider slider;
    public HighscoreTable HT;

    void Start()
    {
        if (PlayerPrefs.HasKey("highscoreTable") == false)
            HT.CreateNewHighscores();
        if (PlayerPrefs.HasKey("Name") == false)
            PlayerPrefs.SetString("Name", "PlayerName");
        if (PlayerPrefs.HasKey("Volume") == false)
            PlayerPrefs.SetFloat("Volume", 100);
        nameText.text = PlayerPrefs.GetString("Name");
        slider.value = PlayerPrefs.GetFloat("Volume");
    }

    //===============PROCEDURE===============//
    public void RefreshName()
    //Purpose:          Sets the text field to the chosen player name
    {
        nameText.text = PlayerPrefs.GetString("Name");
    }

    //===============PROCEDURE===============//
    public void SetName()
    //Purpose:          Changes the name to to a new name set by the user
    {
        PlayerPrefs.SetString("Name", nameText.text);
        int ascii = 0;
        foreach(char c in PlayerPrefs.GetString("Name"))
            ascii += (int)c;
        Debug.Log(PlayerPrefs.GetString("Name"));
        Debug.Log(ascii);
        //If left empty (or some weird ascii), change to a set name
        if (string.IsNullOrWhiteSpace(PlayerPrefs.GetString("Name")) || ascii == 8203)
        {
            PlayerPrefs.SetString("Name", "PlayerName");
            Debug.Log(PlayerPrefs.GetString("Name"));
        }
    }

    //===============PROCEDURE===============//
    public void SetVolume()
    //Purpose:          Sets the slider value to the preset volume level
    {
        float volume = slider.value;
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    //===============PROCEDURE===============//
    public void StartGame()
    //Purpose:          Transistions into the game scene
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //===============PROCEDURE===============//
    public void ButtonAudio()
    //Purpose:          Plays the button audio
    {
        FindObjectOfType<AudioManager>().Play("ButtonPress");
    }

    //===============PROCEDURE===============//
    public void QuitGame()
    //Purpose:          Exits the game
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
