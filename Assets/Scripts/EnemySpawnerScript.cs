using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(enemy, new Vector3(5, -3, 0), transform.rotation);
        //Instantiate(enemy, new Vector3(-5, -3, 0), transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
