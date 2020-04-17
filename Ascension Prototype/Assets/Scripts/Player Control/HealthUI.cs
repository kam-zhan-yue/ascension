using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public GameObject[] heart = new GameObject[5];
    private Animator[] heartAnim = new Animator[5];

    private void Start()
    {
        for (int i = 0; i < heart.Length; i++)
        {
            //Error Prevention
            if(heart[i]!=null)
                heartAnim[i] = heart[i].GetComponent<Animator>();
        }
    }

    public void Hurt(int health)
    {
        //Error Prevention
        if(health-1>-1)
            heartAnim[health - 1].SetBool("hit", true);
    }

}
