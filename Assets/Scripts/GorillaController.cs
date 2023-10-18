using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private bool attacking = false;
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
        Debug.Log("Start");
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 0.5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Platform"))
        {
            isGrounded = true;
            Debug.Log("Grounded");
        }
        if (collision.transform.tag.Equals("Wall") && attacking)
        {
            Destroy(collision.gameObject);
        }
        Debug.Log("Collision");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Platform"))
        {
            isGrounded = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!controlled) //put all enemy ai behavior in here
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (!attacking)
            {
                Vector2 target = new Vector2(player.transform.position.x, rb.position.y);
                Vector2 newPos = Vector2.MoveTowards(rb.position, target, movementSpeed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }
            //jump code - doesn't look or feel good
            if (Mathf.Abs(player.transform.position.y - rb.position.y) < trackingRangeY && (player.transform.position.y - rb.position.y) > detectingRangeY && isGrounded && !attacking)
            {
                rb.AddForce(Vector3.up * 4.5f, ForceMode2D.Impulse);
                isGrounded = false;
            }
            //attack code - attack() wasnt working but works now i guess
            if ((Mathf.Abs(rb.position.x - player.transform.position.x) < attackRange) && isGrounded)
            {
                Attack();
            }
        }
    }

    public void Controlled() //all controlled functions need to set controlled to true to stop ai behavior
    {
        rb.gravityScale = 1;
        horizontal = Input.GetAxisRaw("Horizontal");
        controlled = true; //no need to set to false after being controlled as enemy will die when parasite leaves
        if (Input.GetAxis("Horizontal") != 0)
        {
          gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal") * movementSpeed * 3 * Time.deltaTime, 0.0f, 0.0f);
            
        }

        Flip();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            timer = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.Space) && isGrounded)
        {

            isGrounded = false;
            Debug.Log(Time.time - timer);
            jpower = Time.time - timer;
            if (jpower > 1)
            {
                jpower = 1;
            }
            rb.AddForce(Vector3.up * (jpower * 8), ForceMode2D.Impulse);
            
        }
        if (Input.GetMouseButtonDown(1))
        {
            Attack();
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
}
