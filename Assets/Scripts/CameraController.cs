using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    private float currentPosX;
    private float currentPosY = -0.82f;
    private Vector3 velocity = Vector3.zero;
    private Camera mainCamera; // Reference to the main camera
    private float initialCameraSize; // Store the initial camera size
    private float zoomspeed = 10.0f;
    public bool changingcamerasize = false;
    private bool shrinking = false;
    private bool growing = false;

    private void Start()
    {
        mainCamera = Camera.main; // Get the reference to the main camera
        // Store the initial orthographic size
        initialCameraSize = mainCamera.orthographicSize;
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, currentPosY, transform.position.z), ref velocity, speed);
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
        currentPosY = _newRoom.position.y;
    }


    public bool ChangeCameraSize(bool big)
    {
        if (big == true)
        {
            mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, 8.0f, ref zoomspeed, .5f);
            if (mainCamera.orthographicSize == 8.0f)
            {
                growing = false;
            }

        }
        else
        {
            mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, 4.0f, ref zoomspeed, .5f);
            if (mainCamera.orthographicSize == 4.0f)
            {
                growing = false;
            }
        }

        if (mainCamera.orthographicSize == 8.0f && growing == false )
        {
            changingcamerasize = false;
        }

        else if (mainCamera.orthographicSize == 4.0f && shrinking == false )
        {
            changingcamerasize = false;
        }

        else
        {
            changingcamerasize = true;
        }

        return changingcamerasize;
    }
}
