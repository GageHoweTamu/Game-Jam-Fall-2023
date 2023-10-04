using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform firePoint;
    public GameObject shockwavePrefab;
    public GameObject gorilla;
    public Vector3 shockwave1;
    public Vector3 shockwave2;
    public GameObject shock1;
    public GameObject shock2;
    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindWithTag("Player").name == "Gorilla" && GameObject.FindWithTag("Player").GetComponent<GorillaController>().isGrounded)
        { 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UnityEngine.Debug.Log("Shoot");
                Shoot();
            }
        }


        


    }

    void Shoot()
    {
        shockwave1 = new Vector3(firePoint.position.x + 0.3f, firePoint.position.y, 0f);
        shockwave2 = new Vector3(firePoint.position.x - 0.3f, firePoint.position.y, 0f);
        shock1 = Instantiate(shockwavePrefab, shockwave1, firePoint.rotation);
        shock2 = Instantiate(shockwavePrefab, shockwave2, firePoint.rotation);
        Invoke("Dissappear", 1);
    }

    void Dissappear()
    {
        Destroy(shock1);
        Destroy(shock2);
    }
}