using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform player;

    private void Update()
    {

        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);

    }



}
