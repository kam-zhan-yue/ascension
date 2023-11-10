/*
 * Author: Alexander Kam
 * Date: 30-5-20
 * Licence: Unity Personal Editor Licence
 */
using UnityEngine;
using System.Collections;
using TMPro;

public class TextController : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI timeElapsed;
    public TextMeshProUGUI timeBonus;
    public TextMeshProUGUI doorText;
    public TextMeshProUGUI textNotification;
    public bool activateDoor;
    public bool bossText;

    public TextMeshProUGUI bossPhases;
    public bool activateText;
    public bool[] phase = new bool[3];
    public float timeOnScreen = 0;

    public GameObject GameController;
    public GameController GC;

    float time;
    float bonus;


    private void Update()
    {
        if (GC == null)
        {
            GameController = GameObject.FindGameObjectWithTag("GameController");
            GC = GameController.GetComponent<GameController>();
        }
        else
        {
            UpdateTimer();
            UpdateDoor();
        }
    }

    //===============PROCEDURE===============//
    public void UpdateLevel()
    //Purpose:          Plays notification that enemies have gotten stronger
    {
        StartCoroutine(TextUpdate(textNotification, "The enemies seem stronger...",3f));
    }

    //===============PROCEDURE===============//
    public void StartLevel()
    //Purpose:          Plays notification of the objective of the game
    {
        StartCoroutine(TextUpdate(textNotification, "Ascend to the top and defeat enemies to gain points.", 4f));
    }

    //===============PROCEDURE===============//
    public void UpdatePowerUps(int powerup)
    //Purpose:          Plays notification of the power up gained
    {
        switch (powerup)
        {
            case 1:
                StartCoroutine(TextUpdate(textNotification, "Your damage has increased", 3f));
            break;
            case 2:
                StartCoroutine(TextUpdate(textNotification, "Your speed has increased", 3f));
                break;
            case 3:
                StartCoroutine(TextUpdate(textNotification, "Your jump force has increased", 3f));
                break;
            case 4:
                StartCoroutine(TextUpdate(textNotification, "You cannot power up anymore", 3f));
                break;
        }
    }

    //===============PROCEDURE===============//
    public IEnumerator TextUpdate(TextMeshProUGUI textField, string quote, float duration)
    //Purpose:          Puts a text notification on screen for a specified amount of time
    {
        textField.text = quote;
        Debug.Log("Changed text");
        yield return new WaitForSecondsRealtime(duration);
        textField.text = "";
        Debug.Log("Erased text");
    }

    //===============PROCEDURE===============//
    public void UpdateBoss(int health)
    //Purpose:          Plays text notifications based on boss behaviour
    {
        if (health>70)
            PhaseText(0, "The Minotaur awaits you in his dungeon", 5f);
        else if(health>=29)
            PhaseText(1, "The Minotaur is angry! He will become invulnerable when charging", 4f);
        else
            PhaseText(2, "The Minotaur is tired! Avoid his heavy, sluggish attacks", 4f);
    }

    //===============PROCEDURE===============//
    public void PhaseText(int index, string quote, float time)
    //Purpose:          Controls the amount of time remaining for the boss text
    {
        if (!phase[index])
        {
            bossPhases.text = quote;
            timeOnScreen += Time.deltaTime;
            if (timeOnScreen >= time)
            {
                phase[index] = true;
                bossPhases.text = "";
                timeOnScreen = 0;
            }
        }
    }

    //===============PROCEDURE===============//
    public void UpdateDoor()
    //Purpose:          Changes text of the door if the player is on it or not or if the boss is alive
    {
        if (activateDoor)
            doorText.text = "Press [E] to Advance";
        else if (bossText)
            doorText.text = "You cannot advance!";
        else
            doorText.text = "";
    }

    //===============PROCEDURE===============//
    public void UpdateTimer()
    //Purpose:          Updates the timer, the bonus time, and the points
    {
        bonus = 60 + GC.GetMultiplier() * 15;
        time += Time.deltaTime;
        string elapsedMinutes = (time % 3600).ToString("00");
        string elapsedSeconds = (time % 60).ToString("00");
        GC.time = time;
        GC.bonus = bonus;

        timeElapsed.text = "Time Elapsed: " + ConvertToTime(time);
        timeBonus.text = "Time Bonus: " + ConvertToTime(bonus);
        pointsText.text = "Points: " + GC.GetPoints().ToString();
    }

    //===============FUNCTION===============//
    private string ConvertToTime(float time)
    //Purpose:          Coverts a time float into minutes and seconds format
    {
        string minutes = Mathf.Floor((time % 3600)/60).ToString("00");
        string seconds = Mathf.Floor(time % 60).ToString("00");
        string timer = minutes + ":" + seconds;
        return timer;
    }
}
