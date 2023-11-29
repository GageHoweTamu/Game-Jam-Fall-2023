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
    public GameObject prefabGorilla;
    public GorillaController gorillaScript;
    public string gorillaName = "Gorilla(Clone)";
    public GameObject cheetah;
    public GameObject prefabCheetah;
    public CheetahController cheetahScript;
    public string cheetahName = "Cheetah";
    private string nameAry;
    public bool controlling;
    public bool leaping;
    private SpriteRenderer sprite;
    public Rigidbody2D rb;
    public Rigidbody2D hostRB;
    [SerializeField] private bool normalGrav;
    private Vector2 direction;
    //animator vars
    private Animator anim;
    public Transform anim_child;
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
    public AudioClip deathSound;
    public AudioClip leapSound;
    //text vars
    public GameObject gorillaControlPopUp;
    public Canvas pauseMenu;
    //respawn vars
    public GameObject cameraScript;
    public float x_pos;
    public float y_pos;
    public bool spawnParasite = false;
    public bool spawnGorilla = false;
    public bool spawnCheetah = false;
    public bool respawnNormalGrav = true;
    public gravityFlipperScript gravityFlipper;
    public Transform respawnRoom;
    public CameraController cam;
    public bool bigRoom = false;
    public Transform troubleRoom;
    public GameObject troubleDoor1;
    public GameObject troubleDoor2;
    public bool paused;

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
        pauseMenu.enabled = false;
        normalGrav = true;
        gravityFlipper = GameObject.FindGameObjectWithTag("GravityFlipper").GetComponent<gravityFlipperScript>();
        paused = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Platform"))
        {
            if (normalGrav)
            {
                direction = Vector2.down;
            }
            else
            {
                direction = Vector2.up;
            }
            RaycastHit2D groundHitLeft = Physics2D.Raycast(new Vector3(transform.position.x - 0.55f, transform.position.y, transform.position.z), direction, 0.5f);
            UnityEngine.Debug.DrawRay(new Vector3(transform.position.x - 0.55f, transform.position.y, transform.position.z), direction * 0.5f, Color.red);
            RaycastHit2D groundHitMiddle = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), direction, 0.5f);
            UnityEngine.Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), direction * 0.5f, Color.red);
            RaycastHit2D groundHitRight = Physics2D.Raycast(new Vector3(transform.position.x + 0.8f, transform.position.y, transform.position.z), direction, 0.5f);
            UnityEngine.Debug.DrawRay(new Vector3(transform.position.x + 0.8f, transform.position.y, transform.position.z), direction * 0.5f, Color.red);
            if ((groundHitLeft.collider != null && groundHitMiddle.collider != null) || (groundHitRight.collider != null && groundHitMiddle.collider != null))
            {
                leaping = false;
                leaping = false;
                UnityEngine.Debug.Log("hit ground, done leaping");
            }
        }
        if ((collision.gameObject.tag == "Gorilla") && (!controlling) && leaping) 
        {
            /*
            TextToDisplay = "Press space to jump";
            GameObject GorillaText = Instantiate(controlTextPrefab, gorilla.transform.position.x, gorilla.transform.position.y + 5, 0);
            GorillaText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(textToDisplay);
            */
            gorilla = collision.gameObject;
            gorillaScript = collision.gameObject.GetComponent<GorillaController>();
            hostRB = collision.gameObject.GetComponent<Rigidbody2D>();
            controlGorilla();
        }
        if ((collision.gameObject.tag == "Cheetah") && (!controlling) && leaping) 
        {
            cheetah = collision.gameObject;
            cheetahScript = collision.gameObject.GetComponent<CheetahController>();
            hostRB = collision.gameObject.GetComponent<Rigidbody2D>();
            controlCheetah();
        }
        if(collision.gameObject.tag == "Spike")
        {
            Die();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (normalGrav)
        {
            direction = Vector2.down;
        }
        else
        {
            direction = Vector2.up;
        }
        RaycastHit2D groundHitLeft = Physics2D.Raycast(new Vector3(transform.position.x - 0.55f, transform.position.y, transform.position.z), direction, 0.5f);
        UnityEngine.Debug.DrawRay(new Vector3(transform.position.x - 0.55f, transform.position.y, transform.position.z), direction * 0.5f, Color.red);
        RaycastHit2D groundHitMiddle = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), direction, 0.5f);
        UnityEngine.Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), direction * 0.5f, Color.red);
        RaycastHit2D groundHitRight = Physics2D.Raycast(new Vector3(transform.position.x + 0.8f, transform.position.y, transform.position.z), direction, 0.5f);
        UnityEngine.Debug.DrawRay(new Vector3(transform.position.x + 0.8f, transform.position.y, transform.position.z), direction * 0.5f, Color.red);
        if ((groundHitLeft.collider != null && groundHitMiddle.collider != null) || (groundHitRight.collider != null && groundHitMiddle.collider != null))
        {
            isGrounded = true;
        }
        else 
        { 
            isGrounded = false; 
            //UnityEngine.Debug.Log("not grounded"); 
        }
        if(Input.GetButtonDown("Pause"))
        {
            Time.timeScale = 0;
            pauseMenu.enabled = true;
            paused = true;
        }



        horizontal = Input.GetAxisRaw("Horizontal");
        Flip();
        if (controlling == true)
        {
            nameAry = GameObject.FindWithTag("Player").name.Substring(0,7);
            if (nameAry == gorillaName) 
            {
                gorillaScript.Controlled(normalGrav); //tells the controlled enemy to move
                transform.position = new Vector3(gorilla.transform.position.x, gorilla.transform.position.y, -3); //moves the player to match the controlled enemy, not needed if sprite is removed
                if (Input.GetButtonDown("Fire2") && !leaping)
                {
                    FlapAudioData.PlayOneShot(leapSound);
                    controlling = false;
                    leaping = true;
                    UnityEngine.Debug.Log("leaping from gorilla");
                    if (normalGrav)
                    {
                        transform.position = new Vector3(gorilla.transform.position.x, gorilla.transform.position.y + 1.5f, 0); //moves player away from other entities, can be replaced with whatever movement we want the parasite to do when leaving an enemy
                    }
                    else
                    {
                        transform.position = new Vector3(gorilla.transform.position.x, gorilla.transform.position.y - 1.5f, 0);
                    }
                    rb.simulated = true; //turns the rigidbody back on so its effected by gravity and collisions
                    if (isFacingRight)
                    {
                        if (normalGrav)
                        {
                            rb.velocity = new Vector2(movementSpeed * 2.0f, movementSpeed);
                        }
                        else
                        {
                            rb.velocity = new Vector2(movementSpeed * 2.0f, movementSpeed * -1f);
                        }
                    }
                    else
                    {
                        if (normalGrav)
                        {
                            rb.velocity = new Vector2(movementSpeed * 2.0f, movementSpeed);
                        }
                        else
                        {
                            rb.velocity = new Vector2(movementSpeed * 2.0f, movementSpeed * -1f);
                        }
                    }
                    //sprite.enabled = true; //turns the sprite back on
                    gameObject.tag = "Player";
                    Destroy(gorilla);
                    anim_child.gameObject.SetActive(true);
                }
            }
            if (nameAry == cheetahName)
            {
                cheetahScript.Controlled(normalGrav); //tells the controlled enemy to move
                transform.position = new Vector3(cheetah.transform.position.x, cheetah.transform.position.y, -3); //moves the player to match the controlled enemy, not needed if sprite is removed
                if (Input.GetButtonDown("Fire2") && !leaping)
                {
                    FlapAudioData.PlayOneShot(leapSound);
                    controlling = false;
                    leaping = true;
                    if (normalGrav)
                    {
                        transform.position = new Vector3(cheetah.transform.position.x, cheetah.transform.position.y + 1.5f, 0); //moves player away from other entities, can be replaced with whatever movement we want the parasite to do when leaving an enemy
                    }
                    else
                    {
                        transform.position = new Vector3(cheetah.transform.position.x, cheetah.transform.position.y - 1.5f, 0);
                    }
                    rb.simulated = true; //turns the rigidbody back on so its effected by gravity and collisions
                    if (isFacingRight)
                    {
                        if (normalGrav)
                        {
                            rb.velocity = new Vector2(movementSpeed * 2.0f, movementSpeed);
                        }
                        else
                        {
                            rb.velocity = new Vector2(movementSpeed * 2.0f, movementSpeed * -1f);
                        }
                    }
                    else
                    {
                        if (normalGrav)
                        {
                            rb.velocity = new Vector2(movementSpeed * 2.0f, movementSpeed);
                        }
                        else
                        {
                            rb.velocity = new Vector2(movementSpeed * 2.0f, movementSpeed * -1f);
                        }
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

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                //JUMP
                isGrounded = false;
                if (normalGrav)
                {
                    rb.AddForce(Vector3.up * 9.0f, ForceMode2D.Impulse);
                }
                else
                {
                    rb.AddForce(Vector3.down * 9.0f, ForceMode2D.Impulse);
                }
                FlapAudioData.Play();
            }

            if (Input.GetButtonDown("Fire2") && isGrounded && !leaping)
            {
                FlapAudioData.PlayOneShot(leapSound);
                leaping = true;
                if (isFacingRight)
                {
                    if (normalGrav)
                    {
                        rb.velocity = new Vector2(movementSpeed * 2.0f, movementSpeed);
                    }
                    else
                    {
                        rb.velocity = new Vector2(movementSpeed * 2.0f, movementSpeed * -1f);
                    }
                }
                else
                {
                    if (normalGrav)
                    {
                        rb.velocity = new Vector2(movementSpeed * -2.0f, movementSpeed);
                    }
                    else
                    {
                        rb.velocity = new Vector2(movementSpeed * -2.0f, movementSpeed * -1f);
                    }
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

    public void Die() //pause menu/parasite hit spike
    {
        if (!controlling)
        {
            FlapAudioData.PlayOneShot(deathSound);
            UnityEngine.Debug.Log("Dead");
            gameObject.transform.position = new Vector3(x_pos, y_pos, 0.0f);
            rb.velocity = Vector3.zero;
            gravityFlipper.ResetFlippers(normalGrav, respawnNormalGrav);
            if (spawnGorilla) //parasite to gorilla - error from pause menu
            {
                leaping = true;
                gorilla = Instantiate(prefabGorilla, gameObject.transform.position, gameObject.transform.rotation);
                gorillaScript = gorilla.GetComponent<GorillaController>();
                hostRB = gorilla.GetComponent<Rigidbody2D>();
                controlGorilla();
            }
            else if (spawnCheetah) //parasite to cheetah - error from pause menu
            {
                leaping = true;
                cheetah = Instantiate(prefabCheetah, gameObject.transform.position, gameObject.transform.rotation);
                cheetahScript = cheetah.GetComponent<CheetahController>();
                hostRB = cheetah.GetComponent<Rigidbody2D>();
                controlCheetah();
            }
        }
        else if (gorilla != null) //pause menu as gorilla
        {
            gorilla.transform.position = new Vector3(x_pos, y_pos, 0.0f);
            if (spawnGorilla) //gorilla to gorilla
            {
                gorillaScript.Die(x_pos, y_pos);
            }
            else if (spawnCheetah) //gorilla to cheetah
            {
                Destroy(gorilla);
                cheetah = Instantiate(prefabCheetah, gorilla.transform.position, gameObject.transform.rotation);
                cheetahScript = cheetah.GetComponent<CheetahController>();
                hostRB = cheetah.GetComponent<Rigidbody2D>();
                controlCheetah();
            }
            else if (spawnParasite) //gorilla to parasite
            {
                gameObject.transform.position = new Vector3(x_pos, y_pos, 0.0f);
                Destroy(gorilla);
                controlling = false;
                rb.simulated = true;
                gameObject.tag = "Player";
                anim_child.gameObject.SetActive(true);
            }
        }
        else if (cheetah != null) //pause menu as cheetah
        {
            cheetah.transform.position = new Vector3(x_pos, y_pos, 0.0f);
            if (spawnCheetah) //cheetah to cheetah
            {
                cheetahScript.Die(x_pos, y_pos);
            }
            else if (spawnGorilla) //cheetah to gorilla
            {
                Destroy(cheetah);
                gorilla = Instantiate(prefabGorilla, cheetah.transform.position, gameObject.transform.rotation);
                gorillaScript = gorilla.GetComponent<GorillaController>();
                hostRB = gorilla.GetComponent<Rigidbody2D>();
                controlGorilla();
            }
            else if (spawnParasite) //cheetah to parasite
            {
                gameObject.transform.position = new Vector3(x_pos, y_pos, 0.0f);
                Destroy(cheetah);
                controlling = false;
                rb.simulated = true;
                gameObject.tag = "Player";
                anim_child.gameObject.SetActive(true);
            }
        }
        cam.MoveToNewRoom(respawnRoom);
        if (respawnRoom == troubleRoom)
        {
            troubleDoor1.GetComponent<BoxCollider2D>().enabled = false;
            troubleDoor1.GetComponent<Door>().partnerDoor.GetComponent<BoxCollider2D>().enabled = true;
            troubleDoor2.GetComponent<BoxCollider2D>().enabled = false;
            troubleDoor2.GetComponent<Door>().partnerDoor.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (bigRoom)
        {
            cam.ChangeCamSize(8.0f, true);
        }
        else
        {
            cam.ChangeCamSize(4.0f, false);
        }
    }

    public void controlGorilla()
    {
        hostRB.velocity = Vector3.zero;
        rb.simulated = false; //turns off the rigidbody, disabling gravity and collisions until turned back on
        sprite.enabled = false; //turns off sprite
        gameObject.transform.position = new Vector3(gorilla.transform.position.x, gorilla.transform.position.y, -3); //moves the player to match the controlled enemy, not needed if sprite is removed
        controlling = true; //condition tracking current state: parasite or controlling enemy
        leaping = false;
        gorilla.tag = "Player";
        gameObject.tag = "Untagged";
        anim_child.gameObject.SetActive(false);
        gorillaControlPopUp.SetActive(true);
    }

    public void controlCheetah()
    {
        hostRB.velocity = Vector3.zero;
        rb.simulated = false; //turns off the rigidbody, disabling gravity and collisions unitl turned back on
        sprite.enabled = false; //turns off sprite
        gameObject.transform.position = new Vector3(cheetah.transform.position.x, cheetah.transform.position.y, -3); //moves the player to match the controlled enemy, not needed if sprite is removed
        controlling = true; //condition tracking current state: parasite or controlling enemy
        leaping = false;
        cheetah.tag = "Player";
        gameObject.tag = "Untagged";
        anim_child.gameObject.SetActive(false);
    }
    /*
    public void GetRespawnPos(Transform doorpos)
    {
        x_pos = doorpos.position.x;
        y_pos = doorpos.position.y;
    }
    */
    public void GravityFlip()
    {
        normalGrav = !normalGrav;
        Vector3 localScalePlayer = transform.localScale;
        localScalePlayer.y *= -1f;
        transform.localScale = localScalePlayer;
        rb.gravityScale *= -1f;
        if (gorilla != null)
        {
            Vector3 localScaleGorilla = gorilla.transform.localScale;
            localScaleGorilla.y *= -1f;
            gorilla.transform.localScale = localScaleGorilla;
            hostRB.gravityScale *= -1f;
        }
        else if (cheetah != null)
        {
            Vector3 localScaleCheetah = cheetah.transform.localScale;
            localScaleCheetah.y *= -1f;
            cheetah.transform.localScale = localScaleCheetah;
            hostRB.gravityScale *= -1f;
        }
    }

    public bool GetNormalGrav()
    {
        return normalGrav;
    }

}
    
