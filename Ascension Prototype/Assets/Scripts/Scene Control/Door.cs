using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    Animator anim;
    public GameObject GameController;
    public GameController GC;
    public GameObject TextController;
    public TextController TC;
    public GameObject Boss;
    public Boss bossScript;
    public bool bossLevel;
    public bool advanceLevel;
    public int mainMenu = 0;
    public int mapGenerator = 1;
    public int boss = 2;

    private void Start()
    {
        anim = GetComponent<Animator>();
        GameController = GameObject.FindGameObjectWithTag("GameController");
        GC = GameController.GetComponent<GameController>();
        TextController = GameObject.FindGameObjectWithTag("TextController");
        TC = TextController.GetComponent<TextController>();
        //Assign Door Coordinates to the player
        Vector2 position= transform.position;
        //Find boss if on appropriate level
        if(GC.level % 3 ==0)
        {
            bossLevel = true;
            Boss = GameObject.FindGameObjectWithTag("Boss");
            bossScript = Boss.GetComponent<Boss>();
        }
        FindObjectOfType<AudioManager>().Play("DoorClosing");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (bossLevel)
            {
                //If boss is dead, open door
                if (bossScript.dead)
                {
                    Debug.Log("Player is standing on door, opening...");
                    TC.activateDoor = true;
                    TC.bossText = false;
                    anim.SetBool("Open", true);
                }
                else
                {
                    //If boss is dead, keep door closed
                    Debug.Log("Boss is still alive, do not open door");
                    TC.activateDoor = false;
                    TC.bossText = true;
                }
            }
            //If on a regular level, advance normally
            else
            {
                Debug.Log("Player is standing on door, opening...");
                TC.activateDoor = true;
                TC.bossText = false;
                anim.SetBool("Open", true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                FindObjectOfType<AudioManager>().Play("DoorOpening");
                //If boss is still alive, do not progress
                if (bossLevel)
                {
                    if (!bossScript.dead)
                        return;
                }
                //Prevent player from skipping levels by spamming
                if (advanceLevel)
                    return;
                advanceLevel = true;

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
                    StartCoroutine(FindObjectOfType<LevelLoader>().LoadLevel(boss));
                    Debug.Log("Level is now:" + GC.level);
                    Debug.Log("Spawn a boss battle");
                }
                //Else, reset scene
                else
                {
                    //After every 3 rounds, increase the muliplier by 1
                    if ((GC.level - 1) % 3 == 0)
                        GC.ChangeMultiplier((GC.level - 1) / 3);
                    StartCoroutine(FindObjectOfType<LevelLoader>().LoadLevel(mapGenerator));
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
            TC.activateDoor = false;
            TC.bossText = false;
        }
    }
}
