using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites = new Sprite[4];
    public int[] wallLocations = new int[]{6,11,12,16,18,21,26,27,28,31,34,43,44,45,54,61,62,66,70,71,76,78,84,85,86,87,88,96};
    public string[] states = new string[]{"Patrol","Track","Heal", "Hunt"};
    public string curState = "Patrol";
    GameObject currentTile;
    GameObject player;
    string direction = "down";
    public int enemyPos = 99;
    int pPos = 0;
    float dirTime = 0;
    float movTime = 0;

    void ChangeSprite(int dir)
    {
        spriteRenderer.sprite = sprites[dir];
    }

    void MovePlayer()
    {
                if(direction == "up")
                {
                    if(!isWall(enemyPos + 10) && enemyPos < 90 && (enemyPos + 10) != pPos)
                    {
                        enemyPos += 10;
                        UpdatePosition();
                    }
                }
                else if(direction == "right")
                {
                    if(!isWall(enemyPos + 1) && (enemyPos%10) != 9 && (enemyPos + 1) != pPos)
                    {
                        enemyPos += 1;
                        UpdatePosition();
                    }
                }
                else if(direction == "left")
                {
                    if(!isWall(enemyPos - 1) && (enemyPos%10) != 0 && (enemyPos - 1) != pPos)
                    {
                        enemyPos -= 1;
                        UpdatePosition();
                    }
                }
                else
                {
                    if(!isWall(enemyPos - 10) && enemyPos >= 10 && (enemyPos - 10) != pPos)
                    {
                        enemyPos -= 10;
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

    void lookUp()
    {
        ChangeSprite(0);
        direction = "up";
    }
    void lookDown()
    {
        ChangeSprite(1);
        direction = "down";
    }
    void lookLeft()
    {
        ChangeSprite(2);
        direction = "left";
    }
    void lookRight()
    {
        ChangeSprite(3);
        direction = "right";
    }

    void UpdatePatrolState()
    {
        // Change direction every 2 seconds       
        if (dirTime > 1.0)
        {
            dirTime = 0;
            int dice = Random.Range(1, 5);;

            switch (dice)
            {
                case 1:
                    lookUp();
                    break;
                case 2:
                    lookDown();
                    break;
                case 3:
                    lookLeft();
                    break;
                case 4:
                    lookRight();
                    break;
            }
        }
        // Move every second
        if (movTime > 1.0) 
        {
            movTime = 0;
            MovePlayer();
        }
    }

    void UpdatePosition()
    {
        currentTile = GameObject.FindWithTag(""+enemyPos);
        //Debug.Log("Moving to tile: " + enemyPos);
        transform.position = currentTile.transform.position;
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
        dirTime += Time.deltaTime;
        movTime += Time.deltaTime;
        UpdatePosition();

        player = GameObject.Find("Player");
        PlayerControl cs = player.GetComponent<PlayerControl>();
        pPos = cs.playerPos;

        switch (curState)
        {
            case "Patrol": 
                UpdatePatrolState();
                break;
        }


    }

}
