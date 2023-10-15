using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;
    private Camera mainCamera; // Reference to the main camera

    private float initialCameraSize; // Store the initial camera size

    private int roomNumber = 1; // Update the room number based on your implementation

    private void Start()
    {
        mainCamera = Camera.main; // Get the reference to the main camera

        // Store the initial orthographic size
        initialCameraSize = mainCamera.orthographicSize;
    }

    private void Update()
    {
       
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);

        // Check if in room 8 and adjust camera size
        if (roomNumber == 8)
        {
            mainCamera.orthographicSize = 8.0f; // Set orthographic size to 8.0f
        }
        else
        {
            mainCamera.orthographicSize = initialCameraSize; // Reset to initial orthographic size
        }
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }

    public void SetRoomNumber(bool forb)
    {
        if (forb)
        {
            roomNumber++;
        }
        else
        {
            roomNumber--;
        }
    }

    public void ChangeCameraSize(int newRoomNumber)
    {
        if (newRoomNumber == 8)
        {
            mainCamera.orthographicSize = 16.0f;
        }
        else
        {
            mainCamera.orthographicSize = 4.0f;
        }
    }
}
