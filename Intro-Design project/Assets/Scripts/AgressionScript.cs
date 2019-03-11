using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgressionScript : MonoBehaviour
{
    //private GameManager GMInstance;
    private Transform playerPosition;
    public float speed;
    private Rigidbody2D rb;

    public int damage;
    public float refreshTime = 0.5f;
    private Vector2 initialPosition;
    private bool moving = false;
    private bool goBack = false;
    private int direction = -1;

    // Use this for initialization
    void Start()
    {
        //GMInstance = GameManager.gameManager;
        rb = gameObject.GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    IEnumerator SeekForTarget()
    {
        yield return new WaitForSeconds(refreshTime); // wait for refreshtime in seconds
        if (GameObject.FindGameObjectWithTag("Player") != null)
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform; // find the position of the player
        if (playerPosition != null)
        {
            if (moving == false)
            {
                if (playerPosition.position.x <= initialPosition.x + 0.25 && playerPosition.position.x >= initialPosition.x - 0.25f)
                {
                    GameObject.FindObjectOfType<GameManager>().BlockPlayer();
                    moving = true;
                    if (playerPosition.position.y - initialPosition.y < 0)
                    {
                        direction = 2;
                    }
                    else if (playerPosition.position.y - initialPosition.y > 0)
                    {
                        direction = 3;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.tag == "Player")
        {
            
        }
    }

    private void Update()
    {
        StartCoroutine(SeekForTarget());

        if (goBack == false)
        {
            switch (direction)
            {
                
                case 2:
                    rb.velocity = new Vector2(0, -speed);
                    break;
                case 3:
                    rb.velocity = new Vector2(0, speed);
                    break;
            }
        }
        else if (goBack)
        {
            rb.position = Vector3.MoveTowards(rb.position, initialPosition, speed * Time.deltaTime);
            if (rb.position == initialPosition)
            {
                goBack = false;
            }
        }
    }
}

