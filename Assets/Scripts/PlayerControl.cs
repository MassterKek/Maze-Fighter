using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites = new Sprite[4];
    GameObject currentTile;
    string direction = "down";
    int playerPos = 0;

    void ChangeSprite(int dir)
    {
        spriteRenderer.sprite = sprites[dir];
    }

    void MovePlayer()
    {
        switch(playerPos)
        {
            case 0:
                if(direction == "up")
                {
                    playerPos = 10;
                    UpdatePosition();
                }
                else if(direction == "right")
                {
                    playerPos = 1;
                    UpdatePosition();
                }
                break;
            case 1:
                if(direction == "left")
                {
                    playerPos = 0;
                    UpdatePosition();
                }
                break;
            case 10:
                if(direction == "down")
                {
                    playerPos = 0;
                    UpdatePosition();
                }
                break;

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();

        if (Input.GetKey("up"))
        {
            ChangeSprite(0);
            direction = "up";
        }

        if (Input.GetKey("down"))
        {
            ChangeSprite(1);
            direction = "down";
        }

        if (Input.GetKey("left"))
        {
            ChangeSprite(2);
            direction = "left";
        }

        if (Input.GetKey("right"))
        {
            ChangeSprite(3);
            direction = "right";
        }

        if (Input.GetKey("space"))
        {
            MovePlayer();
        }
    }

    void UpdatePosition()
    {
        currentTile = GameObject.FindWithTag(""+playerPos);
        transform.position = currentTile.transform.position;
    }

}
