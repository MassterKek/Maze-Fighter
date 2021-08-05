using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites = new Sprite[4];
    public int[] wallLocations = new int[]{6,11,12,16,18,21,26,27,28,31,34,43,44,45,54,61,62,66,70,71,76,78,84,85,86,87,88,96};
    GameObject currentTile;
    GameObject enemy;
    public static string direction = "down";
    public int playerPos = 0;
    int ePos = 99;
    public int health;

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

    // Start is called before the first frame update
    void Start()
    {   
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
