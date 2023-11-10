/*
 * Author: Alexander Kam
 * Date: 30-5-20
 * Licence: Unity Personal Editor Licence
 */
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    public float timeTilDeath;
    private float deathTimer;
    public bool death;
    
    void Update()
    {
        if(death)
        {
            deathTimer += Time.deltaTime;
            if(deathTimer >= timeTilDeath)
            {
                Destroy(gameObject);
                this.enabled = false;
            }
        }
    }
}
