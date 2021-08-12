using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int MAX_ITEMS = 4;
    public static int availableHealth = 0;
    public static int[] healChecks = new int[100];
    public static int[] healZones = new int[]{9, 35, 53, 90};
    public static int[] wallLocations = new int[]{11,12,16,18,21,26,28,31,34,43,44,45,54,61,62,66,71,76,78,84,85,86,88};
    public static int[] weaponChecks = new int[100];
    public static int[] weaponZones = new int[]{22, 33, 55, 87};
    public GameObject healthDrop;
    public float spawnTime = 10.0f;
    public int weapons, heals = 0;
    private int cols, rows = 10;
    private float tileSize = 1;
    float spawnTimer = 0;

    public static int availableWeapon()
    {

    }

    private void GenerateGrid()
    {
        int tileCount = 0;
        GameObject referenceTile = (GameObject)Instantiate(Resources.Load("TileSprite"));
        GameObject referenceStone = (GameObject)Instantiate(Resources.Load("WallStone"));
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject tile = (GameObject)Instantiate(referenceTile, transform);
                tile.tag = ""+tileCount;
                tileCount++;

                float posX = col * tileSize;
                float posY = row * tileSize;

                tile.transform.position = new Vector2(posX, posY);

                if(isWall(tileCount-1))
                {
                    GameObject wall = (GameObject)Instantiate(referenceStone, transform);
                    wall.transform.position = new Vector2(posX, posY);
                }

            }
        }
        Destroy(referenceTile);
        Destroy(referenceStone);

        float gridW = cols * tileSize;
        float gridH = rows * tileSize;

        transform.position = new Vector2(-gridW/2 + tileSize/2, -((gridH/2)-1) - tileSize/2);
    }

    private void initializeItems()
    {
        for(int i = 0; i < 4; i++)
        {
            healChecks[healZones[i]] = 0;
            weaponChecks[weaponZones[i]] = 0;
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

    // Checks if any players have picked up an item, and sets the check arrays accordingly
    private void itemTracker()
    {
        for (int i = 0; i < MAX_ITEMS; i++)
        {
            if((Player.playerPos == healZones[i] || Enemy.enemyPos == healZones[i]) && healChecks[healZones[i]] == 1 ){
                healChecks[healZones[i]] = 0;
                availableHealth-=1;
            }

            if((Player.playerPos == weaponZones[i] || Enemy.enemyPos == weaponZones[i]) && weaponChecks[weaponZones[i]] == 1 ){
                weaponChecks[weaponZones[i]] = 0;
                availableWeapon-=1;
            }
        }
    }

    void Start()
    {
        initializeItems();
        GenerateGrid();
    }

    void Update()
    {
        spawnTimer+=Time.deltaTime;
        if(spawnTimer>spawnTime)
        {
            spawnTimer=0;
            if (availableHealth < MAX_ITEMS)
            {
                int chance = UnityEngine.Random.Range(0, 2);
                if (chance > 0)
                {
                    bool choosing = true;
                    int square;
                    while(choosing)
                    {
                        square = UnityEngine.Random.Range(0, MAX_ITEMS);
                        if(healChecks[healZones[square]]==0)
                        {
                            choosing = false;
                        }
                    }
                    healChecks[healZones[square]] = 1;
                    availableHealth+=1;

                    GameObject currentTile = GameObject.FindWithTag(""+healZones[square]);
                    GameObject pack = Instantiate(healthDrop);
                    pack.transform.position=currentTile.transform.position;

                }
            }
        }

    }
}
