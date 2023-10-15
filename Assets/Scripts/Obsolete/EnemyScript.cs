using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private float speed = 250;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Controlled(float horizontal, float vertical) //gets called by parasite, moves when told to
    {
        rb.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, vertical * speed * Time.fixedDeltaTime);
    }
}
