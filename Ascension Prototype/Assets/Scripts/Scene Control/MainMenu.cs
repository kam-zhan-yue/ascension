using System.Collections;
using System.Collections.Generic;
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

    public void RefreshName()
    {
        nameText.text = PlayerPrefs.GetString("Name");
    }

    public void SetName()
    {
        PlayerPrefs.SetString("Name", nameText.text);
        int ascii = 0;
        foreach(char c in PlayerPrefs.GetString("Name"))
            ascii += (int)c;
        Debug.Log(PlayerPrefs.GetString("Name"));
        Debug.Log(ascii);
        if (string.IsNullOrWhiteSpace(PlayerPrefs.GetString("Name")) || ascii == 8203)
        {
            PlayerPrefs.SetString("Name", "PlayerName");
            Debug.Log(PlayerPrefs.GetString("Name"));
        }
    }

    public void SetVolume()
    {
        float volume = slider.value;
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ButtonAudio()
    {
        FindObjectOfType<AudioManager>().Play("ButtonPress");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
