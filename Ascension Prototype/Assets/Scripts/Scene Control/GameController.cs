using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int level;
    public int points;
    private static GameObject instance;
    public GameObject player;
    public GameObject cam;
    private bool camSpawn;

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

    //private void Update()
    //{
    //    if (SpawnCamera() && !camSpawn)
    //    {
    //        Instantiate(cam, new Vector3(player.transform.position.x, player.transform.position.y, -1), Quaternion.identity);
    //        camSpawn = false;
    //    }
    //}

    //private bool SpawnCamera()
    //{
    //    player = GameObject.FindGameObjectWithTag("Player");
    //    if (player == null)
    //        return true;
    //    else
    //        return false;
    //}
}
