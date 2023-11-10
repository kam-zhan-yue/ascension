/*
 * Author: Alexander Kam
 * Date: 30-5-20
 * Licence: Unity Personal Editor Licence
 */
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

    //===============PROCEDURE===============//
    public void DamagePowerUp()
    //Purpose:          Increases damage multiplier
    {
        damageUp++;
        damageMultiplier += 0.3f;
        FindObjectOfType<PowerUpsUI>().AddDamage(damageUp);
        FindObjectOfType<TextController>().UpdatePowerUps(1);
    }

    //===============PROCEDURE===============//
    public void MovementPowerUp()
    //Purpose:          Increases movement speed multiplier
    {
        movementUp++;
        movementMultiplier += 0.01f;
        FindObjectOfType<PowerUpsUI>().AddMovement(movementUp);
        FindObjectOfType<TextController>().UpdatePowerUps(2);
    }

    //===============PROCEDURE===============//
    public void JumpPowerUp()
    //Purpose:          Increases jump force multiplier
    {
        jumpUp++;
        jumpMultiplier += 0.04f;
        FindObjectOfType<PowerUpsUI>().AddJump(jumpUp);
        FindObjectOfType<TextController>().UpdatePowerUps(3);
    }

    //===============PROCEDURE===============//
    public void ResetPowerUps()
    //Purpose:          Resets power jumps and multipliers to zero
    {
        damageUp = 0;
        movementUp = 0;
        jumpUp = 0;
        damageMultiplier = 1f;
        movementMultiplier = 1f;
        jumpMultiplier = 1f;
    }

    //===============PROCEDURE===============//
    public void AddPoints(int add)
    //Purpose:          Adds to the encapsulated variable points
    {
        points += add;
        Debug.Log("Points: " + points);
    }

    //===============PROCEDURE===============//
    public void ResetPoints()
    //Purpose:          Resets the encapsulated variable points to zero
    {
        points = 0;
    }

    //===============FUNCTION===============//
    public int GetPoints()
    //Purpose:          Accessor method for points
    {
        return points;
    }

    //===============PROCEDURE===============//
    public void ChangeMultiplier(int newMultiplier)
    //Purpose:          Changes the value of the encapsulated variable multiplier
    {
        multiplier = newMultiplier;
        Debug.Log("Multiplier is: "+multiplier);
    }

    //===============FUNCTION===============//
    public int GetMultiplier()
    //Purpose:          Accessor method for multiplier
    {
        return multiplier;
    }
}
