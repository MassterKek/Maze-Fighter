using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public static string direction = "down";
    public static int enemyPos = 99;
    public static int goal;
    public static int health;
    public static bool shotClear = false;
    public static string state = "Patrol";
    public float cdTime = 5.0f;
    public float cooldown = 5.0f;
    public int lastPos = 99;
    public float movTime = 0;
    public bool scouting = false;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites = new Sprite[4];
    public float timer = 0.5f;
    public int visGoal = goal;
    public int visHealth = health;
    public string visState = state;
    public int[] wallLocations;
    GameObject currentTile;
    int pPos = 0;
    GameObject player;

    // Lets the enemy know if a given location is a wall
    private bool isWall(int pos)
    {
        bool res = false;
        for (int i = 0; i < wallLocations.Length; i++)
        {
            if (wallLocations[i] == pos)
            {
                res = true;
            }
        }
        return res;
    }

    // Lets the enemy know if the player is to their right
    private bool toRight(int objPos)
    {
        bool res = false;
        if (xPos(objPos) > xPos(enemyPos))
            res = true;
        return res;
    }

    // returns x-coordinate of tile, player or enemy
    private int xPos(int location)
    {
        return location % 10;
    }

    // returns y-coordinate of tile, player or enemy
    private int yPos(int location)
    {
        return location / 10;
    }

    // Used to change sprites to simulate movement
    void ChangeSprite(int dir)
    {
        spriteRenderer.sprite = sprites[dir];
    }

    // Used to move enemy
    void MovePlayer()
    {
        if (direction == "up")
        {
            if (!isWall(enemyPos + 10) && enemyPos < 90 && (enemyPos + 10) != pPos)
            {
                lastPos = enemyPos;
                enemyPos += 10;
                UpdatePosition();
            }
        }
        else if (direction == "right")
        {
            if (!isWall(enemyPos + 1) && (enemyPos % 10) != 9 && (enemyPos + 1) != pPos)
            {
                lastPos = enemyPos;
                enemyPos += 1;
                UpdatePosition();
            }
        }
        else if (direction == "left")
        {
            if (!isWall(enemyPos - 1) && (enemyPos % 10) != 0 && (enemyPos - 1) != pPos)
            {
                lastPos = enemyPos;
                enemyPos -= 1;
                UpdatePosition();
            }
        }
        else
        {
            if (!isWall(enemyPos - 10) && enemyPos >= 10 && (enemyPos - 10) != pPos)
            {
                lastPos = enemyPos;
                enemyPos -= 10;
                UpdatePosition();
            }
        }

    }

    // Start
    void Start()
    {
        wallLocations = GridManager.wallLocations;
        goal = 90;
        health = 100;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
    }

    // Main Update
    void Update()
    {
        visStats();

        cooldown+=Time.deltaTime;

        // Victory Scenario
        if (health <= 0)
        {
            Destroy(gameObject, 0.0f);
        }

        movTime += Time.deltaTime;
        UpdatePosition();

        player = GameObject.Find("Player");
        pPos = PlayerControl.playerPos;

        if (scouting) state = "Scout";
        else if (health<=50) state = "Flee";
        else if(isVisible() && cooldown > cdTime) state = "Hunt";
        else state = "Patrol";

        switch (state)
        {
            case "Patrol":
                UpdatePatrol();
                break;
            case "Scout":
                UpdateScout();
                break;
            case "Flee":
                UpdateFlee();
                break;
            case "Hunt":
                UpdateHunt();
                break;
        }
    }

    // Enemy low-health behaviour
    void UpdateFlee()
    {
        if (GridManager.availableHealth > 0)
        {
            int i = 0;
            bool searching = true;
            while (searching)
            {
                if (i >= GridManager.MAX_ITEMS || GridManager.healChecks[GridManager.healZones[i]] == 1)
                    searching = false;
                else
                    i++;
            }
            if(i<GridManager.MAX_ITEMS)
            {
                goal = GridManager.healZones[i];
                UpdateScout();
            }
            else
            {
                flee();
            }
        }
        else
            flee();
    }

    // Enemy combat behaviour
    void UpdateHunt()
    {
        cooldown=0;
        movTime=0;

        if(inRow())
        {
            if(toRight(pPos))
            {
                changeDirection(3);
                shotClear = true;
            }
            else
            {
                changeDirection(2);
                shotClear = true;
            }
        }
        else if(inCol())
        {
            if(toAbove(pPos))
            {
                changeDirection(0);
                shotClear = true;
            }
            else
            {
                changeDirection(1);
                shotClear = true;
            }
        }
    }

    // Default enemy behaviour, move randomly, but not backwards unless necessary
    void UpdatePatrol()
    {
        if (movTime > timer * 2)
        {
            movTime = 0;

            int[] moves = new int[] { 0, 0, 0, 0 };
            int numMoves = 0;
            if (upClear(enemyPos))
            {
                moves[0] = 1;
                numMoves++;
            }
            if (downClear(enemyPos))
            {
                moves[1] = 1;
                numMoves++;
            }
            if (leftClear(enemyPos))
            {
                moves[2] = 1;
                numMoves++;
            }
            if (rightClear(enemyPos))
            {
                moves[3] = 1;
                numMoves++;
            }

            bool choosing = true;
            int dice = 1;
            while (choosing)
            {
                dice = UnityEngine.Random.Range(0, 4);
                if (moves[dice] == 1)
                    choosing = false;
                if (numMoves > 1 && nextTile(dice) == lastPos)
                    choosing = true;
                if (numMoves == 0)
                    choosing = false;
            }

            changeDirection(dice);
            MovePlayer();
        }

    }

    // Used to update enemy's position
    void UpdatePosition()
    {
        currentTile = GameObject.FindWithTag("" + enemyPos);
        //Debug.Log("Moving to tile: " + enemyPos);
        transform.position = currentTile.transform.position;
    }

    // Used to move enemy to certain goals efficiently using a BFS
    void UpdateScout()
    {
        if (movTime > timer)
        {
            movTime = 0;

            int curTile = enemyPos;

            Queue bfs = new Queue();
            bfs.Enqueue(curTile);
            ArrayList passed = new ArrayList();

            bool searching = true;

            //each index refers to a tile, and the value at that index represents
            //the tile that preceded it in the path
            int[] parent = new int[100];
            parent[enemyPos] = lastPos;
            while (searching)
            {
                if (upClear(curTile) && !passed.Contains(curTile + 10))
                {
                    parent[curTile + 10] = curTile;
                    if (!bfs.Contains(curTile + 10))
                    {
                        bfs.Enqueue(curTile + 10);
                    }
                }
                if (downClear(curTile) && !passed.Contains(curTile - 10))
                {
                    parent[curTile - 10] = curTile;
                    if (!bfs.Contains(curTile - 10))
                    {
                        bfs.Enqueue(curTile - 10);
                    }
                }
                if (leftClear(curTile) && !passed.Contains(curTile - 1))
                {
                    parent[curTile - 1] = curTile;
                    if (!bfs.Contains(curTile - 1))
                    {
                        bfs.Enqueue(curTile - 1);
                    }
                }
                if (rightClear(curTile) && !passed.Contains(curTile + 1))
                {
                    parent[curTile + 1] = curTile;
                    if (!bfs.Contains(curTile + 1))
                    {
                        bfs.Enqueue(curTile + 1);
                    }
                }

                passed.Add(curTile);
                try
                {
                    curTile = (int)bfs.Dequeue();
                }
                catch (Exception e)
                {
                    state = "Patrol";
                    searching = false;
                }

                if (curTile == goal)
                {
                    searching = false;
                }

            }

            Debug.Log("curTile = " + curTile);
            while (parent[curTile] != enemyPos)
            {
                curTile = parent[curTile];
            }
            Debug.Log("Move to " + curTile);

            if (curTile % 10 == enemyPos % 10)
                if (curTile > enemyPos)
                    changeDirection(0);
                else
                    changeDirection(1);

            if (curTile / 10 == enemyPos / 10)
                if (curTile < enemyPos)
                    changeDirection(2);
                else
                    changeDirection(3);

            MovePlayer();

            if (enemyPos == goal)
            {
                scouting = false;
                state = "Patrol";
            }
        }
    }

    // Used to change enemy direction based on input
    void changeDirection(int res)
    {
        ChangeSprite(res);

        switch (res)
        {
            case 0:
                direction = "up";
                break;
            case 1:
                direction = "down";
                break;
            case 2:
                direction = "left";
                break;
            case 3:
                direction = "right";
                break;
        }
    }

    // Lets the enemy know if it's safe to move down
    bool downClear(int objPos)
    {
        return ((!isWall(objPos - 10)) && (objPos - 10 >= 0) && (objPos - 10 != pPos));
    }

    // Similar to patrol, except faster and avoids player sight if possible
    void flee()
    {
        if (movTime > timer*0.75)
        {
            movTime = 0;

            int[] moves = new int[] { 0, 0, 0, 0 };
            int numMoves = 0;
            if (upClear(enemyPos))
            {
                moves[0] = 1;
                numMoves++;
            }
            if (downClear(enemyPos))
            {
                moves[1] = 1;
                numMoves++;
            }
            if (leftClear(enemyPos))
            {
                moves[2] = 1;
                numMoves++;
            }
            if (rightClear(enemyPos))
            {
                moves[3] = 1;
                numMoves++;
            }

            if(isVisible())
            {
                if(inRow())
                {
                    if(moves[0]==1||moves[1]==1)
                    {
                        if(moves[2]==1)
                        {
                            moves[2]=0;
                            numMoves--;
                        }
                        if(moves[3]==1)
                        {
                            moves[3]=0;
                            numMoves--;
                        }
                    }
                }
                else if(inCol())
                {
                    if(moves[2]==1||moves[3]==1)
                    {
                        if(moves[0]==1)
                        {
                            moves[0]=0;
                            numMoves--;
                        }
                        if(moves[1]==1)
                        {
                            moves[1]=0;
                            numMoves--;
                        }
                    }
                }
            }

            bool choosing = true;
            int dice = 1;
            while (choosing)
            {
                dice = UnityEngine.Random.Range(0, 4);
                if (moves[dice] == 1)
                    choosing = false;
                if (numMoves > 1 && nextTile(dice) == lastPos)
                    choosing = true;
                if (numMoves == 0)
                    choosing = false;
            }

            changeDirection(dice);
            MovePlayer();
        }

    }

    // Lets enemy know if player is in their column
    bool inCol()
    {
        bool res = false;
        if (pPos % 10 == enemyPos % 10)
            res = true;
        return res;
    }

    // Lets enemy know if player is in their row
    bool inRow()
    {
        bool res = false;
        if (pPos / 10 == enemyPos / 10)
            res = true;
        return res;
    }

    // Lets the enemy know if the player is in their line of sight
    bool isVisible()
    {
        bool foundStone = false;
        if (inRow())
        {
            if (toRight(pPos))
            {
                for (int i = enemyPos + 1; i < pPos; i++)
                {
                    if (isWall(i))
                        foundStone = true;
                }
            }
            else
            {
                for (int i = pPos + 1; i < enemyPos; i++)
                {
                    if (isWall(i))
                        foundStone = true;
                }
            }
        }
        else if (inCol())
        {
            if (toAbove(pPos))
            {

                for (int i = enemyPos + 10; i < pPos; i += 10)
                {
                    if (isWall(i))
                        foundStone = true;
                }
            }
            else
            {
                for (int i = pPos + 10; i < enemyPos; i += 10)
                {
                    if (isWall(i))
                        foundStone = true;
                }
            }
        }
        return !foundStone;
    }

    // Lets the enemy lnow if it's safe to move left
    bool leftClear(int objPos)
    {
        return ((!isWall(objPos - 1)) && (objPos % 10 - 1 >= 0) && (objPos - 1 != pPos));

    }

    // Used to determine movement from current tile
    int nextTile(int dir)
    {
        int temp = enemyPos;
        switch (dir)
        {
            case 0:
                temp += 10;
                break;
            case 1:
                temp -= 10;
                break;
            case 2:
                temp -= 1;
                break;
            case 3:
                temp += 1;
                break;
        }
        return temp;
    }

    // Lets the enemy know if it's safe to move right
    bool rightClear(int objPos)
    {
        return ((!isWall(objPos + 1)) && (objPos % 10 + 1 <= 9) && (objPos + 1 != pPos));
    }

    // Lets the enemy know if the player is above them
    bool toAbove(int objPos)
    {
        bool res = false;
        if (yPos(objPos) > yPos(enemyPos))
            res = true;
        return res;
    }

    // Lets the enemy know if it's safe to move up
    bool upClear(int objPos)
    {
        return ((!isWall(objPos + 10)) && (objPos + 10 <= 99) && (objPos + 10 != pPos));
    }

    // Sets stats used for testing
    void visStats()
    {
        visState = state;
        visHealth = health;
        visGoal = goal;
    }
}
