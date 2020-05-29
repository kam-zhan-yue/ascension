using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsUI : MonoBehaviour
{
    //Power Ups
    public GameObject movementIcon;
    public GameObject damageIcon;
    public GameObject jumpIcon;
    public float movementX = -150f;
    public float damageX = -100f;
    public  float jumpX = -25f;
    public float y = -20f;
    public int verticalDistance = 50;
    GameObject GameController;
    GameController GC;

    private void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController");
        GC = GameController.GetComponent<GameController>();
        Debug.Log(GC.damageUp);
        for (int i = 1; i < GC.damageUp +1; i++)
            AddDamage(i);
        for (int i = 1; i < GC.jumpUp +1; i++)
            AddJump(i);
        for (int i = 1; i <GC.movementUp +1; i++)
            AddMovement(i);
    }

    public void AddJump(int multiplier)
    {
        Vector2 spawnPos = new Vector2(jumpX, y - verticalDistance * multiplier);
        Debug.Log(multiplier);
        Debug.Log(verticalDistance);
        Debug.Log(verticalDistance * multiplier);
        GameObject x = Instantiate(jumpIcon, spawnPos, Quaternion.identity);
        x.transform.SetParent(transform, false);
    }

    public void AddDamage(int multiplier)
    {
        Vector2 spawnPos = new Vector2(damageX, y - verticalDistance * multiplier);
        Debug.Log(multiplier);
        Debug.Log(verticalDistance);
        Debug.Log(verticalDistance * multiplier);
        GameObject x = Instantiate(damageIcon, spawnPos, Quaternion.identity);
        x.transform.SetParent(transform, false);
    }

    public void AddMovement(int multiplier)
    {
        Vector2 spawnPos = new Vector2(movementX, y - verticalDistance * multiplier);
        Debug.Log(multiplier);
        Debug.Log(verticalDistance);
        Debug.Log(verticalDistance * multiplier);
        GameObject x = Instantiate(movementIcon, spawnPos, Quaternion.identity);
        x.transform.SetParent(transform, false);
    }
}
