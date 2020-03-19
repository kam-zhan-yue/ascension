using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        Instantiate(player, new Vector3(this.transform.position.x, this.transform.position.y,-1), Quaternion.identity);
    }
}
