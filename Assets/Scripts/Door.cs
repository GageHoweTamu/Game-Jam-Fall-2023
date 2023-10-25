using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] public Transform partnerDoor;
    [SerializeField] private CameraController cam;
    [SerializeField] private bool bigdoor; //true means door goes to big room
    [SerializeField] private bool doortype; //true means horizontal & false means vertical

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
                cam.ChangeCamSize(8.0f,true);
            }
            else
            {
                cam.ChangeCamSize(4.0f,false);
            }

            GetComponent<BoxCollider2D>().enabled = false;
            partnerDoor.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
