using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravityFlipperScript : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private PlayerController3 parasiteScript;
    private SpriteRenderer spriteRenderer;
    private float timer;
    public float delay = 1;
    private GameObject[] gravityFlippers;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        parasiteScript = GameObject.Find("PlayerParasite").GetComponent<PlayerController3>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (timer == 0 || Time.time - timer > delay)
            {
                if (parasiteScript != null)
                {
                    parasiteScript.GravityFlip();
                    gravityFlippers = GameObject.FindGameObjectsWithTag("GravityFlipper");
                    for (int i = 0; i < gravityFlippers.Length; ++i)
                    {
                        spriteRenderer = gravityFlippers[i].GetComponent<SpriteRenderer>();
                        Vector3 localScaleFlipper = gravityFlippers[i].transform.localScale;
                        localScaleFlipper.y *= -1f;
                        gravityFlippers[i].transform.localScale = localScaleFlipper;
                        if (localScaleFlipper.y > 0f)
                        {
                            spriteRenderer.color = Color.blue;
                        }
                        else
                        {
                            spriteRenderer.color = Color.red;
                        }
                    }
                }
                else
                {
                    Debug.Log("couldnt find parasite script");
                }
            }
            timer = Time.time;
        }
    }

    public void ResetFlippers(bool normalGrav, bool respawnNormalGrav)
    {
        if (!(normalGrav == respawnNormalGrav))
        {
            parasiteScript.GravityFlip();
            gravityFlippers = GameObject.FindGameObjectsWithTag("GravityFlipper");
            for (int i = 0; i < gravityFlippers.Length; ++i)
            {
                spriteRenderer = gravityFlippers[i].GetComponent<SpriteRenderer>();
                Vector3 localScaleFlipper = gravityFlippers[i].transform.localScale;
                localScaleFlipper.y *= -1f;
                gravityFlippers[i].transform.localScale = localScaleFlipper;
                if (localScaleFlipper.y > 0f)
                {
                    spriteRenderer.color = Color.blue;
                }
                else
                {
                    spriteRenderer.color = Color.red;
                }
            }
        }

    }
}
