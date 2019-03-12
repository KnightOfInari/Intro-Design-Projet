﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private GameObject answerUI;

    private bool move;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        move = true;
    }

    public void MovingAllowed(bool allowed)
    {
        move = allowed;
    }

    public void ShowUI(bool show)
    {
        answerUI.SetActive(show);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.RightArrow) && move == true)
        {
            rb.velocity = transform.right * speed;
        }
    }
}
