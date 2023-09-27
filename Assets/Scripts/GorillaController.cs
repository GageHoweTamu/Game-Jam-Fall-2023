using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorillaController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2;
    private bool isGrounded;
    private Rigidbody2D rb;
    private float jpower;
    private float timer;
    private bool controlled;
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
        controlled = true; //no need to set to false after being controlled as enemy will die when parasite leaves
        gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal") * movementSpeed * 3 * Time.deltaTime, 0.0f, 0.0f);

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
    }
}
