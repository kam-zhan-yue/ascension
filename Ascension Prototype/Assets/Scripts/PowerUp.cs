using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    float i = 0;
    int maximumStat = 6;
    GameObject GameController;
    GameController GC;

    private void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController");
        GC = GameController.GetComponent<GameController>();
    }

    private void Update()
    {
        i += Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f* Mathf.Sin(i*1.7f), -1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Player Collided");
            Pickup();
        }
    }

    void Pickup()
    {
        //Randomly give a stat
        int choice = (int)Random.Range(1, 4);
        if (GC.jumpUp == maximumStat)
        {
            choice = 2;
            if(GC.damageUp == maximumStat)
            {
                choice = 3;
                if (GC.movementUp == maximumStat)
                    choice = 4;
            }
        }
        switch(choice)
        {
            case 1:
                FindObjectOfType<GameController>().JumpPowerUp();
                Debug.Log("Jump Power Up!");
                FindObjectOfType<AudioManager>().Play("PowerUp");
                Destroy(gameObject);
                break;
            case 2:
                FindObjectOfType<GameController>().DamagePowerUp();
                Debug.Log("Damage Power Up!");
                FindObjectOfType<AudioManager>().Play("PowerUp");
                Destroy(gameObject);
                break;
            case 3:
                FindObjectOfType<GameController>().MovementPowerUp();
                Debug.Log("Movement Power Up!");
                FindObjectOfType<AudioManager>().Play("PowerUp");
                Destroy(gameObject);
                break;
            case 4:
                FindObjectOfType<TextController>().UpdatePowerUps(4);
                break;
        }
    }
}
