using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] public Transform partnerDoor;
    [SerializeField] private CameraController cam;
    public BoxCollider2D boxCollider;
    public BoxCollider2D boxCollider2;
    [SerializeField] private bool bigdoor; //true means door goes to big room
    [SerializeField] private bool doortype; //true means horizontal & false means vertical
    [SerializeField] private PlayerController3 parasite;
    public float xpos = 10; //x-position of player when they walk through door(for respawn)
    public float ypos = 10; //y-position of player when they walk through door(for respawn)
    public GameObject gorillaControlPopUp;
    public GameObject cheetahControlPopUp;
    void Start()
    {
        parasite = GameObject.Find("PlayerParasite").GetComponent<PlayerController3>();
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider2 = partnerDoor.GetComponent<BoxCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.tag == "Player")
        {
            //parasite.GetRespawnPos(collision.transform);
            if (collision.transform.position.x < transform.position.x && doortype)
            {
                ypos = collision.transform.position.y;
                xpos = collision.transform.position.x;
                UnityEngine.Debug.Log(xpos);
                UnityEngine.Debug.Log(ypos);
                cam.MoveToNewRoom(nextRoom);
                gorillaControlPopUp.SetActive(false);
                cheetahControlPopUp.SetActive(false);

            }
            else if (collision.transform.position.x > transform.position.x && doortype)
            {
                ypos = collision.transform.position.y;
                xpos = collision.transform.position.x;
                UnityEngine.Debug.Log(xpos);
                UnityEngine.Debug.Log(ypos);
                cam.MoveToNewRoom(previousRoom);
                gorillaControlPopUp.SetActive(false);
                cheetahControlPopUp.SetActive(false);

            }
            else if (collision.transform.position.y < transform.position.y && !doortype)
            {
                ypos = collision.transform.position.y;
                xpos = collision.transform.position.x;
                UnityEngine.Debug.Log(xpos);
                UnityEngine.Debug.Log(ypos);
                cam.MoveToNewRoom(nextRoom);
                gorillaControlPopUp.SetActive(false);
                cheetahControlPopUp.SetActive(false);

            }
            else if (collision.transform.position.y > transform.position.y && !doortype)
            {
                ypos = collision.transform.position.y;
                xpos = collision.transform.position.x;
                UnityEngine.Debug.Log(xpos);
                UnityEngine.Debug.Log(ypos);
                cam.MoveToNewRoom(previousRoom);
                gorillaControlPopUp.SetActive(false);
                cheetahControlPopUp.SetActive(false);

            }

            if (bigdoor == true)
            {
                cam.ChangeCamSize(8.0f,true);
            }
            else
            {
                cam.ChangeCamSize(4.0f,false);
            }

            boxCollider.enabled = false;
            boxCollider2.enabled = true;
        }
    }
}
