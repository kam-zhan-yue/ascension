using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int level;
    private int points;
    private int multiplier;
    private static GameObject instance;
    public GameObject player;
    public GameObject cam;
    private bool camSpawn;

    public float time;
    public float bonus;

    private void Awake()
    {
        level = 1;
        points = 0;
        multiplier = 0;
        //Error prevention to prevent more than one instance of GameController
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
    }

    public void AddPoints(int add)
    {
        points += add;
        Debug.Log("Points: " + points);
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
