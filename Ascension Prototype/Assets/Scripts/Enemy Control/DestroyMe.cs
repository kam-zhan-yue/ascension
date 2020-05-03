using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    public float timeTilDeath;
    private float deathTimer;
    public bool death;

    // Update is called once per frame
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
