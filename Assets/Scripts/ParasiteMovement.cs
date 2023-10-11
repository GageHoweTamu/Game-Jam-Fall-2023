using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerController3 : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2;
    private bool isGrounded;
    private float horizontal;
    private bool isFacingRight = true;
    public GameObject gorilla;
    public GorillaController gorillaScript;
    public string gorillaName = "Gorilla(Clone)";
    public GameObject cheetah;
    public CheetahController cheetahScript;
    public string cheetahName = "Cheetah";
    public bool controlling;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    //animator vars
    private Animator anim;
    private Transform anim_child;
    public float idleWalkThresholdSpeed;
    public float walkRunThresholdSpeed;
    public float walkProportionalAnimSpeed;
    private bool jump;
    private bool fall;
    private bool squash;
    public Vector3 scale;
    public AudioSource FlapAudioData;
    public AudioSource BackgroundMusicData;

    // Start is called before the first frame update
    private void Start()
    {
        isGrounded = false;
        controlling = false;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponentsInChildren<Animator>()[1];
        anim_child = transform.Find("anim_child");
        jump = false;
        BackgroundMusicData.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.transform.tag.Equals("Platform"))
        {
            isGrounded = true;

        }
        if ((collision.gameObject.tag == "Gorilla") && (!controlling)) 
        {
            gorilla = collision.gameObject;
            gorillaScript = collision.gameObject.GetComponent<GorillaController>();
            rb.simulated = false; //turns off the rigidbody, disabling gravity and collisions unitl turned back on
            sprite.enabled = false; //turns off sprite
            gameObject.transform.position = new Vector3(gorilla.transform.position.x, gorilla.transform.position.y, -3); //moves the player to match the controlled enemy, not needed if sprite is removed
            controlling = true; //condition tracking current state: parasite or controlling enemy
            collision.gameObject.tag = "Player";
            gameObject.tag = "Untagged";
        }
        if ((collision.gameObject.tag == "Cheetah") && (!controlling)) 
        {
            cheetah = collision.gameObject;
            cheetahScript = collision.gameObject.GetComponent<CheetahController>();
            rb.simulated = false; //turns off the rigidbody, disabling gravity and collisions unitl turned back on
            sprite.enabled = false; //turns off sprite
            gameObject.transform.position = new Vector3(cheetah.transform.position.x, cheetah.transform.position.y, -3); //moves the player to match the controlled enemy, not needed if sprite is removed
            controlling = true; //condition tracking current state: parasite or controlling enemy
            collision.gameObject.tag = "Player";
            gameObject.tag = "Untagged";
        }
    }


    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (controlling == true)
        {
            if (GameObject.FindWithTag("Player").name == gorillaName) 
            {
                gorillaScript.Controlled(horizontal); //tells the controlled enemy to move
                transform.position = new Vector3(gorilla.transform.position.x, gorilla.transform.position.y, -3); //moves the player to match the controlled enemy, not needed if sprite is removed
                if (Input.GetKeyDown(KeyCode.E))
                {
                    controlling = false;
                    transform.position = new Vector3(gorilla.transform.position.x, (gorilla.transform.position.y + 2), 0); //moves player away from other entities, can be replaced with whatever movement we want the parasite to do when leaving an enemy
                    rb.simulated = true; //turns the rigidbody back on so its effected by gravity and collisions
                    sprite.enabled = true; //turns the sprite back on
                    gameObject.tag = "Player";
                    Destroy(gorilla);
                }
            }
            if (GameObject.FindWithTag("Player").name == cheetahName)
            {
                cheetahScript.Controlled(horizontal); //tells the controlled enemy to move
                transform.position = new Vector3(cheetah.transform.position.x, cheetah.transform.position.y, -3); //moves the player to match the controlled enemy, not needed if sprite is removed
                if (Input.GetKeyDown(KeyCode.E))
                {
                    controlling = false;
                    transform.position = new Vector3(cheetah.transform.position.x, (cheetah.transform.position.y + 2), 0); //moves player away from other entities, can be replaced with whatever movement we want the parasite to do when leaving an enemy
                    rb.simulated = true; //turns the rigidbody back on so its effected by gravity and collisions
                    sprite.enabled = true; //turns the sprite back on
                    gameObject.tag = "Player";
                    Destroy(cheetah);
                }
            }
        }
        else
        {
            Flip();
            //ANIMATION CONTROLS
            anim.SetBool("grounded", isGrounded);
            jump = !isGrounded && rb.velocity.y > 0;
            anim.SetBool("jump",jump);
            squash = (Input.GetAxis("Vertical") < 0.0f && isGrounded && Mathf.Abs(rb.velocity.x) < idleWalkThresholdSpeed);
            anim.SetBool("squash",squash);
            /* cool idea but sprites are origined on their center so this causes floating
            if(squash)
            {
                float new_scale_y = Mathf.Max(transform.localScale.y - 0.5f*Time.deltaTime, scale.y*0.5f);
                transform.localScale = new Vector3(transform.localScale.x, new_scale_y, transform.localScale.z);
            } else 
            {
                float new_scale_y = Mathf.Min(transform.localScale.y + 0.5f*Time.deltaTime, scale.y);
                transform.localScale = new Vector3(transform.localScale.x, new_scale_y, transform.localScale.z);
            }
            */

            if (isGrounded) // grounded
            {
                //ANIMATION CONTROLS
                float speed = Mathf.Abs(rb.velocity.x);
                anim.SetFloat("anim_speed_mult", Mathf.Abs(rb.velocity.x) * walkProportionalAnimSpeed);
                anim.SetBool("walk", speed >= idleWalkThresholdSpeed);
                anim.SetBool("run", speed >= walkRunThresholdSpeed);

                if (Input.GetAxis("Horizontal") != 0.0f)
                {
                    rb.velocity = new Vector2(Input.GetAxis("Horizontal") * movementSpeed, rb.velocity.y);
                    // gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime, 0.0f, 0.0f);
                    // gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime, 0.0f, 0.0f), ForceMode2D.Impulse);
                }
                anim_child.rotation = Quaternion.Euler(0.0f,0.0f,0.0f);
            }
            else // not grounded
            {
                //-- ROTATION WHILE IN AIR
                //angle of attack
                if(rb.velocity.y != 0)
                {    
                    float alpha = 1.0f * Mathf.Rad2Deg*Mathf.Atan(rb.velocity.y / rb.velocity.x);
                    float tiltAroundZ = 1.0f * ((System.Convert.ToSingle(alpha >= 0.0f) - 0.5f)*2.0f) * Mathf.Pow(Mathf.Abs(alpha),0.9f);
                    //
                    RaycastHit2D hit;
                    hit = Physics2D.Raycast (transform.position, Vector2.down, Mathf.Infinity);
                    if(hit.collider != null && hit.distance - 0.75f <= 1.75f && rb.velocity.y < 0)
                    {
                        Quaternion target = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        anim_child.rotation = Quaternion.Slerp(anim_child.rotation, target,  Time.deltaTime * 4.0f);
                    } else
                    {
                        Quaternion target = Quaternion.Euler(0, 0, tiltAroundZ);
                        anim_child.rotation = Quaternion.Slerp(anim_child.rotation, target,  Time.deltaTime * 2.0f);
                    }
                }
                if (Input.GetAxis("Horizontal") != 0.0f)
                {
                    rb.velocity = new Vector2(Input.GetAxis("Horizontal") * movementSpeed * 2.0f, rb.velocity.y);
                    
                    // gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal") * movementSpeed * 2 * Time.deltaTime, 0.0f, 0.0f);
                    // gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime, 0.0f, 0.0f), ForceMode2D.Impulse);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                //JUMP
                isGrounded = false;
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 9.0f, ForceMode2D.Impulse);
                FlapAudioData.Play();
            }
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

}