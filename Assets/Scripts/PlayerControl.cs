using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static float cdTime = 5.0f;
    public static float cooldown = 5.0f;
    public static string direction = "down";
    public static int health;
    public static int playerPos = 0;
    public static int weaponCount = 0;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites = new Sprite[4];
    public int[] wallLocations;
    GameObject currentTile;
    int ePos = 99;
    GameObject enemy;

    // Lets the player know if a given location is a wall
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

    // Used to change sprites to simulate movement
    void ChangeSprite(int dir)
    {
        spriteRenderer.sprite = sprites[dir];
    }

    // Used to move player
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

    // Start
    void Start()
    {
        wallLocations = GridManager.wallLocations;
        health = 100;
        weaponCount = 2;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
    }

    // Main Update
    void Update()
    {

        // Game Over Scenario
        if (health <= 0)
        {
            Destroy(gameObject, 0.0f);
        }

        UpdatePosition();

        cooldown+=Time.deltaTime;

        enemy = GameObject.Find("Enemy");
        ePos = EnemyControl.enemyPos;

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

        // if (Input.GetMouseButtonDown(0) && weaponCount>0) {
        //     weaponCount-=1;
        //     Gun.shoot();
        // }
    }

    // Used to update player's position
    void UpdatePosition()
    {
        currentTile = GameObject.FindWithTag(""+playerPos);
        transform.position = currentTile.transform.position;
    }

}
