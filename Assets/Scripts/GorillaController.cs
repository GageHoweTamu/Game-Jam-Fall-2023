using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GorillaController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2;
    public bool isGrounded;
    private Rigidbody2D rb;
    private float jpower;
    private float timer;
    private float horizontal;
    public bool controlled;
    private bool isFacingRight = true;
    public bool attacking;
    GameObject player;
    private PlayerController3 parasiteScript;
    public float trackingRangeX = 70f;
    public float trackingRangeY = 40f;
    public float detectingRangeY = 5f;
    public float attackRange = 2.5f;
    private Vector2 direction;
    private int interval = 0; //not important
    private int movementFlipper = 1; //not important
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip wallBreak;

    public AudioSource audioSource;

    // Start is called before the first frame update
    private void Start()
    {
        isGrounded = false;
        controlled = false;
        attacking = false;
        Debug.Log("Start");
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 0.5f;
        player = GameObject.Find("PlayerParasite"); //this could be problematic depending on how we load things
        parasiteScript = player.gameObject.GetComponent<PlayerController3>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Wall") && attacking)
        {
            Destroy(collision.gameObject);
            audioSource.PlayOneShot(wallBreak);
        }
        else if (collision.gameObject.CompareTag("Spike"))
        {
            Die(parasiteScript.x_pos, parasiteScript.y_pos);
        }
        //Debug.Log("Collision");
    }

    private void Update()
    {
        if (parasiteScript.GetNormalGrav())
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
        //UnityEngine.Debug.Log("casting ray");
        if ((groundHitLeft.collider != null && groundHitMiddle.collider != null) || (groundHitRight.collider != null && groundHitMiddle.collider != null))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            //UnityEngine.Debug.Log("not grounded"); 
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!controlled) //put all enemy ai behavior in here
        {
            if (interval % 50 == 0)
            {
                movementFlipper *= -1;
            }
            gameObject.transform.position += new Vector3(movementFlipper * movementSpeed * 2 * Time.deltaTime, 0.0f, 0.0f);
            ++interval;
        }
        /*
        if (Mathf.Abs(player.transform.position.x - rb.position.x) < trackingRangeX && Mathf.Abs(player.transform.position.y - rb.position.y) < trackingRangeY)
        {
            AIFlip(player.transform.position.x);
            //jump code
            if (player.transform.position.y > rb.position.y && (player.transform.position.y - rb.position.y) > detectingRangeY && isGrounded && !attacking)
            {
                //change the value below to change jump height
                //rb.AddForce(Vector2.up * 6f, ForceMode2D.Impulse);
                isGrounded = false;
            }
            //attack code
            if ((Mathf.Abs(rb.position.x - player.transform.position.x) < attackRange) && isGrounded && !attacking)
            {
                //Attack();
            }
            //move code
            else
            {
                if (isFacingRight)
                {
                    //transform.position += Vector3.right * movementSpeed * Time.deltaTime;
                }
                else
                {
                    //transform.position += Vector3.left * movementSpeed * Time.deltaTime;
                }
            }
        }
        */
    }


    public void Controlled(bool normalGrav) //all controlled functions need to set controlled to true to stop ai behavior
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        controlled = true; //no need to set to false after being controlled as enemy will die when parasite leaves
        if (normalGrav)
        {
            direction = Vector2.up;
        }
        else
        {
            direction = Vector2.down;
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
          gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal") * movementSpeed * 3 * Time.deltaTime, 0.0f, 0.0f);
            
        }

        Flip();
        if (Input.GetButtonDown("Jump") && isGrounded && !parasiteScript.paused)
        {
            timer = Time.time;
        }
        if (Input.GetButtonUp("Jump") && isGrounded && !parasiteScript.paused)
        {

            isGrounded = false;
            Debug.Log(Time.time - timer);
            jpower = (Time.time - timer)*2;
            if (jpower > 1)
            {
                jpower = 1;
            }
            rb.AddForce(direction * (jpower * 10), ForceMode2D.Impulse);
            audioSource.PlayOneShot(jumpSound);

        }
        if (Input.GetButtonDown("Fire1") && !parasiteScript.paused)
        {
            Attack();
            audioSource.PlayOneShot(attackSound);
        }
    }

    public void Attack()
    {
        if (!attacking)
        {
            if (isFacingRight)
            {
                attacking = true;
                rb.AddForce(Vector2.right * 3, ForceMode2D.Impulse); //gorilla attack right
                Invoke("StopAttack", 1);
            }
            if (!isFacingRight)
            {
                attacking = true;
                rb.AddForce(Vector2.left * 3, ForceMode2D.Impulse); //gorilla attack left
                Invoke("StopAttack", 1);
            }
        }
    }

    private void StopAttack()
    {
        attacking = false;
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

    //flip, but works while not controlled
    private void AIFlip(float playerX)
    {
        if (isFacingRight && playerX < rb.position.x || !isFacingRight && playerX > rb.position.x)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void Die(float x_pos, float y_pos)
    {
        audioSource.PlayOneShot(deathSound);
        gameObject.transform.position = new Vector3(x_pos, y_pos, 0.0f);
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }
        if (parasiteScript.spawnCheetah) //gorilla to cheetah - spike
        {
            parasiteScript.cheetah = Instantiate(parasiteScript.prefabCheetah, gameObject.transform.position, gameObject.transform.rotation);
            parasiteScript.cheetahScript = parasiteScript.cheetah.GetComponent<CheetahController>();
            parasiteScript.hostRB = parasiteScript.cheetah.GetComponent<Rigidbody2D>();
            Destroy(gameObject);
            parasiteScript.controlCheetah();
        }
        else if (parasiteScript.spawnParasite) //gorilla to parasite - spike
        {
            parasiteScript.gameObject.transform.position = new Vector3(x_pos, y_pos, 0.0f);
            parasiteScript.controlling = false;
            parasiteScript.rb.simulated = true;
            parasiteScript.gameObject.tag = "Player";
            parasiteScript.anim_child.gameObject.SetActive(true);
            Destroy(gameObject);
        }
        parasiteScript.gravityFlipper.ResetFlippers(parasiteScript.GetNormalGrav(), parasiteScript.respawnNormalGrav);
        parasiteScript.cam.MoveToNewRoom(parasiteScript.respawnRoom);
        if (parasiteScript.respawnRoom == parasiteScript.troubleRoom)
        {
            parasiteScript.troubleDoor1.GetComponent<BoxCollider2D>().enabled = false;
            parasiteScript.troubleDoor1.GetComponent<Door>().partnerDoor.GetComponent<BoxCollider2D>().enabled = true;
            parasiteScript.troubleDoor2.GetComponent<BoxCollider2D>().enabled = false;
            parasiteScript.troubleDoor2.GetComponent<Door>().partnerDoor.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (parasiteScript.bigRoom)
        {
            parasiteScript.cam.ChangeCamSize(8.0f, true);
        }
        else
        {
            parasiteScript.cam.ChangeCamSize(4.0f, false);
        }
    }
}
