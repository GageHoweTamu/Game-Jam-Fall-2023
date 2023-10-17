using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;
    [SerializeField] private int doornumber;
    [SerializeField] private bool doortype; //true means horizontal & false means vertical
    private bool direction = true;
    public bool changingcamerasize = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x && doortype)
            {
                cam.MoveToNewRoom(nextRoom);
                direction = true;
            }
            else if (collision.transform.position.x > transform.position.x && doortype)
            {
                cam.MoveToNewRoom(previousRoom);
                direction = false;
            }
            else if (collision.transform.position.y < transform.position.y && !doortype)
            {
                cam.MoveToNewRoom(nextRoom);
                direction = true;
            }
            else if (collision.transform.position.y > transform.position.y && !doortype)
            {
                cam.MoveToNewRoom(previousRoom);
                direction = false;
            }

            // Check for door 7 after updating roomnumber
            if (doornumber == 7)
            {
                changingcamerasize = true;
            }
        }
    }

    private void Update()
    {
        if (changingcamerasize == true)
        {
            changingcamerasize = cam.ChangeCameraSize(doornumber,direction);
        }
    }
}
