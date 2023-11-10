/*
 * Author: Alexander Kam
 * Date: 30-5-20
 * Licence: Unity Personal Editor Licence
 */
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject GameController;
    public GameController GC;
    public bool FlyingEye;
    public bool Skeleton;
    public GameObject FE;
    public GameObject SK;
    // Start is called before the first frame update
    void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController");
        GC = GameController.GetComponent<GameController>();
        for(int i=0; i<GC.GetMultiplier()+3; i++)
        {
            if(FlyingEye && Spawn())
            {
                Vector2 spawnPos = new Vector2(this.transform.position.x, this.transform.position.y);
                spawnPos += Random.insideUnitCircle.normalized;
                Instantiate(FE, spawnPos, Quaternion.identity);
            }
            if(Skeleton && Spawn())
                Instantiate(SK, new Vector3(this.transform.position.x, this.transform.position.y, -1), Quaternion.identity);
        }
    }

    //===============FUNCTION===============//
    bool Spawn()
    //Purpose:          Has a 10% chance of returning true to spawn an enemy
    {
        bool spawn = false;
        if ((int)Random.Range(1, 10) == 1)
            spawn = true;
        return spawn;
    }
}
