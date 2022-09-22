using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlyBird : MonoBehaviour
{
    public GameManager gameManager;
    public float velocity = 1;
    private Rigidbody2D rb;

    public static Action<int> OnBirdDead;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rb.velocity = Vector3.up * velocity;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector3.up * velocity;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        OnBirdDead?.Invoke(Score.score);
        gameManager.GameOver();
    }
}
