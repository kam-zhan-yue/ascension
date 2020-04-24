    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextController : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI timeElapsed;
    public TextMeshProUGUI timeBonus;
    public TextMeshProUGUI doorText;
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

    public void UpdateBoss(int health)
    {
        if(health>70)
            PhaseText(0, "The Minotaur awaits you in his dungeon", 5f);
        else if(health>=29)
            PhaseText(1, "The Minotaur is angry! He will become invulnerable when charging", 4f);
        else
            PhaseText(2, "The Minotaur is tired! Avoid his heavy, sluggish attacks", 4f);
    }

    public void PhaseText(int index, string quote, float time)
    {
        if(!phase[index])
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

    public void UpdateDoor()
    {
        if (activateDoor)
            doorText.text = "Press [E] to Advance";
        else if (bossText)
            doorText.text = "You cannot advance!";
        else
            doorText.text = "";
    }

    public void UpdateTimer()
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

    private string ConvertToTime(float time)
    {
        string minutes = Mathf.Floor((time % 3600)/60).ToString("00");
        string seconds = Mathf.Floor(time % 60).ToString("00");
        string timer = minutes + ":" + seconds;
        return timer;
    }
}
