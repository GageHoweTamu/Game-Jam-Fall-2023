using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorillaController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2;
    public bool isGrounded;
    private Rigidbody2D rb;
    private float jpower;
    private float timer;
    private float horizontal;
    private bool controlled;
    private bool isFacingRight = true;
    private bool attacking = false;
    private int interval = 0; //not important
    private int movementFlipper = 1; //not important
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
    

    // Update is called once per frame
    void Update()
    {
        if (!controlled) //put all enemy ai behavior in here
        {
            if (interval % 20 == 0)
            {
                movementFlipper *= -1;
            }
            gameObject.transform.position += new Vector3(movementFlipper * movementSpeed * 3 * Time.deltaTime, 0.0f, 0.0f);
            ++interval;
        }
    }

    public void Controlled(float horizontal) //all controlled functions need to set controlled to true to stop ai behavior
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        controlled = true; //no need to set to false after being controlled as enemy will die when parasite leaves
        if (Input.GetAxis("Horizontal") != 0)
        {
          gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal") * movementSpeed * 3 * Time.deltaTime, 0.0f, 0.0f);
            attacking = false;
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
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * (jpower * 8), ForceMode2D.Impulse);
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (isFacingRight)
            {
                attacking = true;
                rb.AddForce(Vector2.right * 3, ForceMode2D.Impulse); //gorilla attack right
            }
            if (!isFacingRight)
            {
                rb.AddForce(Vector2.left * 3, ForceMode2D.Impulse); //gorilla attack left
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
