using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crumblingPlatformScript : MonoBehaviour
{
    public float disappearTime = 0.5f;
    public float appearTime = 1.5f;
    private SpriteRenderer sprite;
    private BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Gorilla") || collision.gameObject.CompareTag("Cheetah"))
        {
            Invoke(nameof(Disappear), disappearTime);
            Invoke(nameof(Appear), appearTime);
        }
    }

    private void Disappear()
    {
        sprite.enabled = false;
        boxCollider.enabled = false;
    }

    private void Appear()
    {
        sprite.enabled = true;
        boxCollider.enabled = true;
    }
}
