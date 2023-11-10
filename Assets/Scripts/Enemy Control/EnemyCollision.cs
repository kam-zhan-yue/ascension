/*
 * Author: Alexander Kam
 * Date: 30-5-20
 * Licence: Unity Personal Editor Licence
 */
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public GameObject player;
    public Skeleton skeleton;
    public FlyingEye flyingEye;
    public Boss boss;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /* IMPORTANT NOTE:
      * PUT THE PLAYERDETECTION ON ANOTHER LAYER OTHER THAN ENEMY
      * THAT IS ALL
      * - AK
    */

    //===============PROCEDURE===============//
    private void OnTriggerEnter2D(Collider2D collision)
    //Purpose:          Damages the player if the enemy collides with the player
    {
        //Debug.Log(collision.gameObject.layer);
        if (skeleton != null)
        {
            if (collision.gameObject.layer == 11 && !skeleton.dead)
            {
                player.GetComponent<PlayerController>().TakeDamage(transform, 1, 2f);
            }
        }
        if(flyingEye != null)
        {
            if (collision.gameObject.layer == 11 && !flyingEye.dead)
            {
                player.GetComponent<PlayerController>().TakeDamage(transform, 1, 2f);
            }
        }
        if(boss!=null)
        {
            if (collision.gameObject.layer == 11 && !boss.dead)
            {
                Debug.Log("Player collided with boss");
                if (boss.charging == false)
                    player.GetComponent<PlayerController>().TakeDamage(transform, 1, 2f);
                else
                    player.GetComponent<PlayerController>().ChargeDamage(2, 20f);
            }
        }
    }

    //===============PROCEDURE===============//
    public void Disable()
    //Purpose:          Deactivates the script
    {
        this.enabled = false;
    }
}