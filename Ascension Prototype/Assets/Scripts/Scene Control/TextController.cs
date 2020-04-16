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
    }

    private string ConvertToTime(float time)
    {
        string minutes = Mathf.Floor((time % 3600)/60).ToString("00");
        string seconds = Mathf.Floor(time % 60).ToString("00");
        string timer = minutes + ":" + seconds;
        return timer;
    }
}
