using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2;
    private bool isGrounded;
    // Start is called before the first frame update
    private void Start()
    {
        isGrounded = false;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Platform"))
        {
            isGrounded = true;
            
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            if (Input.GetAxis("Horizontal") != 0.0f)
            {
                gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime, 0.0f, 0.0f);
                // gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime, 0.0f, 0.0f), ForceMode2D.Impulse);
            }
        }
        else
        {
            if (Input.GetAxis("Horizontal") != 0.0f)
            {
                gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal") * movementSpeed * 2 * Time.deltaTime, 0.0f, 0.0f);
                // gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime, 0.0f, 0.0f), ForceMode2D.Impulse);
            }
        }



        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {

            isGrounded = false;
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 9.0f, ForceMode2D.Impulse);
        }
    }
}
