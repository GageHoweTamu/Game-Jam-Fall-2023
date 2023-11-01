using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheetahController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5;
    public bool isGrounded;
    private Rigidbody2D rb;
    private float jpower;
    private float timer;
    private float horizontal;
    public bool controlled;
    private bool isFacingRight = true;
    public bool attacking;
    GameObject player;
    public float trackingRangeX = 70f;
    public float trackingRangeY = 40f;
    public float detectingRangeY = 5f;
    public float attackRange = 2.5f;

    // Start is called before the first frame update
    private void Start()
    {
        isGrounded = false;
        controlled = false;
        attacking = false;
        Debug.Log("Start");
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 0.5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Wall") && attacking)
        {
            Destroy(collision.gameObject);
        }
        Debug.Log("Collision");
    }

    private void Update()
    {
        RaycastHit2D groundHitLeft = Physics2D.Raycast(new Vector3(transform.position.x - 0.55f, transform.position.y, transform.position.z), Vector2.down, 0.5f);
        UnityEngine.Debug.DrawRay(new Vector3(transform.position.x - 0.55f, transform.position.y, transform.position.z), Vector2.down * 0.5f, Color.red);
        RaycastHit2D groundHitMiddle = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector2.down, 0.5f);
        UnityEngine.Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector2.down * 0.5f, Color.red);
        RaycastHit2D groundHitRight = Physics2D.Raycast(new Vector3(transform.position.x + 0.8f, transform.position.y, transform.position.z), Vector2.down, 0.5f);
        UnityEngine.Debug.DrawRay(new Vector3(transform.position.x + 0.8f, transform.position.y, transform.position.z), Vector2.down * 0.5f, Color.red);
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
            player = GameObject.FindGameObjectWithTag("Player");
            if (Mathf.Abs(player.transform.position.x - rb.position.x) < trackingRangeX && Mathf.Abs(player.transform.position.y - rb.position.y) < trackingRangeY)
            {
                AIFlip(player.transform.position.x);
                //jump code
                if (player.transform.position.y > rb.position.y && (player.transform.position.y - rb.position.y) > detectingRangeY && isGrounded && !attacking)
                {
                    //change the value below to change jump height
                    rb.AddForce(Vector3.up * 6f, ForceMode2D.Impulse);
                    isGrounded = false;
                }
                //attack code
                if ((Mathf.Abs(rb.position.x - player.transform.position.x) < attackRange) && isGrounded && !attacking)
                {
                    Attack();
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
        }
    }

    public void Controlled() //all controlled functions need to set controlled to true to stop ai behavior
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        controlled = true; //no need to set to false after being controlled as enemy will die when parasite leaves
        if (Input.GetAxis("Horizontal") != 0)
        {
          gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal") * movementSpeed * 3 * Time.deltaTime, 0.0f, 0.0f);
            
        }

        Flip();

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * 3, ForceMode2D.Impulse);
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
}