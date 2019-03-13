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

    Animator anim;

    Vector2 move;
    // Use this for initialization
    void Start()
    {
        //GMInstance = GameManager.gameManager;
        rb = gameObject.GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        move = Vector2.zero;
        anim = GetComponent<Animator>();
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
                        move = new Vector2(0, -speed);
                    }
                    else if (playerPosition.position.y - initialPosition.y > 0)
                    {
                        move = new Vector2(0, speed);
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Bonked the player");
            GameObject.FindObjectOfType<GameManager>().PlayerCanAnswer();

            move = Vector2.zero;
            anim.SetBool("Walking", false);

        }
    }

    private void Update()
    {
        StartCoroutine(SeekForTarget());

        if (goBack == false)
        {
            rb.velocity = move;
            if (rb.velocity != Vector2.zero)
                anim.SetBool("Walking", true);
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

