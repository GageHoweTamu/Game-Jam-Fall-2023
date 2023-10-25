using Pathfinding.Legacy;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController3 : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2;
    public bool isGrounded;
    private float horizontal;
    private bool isFacingRight = true;
    public GameObject gorilla;
    public GorillaController gorillaScript;
    public string gorillaName = "Gorilla(Clone)";
    public GameObject cheetah;
    public CheetahController cheetahScript;
    public string cheetahName = "Cheetah";
    public bool controlling;
    public bool leaping;
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
    //sound vars
    public AudioSource FlapAudioData;
    public AudioSource BackgroundMusicData;
    //text vars
    public GameObject gorillaControlPopUp;
    public Canvas pauseMenu;


   


    // Start is called before the first frame update
    private void Start()
    {
        isGrounded = false;
        controlling = false;
        leaping = false;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponentsInChildren<Animator>()[1];
        anim_child = transform.Find("anim_child");
        jump = false;
        BackgroundMusicData.Play();
        gorillaControlPopUp.SetActive(false);
    
       

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Gorilla") && (!controlling) && leaping) 
        {
            /*
            TextToDisplay = "Press space to jump";
            GameObject GorillaText = Instantiate(controlTextPrefab, gorilla.transform.position.x, gorilla.transform.position.y + 5, 0);
            GorillaText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(textToDisplay);
            */
            gorilla = collision.gameObject;
            gorillaScript = collision.gameObject.GetComponent<GorillaController>();
            rb.simulated = false; //turns off the rigidbody, disabling gravity and collisions until turned back on
            sprite.enabled = false; //turns off sprite
            gameObject.transform.position = new Vector3(gorilla.transform.position.x, gorilla.transform.position.y, -3); //moves the player to match the controlled enemy, not needed if sprite is removed
            controlling = true; //condition tracking current state: parasite or controlling enemy
            leaping = false;
            collision.gameObject.tag = "Player";
            gameObject.tag = "Untagged";
            anim_child.gameObject.SetActive(false);
            gorillaControlPopUp.SetActive(true);
        }
        if ((collision.gameObject.tag == "Cheetah") && (!controlling) && leaping) 
        {
            cheetah = collision.gameObject;
            cheetahScript = collision.gameObject.GetComponent<CheetahController>();
            rb.simulated = false; //turns off the rigidbody, disabling gravity and collisions unitl turned back on
            sprite.enabled = false; //turns off sprite
            gameObject.transform.position = new Vector3(cheetah.transform.position.x, cheetah.transform.position.y, -3); //moves the player to match the controlled enemy, not needed if sprite is removed
            controlling = true; //condition tracking current state: parasite or controlling enemy
            leaping = false;
            collision.gameObject.tag = "Player";
            gameObject.tag = "Untagged";
            anim_child.gameObject.SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        //Because raycast is from middle of body, can't jump when edge of body is on a platform - no coyote time
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f);
        UnityEngine.Debug.DrawRay(transform.position, Vector2.down * 0.5f, Color.red);
        //UnityEngine.Debug.Log("casting ray");
        if (groundHit.collider != null)
        {
            isGrounded = true;
            leaping = false;
        }
        else 
        { 
            isGrounded = false; 
            //UnityEngine.Debug.Log("not grounded"); 
        }



        horizontal = Input.GetAxisRaw("Horizontal");
        Flip();
        if (controlling == true)
        {
            if (GameObject.FindWithTag("Player").name == gorillaName) 
            {
                gorillaScript.Controlled(); //tells the controlled enemy to move
                transform.position = new Vector3(gorilla.transform.position.x, gorilla.transform.position.y, -3); //moves the player to match the controlled enemy, not needed if sprite is removed
                if (Input.GetMouseButtonDown(1) && !leaping)
                {
                    controlling = false;
                    leaping = true;
                    transform.position = new Vector3(gorilla.transform.position.x, (gorilla.transform.position.y), 0); //moves player away from other entities, can be replaced with whatever movement we want the parasite to do when leaving an enemy
                    rb.simulated = true; //turns the rigidbody back on so its effected by gravity and collisions
                    if (isFacingRight)
                    {
                        rb.velocity = new Vector2(movementSpeed * 2.0f, movementSpeed);
                    }
                    else
                    {
                        rb.velocity = new Vector2(movementSpeed * -2.0f, movementSpeed);
                    }
                    //sprite.enabled = true; //turns the sprite back on
                    gameObject.tag = "Player";
                    Destroy(gorilla);
                    anim_child.gameObject.SetActive(true);
                }
            }
            if (GameObject.FindWithTag("Player").name == cheetahName)
            {
                cheetahScript.Controlled(); //tells the controlled enemy to move
                transform.position = new Vector3(cheetah.transform.position.x, cheetah.transform.position.y, -3); //moves the player to match the controlled enemy, not needed if sprite is removed
                if (Input.GetMouseButtonDown(1) && !leaping)
                {
                    controlling = false;
                    leaping = true;
                    transform.position = new Vector3(cheetah.transform.position.x, (cheetah.transform.position.y), 0); //moves player away from other entities, can be replaced with whatever movement we want the parasite to do when leaving an enemy
                    rb.simulated = true; //turns the rigidbody back on so its effected by gravity and collisions
                    if (isFacingRight)
                    {
                        rb.velocity = new Vector2(movementSpeed * 2.0f, movementSpeed);
                    }
                    else
                    {
                        rb.velocity = new Vector2(movementSpeed * -2.0f, movementSpeed);
                    }
                    //sprite.enabled = true; //turns the sprite back on
                    gameObject.tag = "Player";
                    Destroy(cheetah);
                    anim_child.gameObject.SetActive(true);
                }
            }
        }
        else
        {
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

            if (Input.GetMouseButtonDown(1) && isGrounded && !leaping)
            {
                leaping = true;
                if (isFacingRight)
                {
                    rb.velocity = new Vector2(movementSpeed * 2.0f, movementSpeed);
                }
                else
                {
                    rb.velocity = new Vector2(movementSpeed * -2.0f, movementSpeed);
                }
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
    
