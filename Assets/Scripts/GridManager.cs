using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public static int[] wallLocations = new int[]{11,12,16,18,21,26,28,31,34,43,44,45,54,61,62,66,71,76,78,84,85,86,88};
    private int cols = 10;

    private int rows = 10;
    private float tileSize = 1;

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
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
