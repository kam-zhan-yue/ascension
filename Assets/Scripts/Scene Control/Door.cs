/*
 * Author: Alexander Kam
 * Date: 30-5-20
 * Licence: Unity Personal Editor Licence
 */
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform popUp;
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
        if(GC.level ==1)
        {
            FindObjectOfType<TextController>().StartLevel();
        }
        if ((GC.level - 1) % 3 == 0 && GC.level != 1)
        {
            FindObjectOfType<TextController>().UpdateLevel();
        }
        FindObjectOfType<AudioManager>().Play("DoorClosing");
    }

    //===============PROCEDURE===============//
    private void OnTriggerEnter2D(Collider2D collision)
    //Purpose:          Plays a door open animation or doesn't if boss is still alive
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

    //===============PROCEDURE===============//
    private void OnTriggerStay2D(Collider2D collision)
    //Purpose:          Lets the player go through if a button is pressed and allocates points
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
                PointsUp(200 + GC.GetMultiplier() * 100);

                //Add bonus if bonus achieved
                if (GC.time < GC.bonus)
                {
                    GC.AddPoints(100 + GC.GetMultiplier() * 50);
                    PointsUp(100 + GC.GetMultiplier() * 50);
                    FindObjectOfType<AudioManager>().Play("Bonus");
                }
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

    //===============PROCEDURE===============//
    void PointsUp(int points)
    //Purpose:          Creates pop up text for points achieved
    {
        Vector3 pos = transform.position;
        pos += new Vector3(0, 0.5f);
        popUp.GetComponent<TextPopUp>().PointsPopUp(points);
    }

    //===============PROCEDURE===============//
    private void OnTriggerExit2D (Collider2D collision)
    //Purpose:          Plays a door closing animation
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
