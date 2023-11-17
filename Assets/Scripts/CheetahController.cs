using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheetahController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5;

    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip deathSound;

    public AudioSource audioSource;

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

    // Start is called before the first frame update
    private void Start()
    {
        isGrounded = false;
        controlled = false;
        attacking = false;
        Debug.Log("Start");
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 0.5f;
        player = GameObject.Find("PlayerParasite");
        parasiteScript = player.gameObject.GetComponent<PlayerController3>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            Die(parasiteScript.x_pos, parasiteScript.y_pos);
        }
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
            if (interval % 40 == 0)
            {
                movementFlipper *= -1;
            }
            gameObject.transform.position += new Vector3(movementFlipper * movementSpeed * 2 * Time.deltaTime, 0.0f, 0.0f);
            ++interval;
            /*
            if (Mathf.Abs(player.transform.position.x - rb.position.x) < trackingRangeX && Mathf.Abs(player.transform.position.y - rb.position.y) < trackingRangeY)
            {
                AIFlip(player.transform.position.x);
                //jump code
                if (player.transform.position.y > rb.position.y && (player.transform.position.y - rb.position.y) > detectingRangeY && isGrounded && !attacking)
                {
                    //change the value below to change jump height
                    rb.AddForce(direction * 6f, ForceMode2D.Impulse);
                    isGrounded = false;
                    audioSource.PlayOneShot(jumpSound);
                }
                //attack code
                if ((Mathf.Abs(rb.position.x - player.transform.position.x) < attackRange) && isGrounded && !attacking)
                {
                    Attack();
                    audioSource.PlayOneShot(attackSound);
                }
                //move code
                else
                {
                    if (isFacingRight)
                    {
                        transform.position += Vector3.right * movementSpeed * Time.deltaTime;
                    }
                    else
                    {
                        transform.position += Vector3.left * movementSpeed * Time.deltaTime;
                    }
                }
            }
            */
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
          gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal") * movementSpeed * 3 * Time.deltaTime, 0.0f, 0.0f);
            
        }

        Flip();

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(direction * 3, ForceMode2D.Impulse);
        }
    }

    public void Attack()
    {
        //if (!attacking)
        //{
            if (isFacingRight)
            {
                attacking = true;
                rb.AddForce(Vector2.right * 6, ForceMode2D.Impulse); //gorilla attack right
                //Invoke("StopAttack", 1);
            }
            if (!isFacingRight)
            {
                attacking = true;
                rb.AddForce(Vector2.left * 6, ForceMode2D.Impulse); //gorilla attack left
                //Invoke("StopAttack", 1);
            }
        //}
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
        if (parasiteScript.spawnGorilla) //cheetah to gorilla - spike
        {
            parasiteScript.gorilla = Instantiate(parasiteScript.prefabGorilla, gameObject.transform.position, gameObject.transform.rotation);
            parasiteScript.gorillaScript = parasiteScript.gorilla.GetComponent<GorillaController>();
            parasiteScript.hostRB = parasiteScript.gorilla.GetComponent<Rigidbody2D>();
            Destroy(gameObject);
            parasiteScript.controlGorilla();
        }
        else if (parasiteScript.spawnParasite) //cheetah to parasite - spike
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
    }
}