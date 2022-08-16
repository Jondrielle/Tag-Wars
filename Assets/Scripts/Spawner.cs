using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    private double candyZPosition;

    public GameObject[] prefabList = new GameObject[2];
    public GameObject teleportPrefab;
    public TilemapGenerator map;
    public GameObject prefab;

    private void Awake()
    {
        prefabList[0] = prefab;
        prefabList[1] = teleportPrefab;
    }
    void Start()
    {
        candyZPosition = -0.18;
        StartCoroutine(Spawn());
    }

    /*
     * Randomly creates an instance 
     * of a crate and randomly destorys it
     */
    IEnumerator Spawn()
    {
        while (true)
        {
            //randomly retrieve index and retrieve index value
            int getPrefabIndex = RetrievePrefab();
            GameObject spawnPrefab = prefabList[getPrefabIndex];

            //wait then spawn the prefab retrieved and spawn it on the map
            yield return new WaitForSecondsRealtime(RandomSpawnTime());
            GameObject newPrefab = Instantiate(spawnPrefab, new Vector3(RandomXLocation(), RandomYLocation(), (float)candyZPosition), Quaternion.identity);
            prefab.SetActive(true);

            StartCoroutine(Spawn());

            //destroy the prefab
            yield return new WaitForSecondsRealtime(RandomSpawnDestoryTime());
            Destroy(newPrefab);
        }
    }

    //Get a random prefab
    public int RetrievePrefab()
    {
        return Random.Range(0, prefabList.Length);
    }

    /*
     * Randomly picks and x location for 
     * crate to generate 
     */
    public float RandomXLocation()
    {
        GameObject tileGameObject = map.ReturnRandomTile();
        float xSpawnLocation = tileGameObject.transform.position.x;
        //Debug.Log("X Location is:" + xSpawnLocation);
        return xSpawnLocation;
    }

    /*
     * Randomly picks and y location for 
     * crate to generate 
     */
    public float RandomYLocation()
    {
        GameObject tileGameObject = map.ReturnRandomTile();
        float ySpawnLocation = tileGameObject.transform.position.y;
        return ySpawnLocation;
    }

    /*
     * Randomly picks when a crate is spawned
     */
    public float RandomSpawnTime()
    {
        float randomSpawnTime = Random.Range(10, 30);
        return randomSpawnTime;
    }

    /*
     * Randomly picks when a crate will be destoryed
     */
    public float RandomSpawnDestoryTime(){
        float randomSpawnDestoryTime = Random.Range(10, 30);
        return randomSpawnDestoryTime;
    }
}
