using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;
    public int roomnumber = 1;
    private bool direction = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)
            {
                cam.MoveToNewRoom(nextRoom);
                direction = true;
                cam.SetRoomNumber(direction); // Update the room number in the camera controller
            }
            else
            {
                cam.MoveToNewRoom(previousRoom);
                direction = false;
                cam.SetRoomNumber(direction); // Update the room number in the camera controller
            }

            // Check for room 8 after updating roomnumber
            if (roomnumber == 8)
            {
                cam.ChangeCameraSize(roomnumber);
            }
        }
    }
}
