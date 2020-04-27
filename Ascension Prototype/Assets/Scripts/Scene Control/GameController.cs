using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int level;
    public string playerName;
    private int points;
    private int multiplier;
    private static GameObject instance;
    public GameObject cam;
    private bool camSpawn;

    public float time;
    public float bonus;

    public bool addEntry;

    private void Awake()
    {
        //Error prevention to prevent more than one instance of GameController
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
        level = 1;
        points = 0;
        multiplier = 0;
        if (string.IsNullOrEmpty(MainMenu.playerName))
            playerName = "Player";
        else
            playerName = MainMenu.playerName;
        DontDestroyOnLoad(gameObject);
    }
    

    public void AddPoints(int add)
    {
        points += add;
        Debug.Log("Points: " + points);
    }

    public void ResetPoints()
    {
        points = 0;
    }

    public int GetPoints()
    {
        return points;
    }

    public void ChangeMultiplier(int newMultiplier)
    {
        multiplier = newMultiplier;
        Debug.Log("Multiplier is: "+multiplier);
    }

    public int GetMultiplier()
    {
        return multiplier;
    }
}
