using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update\
    public float speed = 20f;
    public Rigidbody rb;

    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);
        Destroy(gameObject);
    }
}
