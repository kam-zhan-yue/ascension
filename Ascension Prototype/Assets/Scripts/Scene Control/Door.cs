using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    Animator anim;
    public GameObject GameController;
    public GameController GC;

    private void Start()
    {
        anim = GetComponent<Animator>();
        GameController = GameObject.FindGameObjectWithTag("GameController");
        GC = GameController.GetComponent<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Player is standing on door, opening...");
            anim.SetBool("Open", true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("New Scene");
                //Increment level by 1 and add points
                GC.level++;
                GC.AddPoints(200 + GC.GetMultiplier() * 100);
                //Add bonus if bonus achieved
                if (GC.time < GC.bonus)
                    GC.AddPoints(100 + GC.GetMultiplier() * 50);
                //Every three levels, spawn a boss scene
                if (GC.level%3==0)
                {
                    SceneManager.LoadScene(2);
                    Debug.Log("Level is now:" + GC.level);
                    Debug.Log("Spawn a boss battle");
                }
                //Else, reset scene
                else
                {
                    //After every 3 rounds, increase the muliplier by 1
                    if ((GC.level - 1) % 3 == 0)
                        GC.ChangeMultiplier((GC.level - 1) / 3);
                    SceneManager.LoadScene(1);
                    Debug.Log("Level is now:" + GC.level);
                    Debug.Log("Spawn a regular map");
                }
            }
        }
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player has left the door, closing...");
            anim.SetBool("Open", false);
        }
    }
}
