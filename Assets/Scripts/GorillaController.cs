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
    // Start is called before the first frame update
    private void Start()
    {
        isGrounded = false;
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

    }

    public void Controlled(float horizontal)
    {
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
