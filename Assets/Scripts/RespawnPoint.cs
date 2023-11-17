using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RespawnPoint : MonoBehaviour
{
    private GameObject[] respawnPoints;
    private SpriteRenderer spriteRenderer;
    private PlayerController3 parasiteScript;
    public bool spawnParasite; //true = spawn as parasite
    public bool spawnGorilla; //true = spawn as gorilla
    public bool spawnCheetah; //true = spawn as cheetah
    public bool respawnNormalGrav; //true = spawn with normal gravity
    public Transform room;
    public bool bigRoom; //true = spawn room is a big room, false = small room

    // Start is called before the first frame update
    void Start()
    {
        parasiteScript = GameObject.Find("PlayerParasite").GetComponent<PlayerController3>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
            for (int i = 0; i < respawnPoints.Length; ++i)
            {
                spriteRenderer = respawnPoints[i].GetComponent<SpriteRenderer>();
                spriteRenderer.color = Color.white;
            }
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.green;
            parasiteScript.x_pos = transform.position.x;
            parasiteScript.y_pos = transform.position.y;
            parasiteScript.spawnParasite = spawnParasite;
            parasiteScript.spawnGorilla = spawnGorilla;
            parasiteScript.spawnCheetah = spawnCheetah;
            parasiteScript.respawnNormalGrav = respawnNormalGrav;
            parasiteScript.respawnRoom = room;
            parasiteScript.bigRoom = bigRoom;

        }

    }
}
