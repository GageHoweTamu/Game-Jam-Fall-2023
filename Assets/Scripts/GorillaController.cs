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
    //animation vars
    private Animator anim;
    // public Transform anim_child;
    private float idleWalkThresholdSpeed = 0.2f;
    private float walkProportionalAnimSpeed = 1.0f;
    //
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
        //animator
        anim = GetComponentsInChildren<Animator>()[0];
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
        //ANIMATION CONTROLS
        anim.SetFloat("anim_speed_mult", Mathf.Abs(rb.velocity.x) * walkProportionalAnimSpeed);
        //
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
            rb.velocity = new Vector2(movementFlipper * movementSpeed * 2, rb.velocity.y);
            // would rather not do finite derivatives on transform.position
            // gameObject.transform.position += new Vector3(movementFlipper * movementSpeed * 2 * Time.deltaTime, 0.0f, 0.0f);
            ++interval;
            AIFlip();
        }
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
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * movementSpeed * 3, rb.velocity.y);
        //   gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal") * movementSpeed * 3 * Time.deltaTime, 0.0f, 0.0f);
            
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

    //flips based on velocity
    private void AIFlip()
    {
        if(isFacingRight && rb.velocity.x < 0 || !isFacingRight && rb.velocity.x > 0)
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
        parasiteScript.ResetDoors(parasiteScript.respawnRoom);
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
