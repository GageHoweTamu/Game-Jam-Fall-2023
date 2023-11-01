using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpPadScript : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    public float boostStrength = 10f;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Gorilla") || collision.gameObject.CompareTag("Cheetah"))
        {
            rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                if (gameObject.transform.rotation.eulerAngles.z == 90) //up
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(Vector3.up * boostStrength, ForceMode2D.Impulse);
                    //might need to do stuff with grounded checking if combining this with gorilla
                }
                else if (gameObject.transform.rotation.eulerAngles.z == 270) //down
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(Vector3.down * boostStrength, ForceMode2D.Impulse);
                    //might need to do stuff with grounded checking if combining this with gorilla
                }
                else if (gameObject.transform.rotation.eulerAngles.z == 180)//left
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    rb.AddForce(Vector3.left * boostStrength, ForceMode2D.Impulse);
                    //might need to do stuff with grounded checking if combining this with gorilla
                }
                else if (gameObject.transform.rotation.eulerAngles.z == 0) //right
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    rb.AddForce(Vector3.right * boostStrength, ForceMode2D.Impulse);
                    //might need to do stuff with grounded checking if combining this with gorilla
                }

            }
            else
            {
                Debug.Log("rb missing: " + collision.gameObject.name);
            }
        }
    }
}
