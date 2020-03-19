using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int level;
    public int points;
    private static GameObject instance;

    private void Awake()
    {
        level = 1;
        points = 0;
        //Error prevention to prevent more than one instance of GameController
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
    }
}
