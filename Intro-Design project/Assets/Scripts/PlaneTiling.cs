﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class PlaneTiling : MonoBehaviour
{

    public int offsetX = 2;
    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;

    public bool reverseScale = false; // use if the object is not tillable

    private float spriteWidth = 0f; // the width of the element
    private Camera cam;
    private Transform myTransform;

    // Start is called before the first frame update
    private void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }

    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasALeftBuddy == false || hasARightBuddy == false)
        {
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;

            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false)
            {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }

            if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false)
            {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
    }

    void MakeNewBuddy(int rightOrLeft)
    {
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

        if (reverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = myTransform.parent;
        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<PlaneTiling>().hasALeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<PlaneTiling>().hasARightBuddy = true;
        }
    }
}
