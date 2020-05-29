using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int level;
    private int points;
    private int multiplier;
    private static GameObject instance;
    public GameObject cam;
    private bool camSpawn;
    public float time;
    public float bonus;

    public bool addEntry;

    //Power up variables
    public int damageUp;
    public int jumpUp;
    public int movementUp;
    public float damageMultiplier;
    public float movementMultiplier;
    public float jumpMultiplier;

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
        ResetPowerUps();
        DontDestroyOnLoad(gameObject);
    }

    public void DamagePowerUp()
    {
        damageUp++;
        damageMultiplier += 0.3f;
        FindObjectOfType<PowerUpsUI>().AddDamage(damageUp);
        FindObjectOfType<TextController>().UpdatePowerUps(1);
    }

    public void MovementPowerUp()
    {
        movementUp++;
        movementMultiplier += 0.01f;
        FindObjectOfType<PowerUpsUI>().AddMovement(movementUp);
        FindObjectOfType<TextController>().UpdatePowerUps(2);
    }

    public void JumpPowerUp()
    {
        jumpUp++;
        jumpMultiplier += 0.04f;
        FindObjectOfType<PowerUpsUI>().AddJump(jumpUp);
        FindObjectOfType<TextController>().UpdatePowerUps(3);
    }

    public void ResetPowerUps()
    {
        damageUp = 0;
        movementUp = 0;
        jumpUp = 0;
        damageMultiplier = 1f;
        movementMultiplier = 1f;
        jumpMultiplier = 1f;
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
