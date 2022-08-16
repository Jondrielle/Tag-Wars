using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
/* Danndx 2021 (youtube.com/danndx)
From video: youtu.be/qNZ-0-7WuS8
thanks - delete me! :) */
 

public class TilemapGenerator : MonoBehaviour
{
    Dictionary<int, GameObject> tileset;
    Dictionary<int, GameObject> tile_groups;
    public GameObject prefab_dirt;
    public GameObject prefab_water;
    public GameObject prefab_grass;
    public GameObject prefab_dirtbush;
 
    public int map_width = 16;
    public int map_height = 9;
 
    List<List<int>> noise_grid = new List<List<int>>();
    List<List<GameObject>> tile_grid = new List<List<GameObject>>();
    public List<GameObject> tiles_all;
    public List<GameObject> borderTiles;
    


 
    // recommend 4 to 20
    public float magnification = 7.0f;
 
    public int x_offset = 0; // <- +>
    public int y_offset = 0; // v- +^

    public float XposOffset, YposOffset;
 

    public void Start(){
        GenerateTileMap();
    }


    [ContextMenu("Spawn Tile Formation")]
    public void GenerateTileMap()
    {
        tiles_all = new List<GameObject>();
        CreateTileset();
        CreateTileGroups();
        GenerateMap();
    }
 
    void CreateTileset()
    {
        /** Collect and assign ID codes to the tile prefabs, for ease of access.
            Best ordered to match land elevation. **/
 
        tileset = new Dictionary<int, GameObject>();
        tileset.Add(1, prefab_dirt);
        tileset.Add(0, prefab_water);
        tileset.Add(2, prefab_grass);
        tileset.Add(3, prefab_dirtbush);
    }
 
    void CreateTileGroups()
    {
        /** Create empty gameobjects for grouping tiles of the same type, ie
            forest tiles **/
 
        tile_groups = new Dictionary<int, GameObject>();
        foreach(KeyValuePair<int, GameObject> prefab_pair in tileset)
        {
            GameObject tile_group = new GameObject(prefab_pair.Value.name);
            tile_group.transform.parent = gameObject.transform;
            tile_group.transform.localPosition = new Vector3(0, 0, 0);

            tile_groups.Add(prefab_pair.Key, tile_group);
        }
    }
 
    void GenerateMap()
    {
        /** Generate a 2D grid using the Perlin noise fuction, storing it as
            both raw ID values and tile gameobjects **/
 
        for(int x = 0; x < map_width; x++)
        {
            noise_grid.Add(new List<int>());
            tile_grid.Add(new List<GameObject>());
 
            for(int y = 0; y < map_height; y++)
            {
                int tile_id = GetIdUsingPerlin(x, y);
                noise_grid[x].Add(tile_id);
                CreateTile(tile_id, x, y);
            }
        }
    }
 
    int GetIdUsingPerlin(int x, int y)
    {
        /** Using a grid coordinate input, generate a Perlin noise value to be
            converted into a tile ID code. Rescale the normalised Perlin value
            to the number of tiles available. **/
 
        float raw_perlin = Mathf.PerlinNoise(
            (x - x_offset) / magnification,
            (y - y_offset) / magnification
        );
        float clamp_perlin = Mathf.Clamp01(raw_perlin); // Thanks: youtu.be/qNZ-0-7WuS8&lc=UgyoLWkYZxyp1nNc4f94AaABAg
        float scaled_perlin = clamp_perlin * tileset.Count;
 
        // Replaced 4 with tileset.Count to make adding tiles easier
        if(scaled_perlin == tileset.Count)
        {
            scaled_perlin = (tileset.Count - 1);
        }
        return Mathf.FloorToInt(scaled_perlin);
    }
 
    void CreateTile(int tile_id, int x, int y)
    {
        /** Creates a new tile using the type id code, group it with common
            tiles, set it's position and store the gameobject. **/
 
        GameObject tile_prefab = tileset[tile_id];
        GameObject tile_group = tile_groups[tile_id];
        GameObject tile = Instantiate(tile_prefab, tile_group.transform);


 
        tile.name = string.Format("tile_x{0}_y{1}", x, y);
        tile.transform.localPosition = new Vector3(x * XposOffset, y * YposOffset, 0);
        tile.transform.localScale = new Vector3(6, 6, 1);
 

        tile_grid[x].Add(tile);
        tiles_all.Add(tile);

        CheckIfBorderTile(tile,x,y);
    }

    // returns a random tile gameobject
    public GameObject ReturnRandomTile(){
        return tiles_all[Random.Range (0, tiles_all.Count)];
    }

    public void CheckIfBorderTile(GameObject tile, int tileX, int tileY)
    {
        //x0 || x99 y0 || y99
       if(tileX == 0 || tileX == 99 || tileY == 0 || tileY == 99)
       {
            borderTiles.Add(tile);
       }
    }

    public void GetBorderTiles(Camera playerCamera)
    {
        //make sure x is not greater than 115.5 or less than -42.8
       //make sure y is not less than -18.4 or greater than  143
        //18.3, -18.4, -56.8)
        // (115.5, -14.4, -56.8)
        foreach (GameObject tile in borderTiles)
        {
            Vector3 tilesLocalPos = tile.transform.position;
            print("Player camera position is: " + playerCamera.transform.position);
            print("Player tile position is: " + tilesLocalPos);
            if (playerCamera.transform.localPosition.Equals(tilesLocalPos))
            {
                print("Stop moving the players camera");
            }
        }
    }
}