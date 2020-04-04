using System.Collections;
using System.Collections.Generic;
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
        for(int i=0; i<GC.level+2; i++)
        {
            if(FlyingEye && Spawn())
                Instantiate(FE, new Vector3(this.transform.position.x, this.transform.position.y, -1), Quaternion.identity);
            if(Skeleton && Spawn())
                Instantiate(SK, new Vector3(this.transform.position.x, this.transform.position.y, -1), Quaternion.identity);
        }
    }

    bool Spawn()
    {
        bool spawn = false;
        if ((int)Random.Range(1, 10) == 1)
            spawn = true;
        return spawn;
    }
}
