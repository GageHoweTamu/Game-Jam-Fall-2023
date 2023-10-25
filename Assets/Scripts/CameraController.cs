using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float orthospeed = 2f;
    private float currentPosX;
    private float currentPosY = -0.82f;
    private Vector3 velocity = Vector3.zero;
    private Camera mainCamera;
    private float roomsize = 4.0f;
    private bool changing = false;


    private void Start()
    {
        mainCamera = Camera.main; // Get the reference to the main camera
        mainCamera.orthographicSize = 4.0f;
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, currentPosY, transform.position.z), ref velocity, speed);
        if (changing == true)
        {
            if (mainCamera.orthographicSize < 7.9f)
            {
                mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, roomsize, ref orthospeed, speed);
            }
        }
        else if (changing == false)
        {
            if (mainCamera.orthographicSize > 4.1f)
            {
                mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, roomsize, ref orthospeed, speed);
            }
        }
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
        currentPosY = _newRoom.position.y;
    }

    public void ChangeCamSize(float size, bool isChanging) //true is big false is small
    {
        roomsize = size;
        changing = isChanging;
    }
}