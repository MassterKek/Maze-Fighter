using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static string direction = "down";
    public int health;
    public int playerPos = 0;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites = new Sprite[4];
    public int[] wallLocations;
    GameObject currentTile;
    int ePos = 99;
    GameObject enemy;

    private bool isWall(int pos)
    {
        bool res = false;
        for(int i = 0; i < wallLocations.Length; i++)
        {
            if(wallLocations[i] == pos)
            {
                res = true;
            }
        }
        return res;
    }

    void ChangeSprite(int dir)
    {
        spriteRenderer.sprite = sprites[dir];
    }

    void MovePlayer()
    {
        if(direction == "up")
        {
            if(!isWall(playerPos + 10) && playerPos < 90 && (playerPos + 10) != ePos)
            {
                playerPos += 10;
                UpdatePosition();
            }
        }
        else if(direction == "right")
        {
            if(!isWall(playerPos + 1) && (playerPos%10) != 9 && (playerPos + 1) != ePos)
            {
                playerPos += 1;
                UpdatePosition();
            }
        }
        else if(direction == "left")
        {
            if(!isWall(playerPos - 1) && (playerPos%10) != 0 && (playerPos - 1) != ePos)
            {
                playerPos -= 1;
                UpdatePosition();
            }
        }
        else
        {
            if(!isWall(playerPos - 10) && playerPos >= 10 && (playerPos - 10) != ePos)
            {
                playerPos -= 10;
                UpdatePosition();
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        wallLocations = GridManager.wallLocations;
        health = 100;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();

        enemy = GameObject.Find("Enemy");
        EnemyControl cs = enemy.GetComponent<EnemyControl>();
        ePos = cs.enemyPos;

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

        if (Input.GetKeyDown("space"))
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
