/*
 * Author: Alexander Kam
 * Date: 30-5-20
 * Licence: Unity Personal Editor Licence
 */
using UnityEngine;

public class PowerUpSpawn : MonoBehaviour
{
    public GameObject powerUp;
    public int chance;
    private void Start()
    {
        Vector2 spawnPos = new Vector2(this.transform.position.x, this.transform.position.y);
        if(Spawn(chance))
            Instantiate(powerUp, spawnPos, Quaternion.identity);
    }

    //===============FUNCTION===============//
    bool Spawn(int chance)
    //Purpose:          Spawns a powerup based on a certain chance
    {
        bool spawn = false;
        if ((int)Random.Range(1, chance) == 1)
            spawn = true;
        return spawn;
    }
}
