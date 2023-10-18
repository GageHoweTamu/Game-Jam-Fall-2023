using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;
    [SerializeField] private bool bigdoor;
    [SerializeField] private bool doortype; //true means horizontal & false means vertical
    public bool changingcamerasize = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x && doortype)
            {
                cam.MoveToNewRoom(nextRoom);
            }
            else if (collision.transform.position.x > transform.position.x && doortype)
            {
                cam.MoveToNewRoom(previousRoom);
            }
            else if (collision.transform.position.y < transform.position.y && !doortype)
            {
                cam.MoveToNewRoom(nextRoom);
            }
            else if (collision.transform.position.y > transform.position.y && !doortype)
            {
                cam.MoveToNewRoom(previousRoom);
            }

            if (bigdoor == true)
            {
                changingcamerasize = true;
            }
        }
    }

    private void Update()
    {
        if (changingcamerasize == true)
        {
            changingcamerasize = cam.ChangeCameraSize(bigdoor);
        }
    }
}
