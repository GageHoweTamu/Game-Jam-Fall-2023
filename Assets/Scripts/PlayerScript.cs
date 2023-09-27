using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float horizontal = 0;
    private float vertical = 0;
    private float movementSpeed = 2;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public GorillaController host;
    public bool controlling;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        controlling = false;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (controlling == true)
        {
            host.Controlled(horizontal); //tells the controlled enemy to move
            transform.position = new Vector3(host.transform.position.x, host.transform.position.y, -3); //moves the player to match the controlled enemy, not needed if sprite is removed
            if (Input.GetKeyDown(KeyCode.E))
            {
                controlling = false;
                transform.position = new Vector3(host.transform.position.x, (host.transform.position.y + 2), 0); //moves player away from other entities, can be replaced with whatever movement we want the parasite to do when leaving an enemy
                rb.simulated = true; //turns the rigidbody back on so its effected by gravity and collisions
                sprite.enabled = true; //turns the sprite back on
            }
        }
        else
        {
            gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal") * movementSpeed * 3 * Time.deltaTime, 0.0f, 0.0f); //if not controlling an enemy, moves itself
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) //when it hits an enemy
    {
        if ((collision.gameObject.layer == 3) && (!controlling)) //for different enemies, all that matters is they are on the same game layer (ex. 3: Enemy), and all have same names for functions to be called (ex. host.Controlled)
        {
            host = collision.gameObject.GetComponent<GorillaController>();
            rb.simulated = false; //turns off the rigidbody, disabling gravity and collisions unitl turned back on
            sprite.enabled = false; //turns off sprite
            transform.position = new Vector3(host.transform.position.x, host.transform.position.y, -3); //moves the player to match the controlled enemy, not needed if sprite is removed
            controlling = true; //condition tracking current state: parasite or controlling enemy
        }
    }
}
