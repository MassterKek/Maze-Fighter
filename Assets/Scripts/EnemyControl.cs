using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public EnemyControl(){

    }

    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites = new Sprite[4];
    public int[] wallLocations = new int[]{6,11,12,16,18,21,26,27,28,31,34,43,44,45,54,61,62,66,70,71,76,78,84,85,86,87,88,96};
    GameObject currentTile;
    GameObject player;
    string direction = "down";
    public string state = "Patrol";
    public int enemyPos = 99;
    public int lastPos = 99;
    bool blocked = false;
    int pPos = 0;
    public float timer = 0.5f;
    public float movTime = 0;
    public int goal;
    public int health;
    public int weaponCount;

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
                lastPos = enemyPos;
                enemyPos += 10;
                UpdatePosition();
            }
        }
        else if(direction == "right")
        {
            if(!isWall(enemyPos + 1) && (enemyPos%10) != 9 && (enemyPos + 1) != pPos)
            {
                lastPos = enemyPos;
                enemyPos += 1;
                UpdatePosition();
            }
        }
        else if(direction == "left")
        {
            if(!isWall(enemyPos - 1) && (enemyPos%10) != 0 && (enemyPos - 1) != pPos)
            {
                lastPos = enemyPos;
                enemyPos -= 1;
                UpdatePosition();
            }
        }
        else
        {
            if(!isWall(enemyPos - 10) && enemyPos >= 10 && (enemyPos - 10) != pPos)
            {
                lastPos = enemyPos;
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

    // Start is called before the first frame update
    void Start()
    {
        goal = 90;
        health = 100;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
    }

    // Update is called once per frame
    void Update()
    {
        movTime += Time.deltaTime;
        UpdatePosition();

        player = GameObject.Find("Player");
        PlayerControl cs = player.GetComponent<PlayerControl>();
        pPos = cs.playerPos;

        if(weaponCount==0)state="Scout";
        else state="Patrol";

        switch(state)
        {
            case "Patrol":
                UpdatePatrol();
                break;
            case "Scout":
                UpdateScout();
                break;
            
        }

    }

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

    void UpdatePatrol()
    {
        if (movTime > timer*2)
        {
            movTime = 0;

            int[] moves = new int[]{0,0,0,0};
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
            while(choosing)
            {
                dice = UnityEngine.Random.Range(0, 4);
                if (moves[dice] == 1)
			        choosing = false;
                if (numMoves > 1 && nextTile(dice) == lastPos)
                    choosing = true;
                if(numMoves == 0)
                    choosing = false;
            }

            changeDirection(dice);
            MovePlayer();
        }

    }

    void UpdateScout()
    {
        if(movTime > timer)
        {
            movTime = 0;

            int curTile = enemyPos;

            Queue bfs = new Queue();
            bfs.Enqueue(curTile);
            ArrayList passed= new ArrayList();

            bool searching = true;
            
            //each index refers to a tile, and the value at that index represents
            //the tile that preceded it in the path
            int[] parent = new int[100];
            parent[enemyPos] = lastPos;
            while (searching)
            {
                if (upClear(curTile) && !passed.Contains(curTile+10))
                {
                    parent[curTile+10] = curTile;
                    if(!bfs.Contains(curTile+10))
                    {
                        bfs.Enqueue(curTile+10);
                    }
                }
                if (downClear(curTile) && !passed.Contains(curTile-10))
                {
                    parent[curTile-10] = curTile;
                    if(!bfs.Contains(curTile-10))
                    {
                        bfs.Enqueue(curTile-10);
                    }
                }
                if (leftClear(curTile) && !passed.Contains(curTile-1))
                {
                    parent[curTile-1] = curTile;
                    if(!bfs.Contains(curTile-1))
                    {
                        bfs.Enqueue(curTile-1);
                    }
                }
                if (rightClear(curTile) && !passed.Contains(curTile+1))
                {
                    parent[curTile+1] = curTile;
                    if(!bfs.Contains(curTile+1))
                    {
                        bfs.Enqueue(curTile+1);
                    }
                }

                passed.Add(curTile);
                try
                {
                    curTile = (int)bfs.Dequeue();
                }
                catch(Exception e)
                {
                    state = "Patrol";
                    searching = false;
                }
                
                if(curTile == goal)
                {
                    searching = false;
                }

            }

                Debug.Log("curTile = " + curTile);
                while(parent[curTile] != enemyPos)
                {
                    curTile = parent[curTile];
                }
                Debug.Log("Move to " + curTile);

                if (curTile%10 == enemyPos%10)
                    if (curTile > enemyPos)
                        changeDirection(0);
                    else
                        changeDirection(1);

                if (curTile/10 == enemyPos/10)
                    if (curTile < enemyPos)
                        changeDirection(2);
                    else
                        changeDirection(3);
                
                MovePlayer();

            if(enemyPos == goal)
            {
                weaponCount++;
                state = "Patrol";
            }
        }
    }


    int nextTile(int dir)
    {
        int temp = enemyPos;
        switch(dir)
        {
            case 0:
                temp+=10;
                break;
            case 1:
                temp-=10;
                break;
            case 2:
                temp-=1;
                break;
            case 3:
                temp+=1;
                break;
        }
        return temp;
    }

    void UpdatePosition()
    {
        currentTile = GameObject.FindWithTag(""+enemyPos);
        //Debug.Log("Moving to tile: " + enemyPos);
        transform.position = currentTile.transform.position;
    }

    public void TakeDamage (int damage) {
        health -= damage;

        if (health < 0) {
            Destroy(player.gameObject);
        }
    }

    bool isVisible()
    {
        bool foundStone = false;
        if (inRow())
        {	
            if (toRight(pPos))
            {
                for (int i=enemyPos+1; i<pPos; i++)
                {
                    if (isWall(i))
                        foundStone = true;
                }
            }
            else
            {
                for (int i=pPos+1; i<enemyPos; i++)
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
                
                for (int i=enemyPos+10; i<pPos; i+=10)
                {
                    if (isWall(i))
                        foundStone = true;
                }
            }
            else
            {
                for (int i=pPos+10; i<enemyPos; i+=10)
                {
                    if (isWall(i))
                        foundStone = true;
                }
            }
        }
        return !foundStone;
    }

    bool inRow()
    {
        bool res = false;
        if (pPos/10 == enemyPos/10)
            res = true;
        return res;
    }

    bool inCol()
    {
        bool res = false;
        if (pPos%10 == enemyPos%10)
            res = true;
        return res;
    }

    int xPos(int location)
    {
        return location%10;
    }

    private int yPos(int location)
    {
        return location/10;
    }

    private bool toRight(int objPos)
    {
        bool res = false;
        if (xPos(objPos) > xPos(enemyPos))
            res = true;
        return res;
    }

    bool toAbove(int objPos)
    {
        bool res = false;
        if (yPos(objPos) > yPos(enemyPos))
            res = true;
        return res;
    }

    bool upClear(int objPos)
    {
        return ((!isWall(objPos + 10)) && (objPos + 10 <= 99) && (objPos + 10 != pPos));
    }

    bool downClear(int objPos)
    {
        return ((!isWall(objPos - 10)) && (objPos - 10 >= 0) && (objPos - 10 != pPos));
    }

    bool leftClear(int objPos)
    {
        return ((!isWall(objPos - 1)) && (objPos%10 - 1 >= 0) && (objPos - 1 != pPos));

    }

    bool rightClear(int objPos)
    {
        return ((!isWall(objPos + 1)) && (objPos%10 + 1 <= 9) && (objPos + 1 != pPos));
    }

}
