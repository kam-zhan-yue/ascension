using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public GameObject player;
    public Skeleton skeleton;
    public FlyingEye flyingEye;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.layer);
        if(skeleton != null)
        {
            if (collision.gameObject.layer == 11 && !skeleton.dead)
            {
                player.GetComponent<PlayerController>().TakeDamage(transform, 2f, 2f);
            }
        }
        if(flyingEye != null)
        {
            if (collision.gameObject.layer == 11 && !flyingEye.dead)
            {
                player.GetComponent<PlayerController>().TakeDamage(transform, 2f, 2f);
            }
        }
    }

    public void Disable()
    {
        this.enabled = false;
    }
}
