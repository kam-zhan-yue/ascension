/*
 * Author: Alexander Kam
 * Date: 30-5-20
 * Licence: Unity Personal Editor Licence
 */
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

    //===============PROCEDURE===============//
    public void Hurt(int health)
    //Purpose:          Depletes hearts accoridng to the player's health
    {
        //Error Prevention
        if (health-1>-1)
            heartAnim[health - 1].SetBool("hit", true);
    }
}
