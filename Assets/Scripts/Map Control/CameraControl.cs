/*
 * Author: Alexander Kam
 * Date: 30-5-20
 * Licence: Unity Personal Editor Licence
 */
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour
{
    //Player that is being followed
    public GameObject playerGO;
    public Transform player;

    //Zeroing out the velocity
    Vector3 velocity = Vector3.zero;

    //Time to follow target
    public float smoothTime = .15f;

    //Enable and set the max Y value
    public bool YMaxEnabled = false;
    public float YMaxValue = 0f;

    //Enable and set the min Y value
    public bool YMinEnabled = false;
    public float YMinValue = 0f;

    //Enable and set the max X value
    public bool XMaxEnabled = false;
    public float XMaxValue = 0f;

    //Enable and set the min X value
    public bool XMinEnabled = false;
    public float XMinValue = 0f;

    //Map Generator Variables
    public MapGenerator MG;

    public bool foundPlayer;

    void Start()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        if(buildIndex==1)
            MG = GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>();
    }

    void FixedUpdate()
    {
        //Error Prevention
        if (!foundPlayer)
            playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO == null)
            return;
        else
        {
            player = playerGO.transform;
            foundPlayer = true;
        }

        //Target position
        Vector3 playerPos = player.position;

        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        //Vertical Clamp if in Map Generation
        if (buildIndex == 1)
        {
            if (player.position.y < -25)
                playerPos.y = -30;
            else if (player.position.y < -15)
                playerPos.y = -20;
            else if (player.position.y < -5)
                playerPos.y = -10;
            else
                playerPos.y = 0;
            bool ascending = PlayerIsAscending(player.position, MG.y, MG.x);
            if (ascending)
            {
                playerPos.y = player.position.y;
                if (player.position.y < -30)
                    playerPos.y = -30;
            }
        }

        //Vertical Clamping
        if (YMinEnabled && YMaxEnabled)
        {
            playerPos.y = Mathf.Clamp(player.position.y, YMinValue, YMaxValue);
        }
        else if (YMinEnabled)
        {
            playerPos.y = Mathf.Clamp(player.position.y, YMinValue, player.position.y);
        }
        else if (YMaxEnabled)
        {
            playerPos.y = Mathf.Clamp(player.position.y, player.position.y, YMaxValue);
        }

        //Horizontal Clamping
        if (XMinEnabled && XMaxEnabled)
        {
            playerPos.x = Mathf.Clamp(player.position.x, XMinValue, XMaxValue);
        }
        else if (XMinEnabled)
        {
            playerPos.x = Mathf.Clamp(player.position.x, XMinValue, player.position.x);
        }
        else if (XMaxEnabled)
        {
            playerPos.x = Mathf.Clamp(player.position.x, player.position.x, XMaxValue);
        }

        //Align camera and the player's z position
        playerPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref velocity, smoothTime);
    }

    //===============PROCEDURE===============//
    void VerticalClampMG(Vector3 playerPos)
    //Purpose:          Clamps if the player is at an ascending room
    {
        Debug.Log("working");
        //Vertical Clamping for Map Generation
        if (player.position.y < -25)
            playerPos.y = -30;
        else if (player.position.y < -15)
            playerPos.y = -20;
        else if (player.position.y < -5)
            playerPos.y = -10;
        else
            playerPos.y = 0;
        bool ascending = PlayerIsAscending(player.position, MG.y, MG.x);
        if (ascending)
        {
            playerPos.y = player.position.y;
            if (player.position.y < -30)
                playerPos.y = -30;
        }
    }
    
    //===============PROCEDURE===============//
    public static bool PlayerIsAscending(Vector3 playerPosition, float[] y, float[] x)
    //Purpose:          Checks if the player is in a room that is going upwards
    {
        bool ascending = false;
        for (int i = 0; i < y.Length; i++)
        {
            if (playerPosition.x > x[i] * 20 - 10 && playerPosition.x < x[i] * 20 + 10
                && playerPosition.y > y[i] * -10 - 10 && playerPosition.y < y[i] * -10 + 10)
                ascending = true;
        }
        return ascending;
    }

    //===============PROCEDURE===============//
    public IEnumerator Shake(float duration, float magnitude)
    //Purpose:         To shake the camera in a random 
    {
        Debug.Log("Shake Screen");
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = originalPos;
    }
}
