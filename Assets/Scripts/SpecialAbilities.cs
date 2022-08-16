using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class SpecialAbilities : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] PrefabMovement prefabScript;
    public GameObject prefab;
    public GameObject characterPrefab;
    private Vector2 characterPosition;
    private float timeLimit;
    private GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        timeLimit = 10;
    }

    public void CreateDoopleGanger()
    {
        characterPrefab = Instantiate(prefab, new Vector3(playerManager.player.transform.position.x + 2,
           playerManager.player.transform.position.y, playerManager.player.transform.position.z), Quaternion.identity);
        PursueEnemy();
        //DestroyDoopleGanger();
    }

    /*
     * 
     * This method destroys the doopleGanger
     * 
     */
    public void DestroyDoopleGanger()
    {
        Destroy(characterPrefab, timeLimit);
        //print("DoopleGanger was destroyed");
    }

    /*
     * 
     * DoopleGanger pursues the enemy
     * 
     */
    public void PursueEnemy()
    {
        string enemyName = EnemyToTarget();
        enemy = GameObject.Find(enemyName);
        PlayerManager targetPlayer = enemy.GetComponent<PlayerManager>();
        prefabScript.PursueEnemy(characterPosition,enemy.transform.position, targetPlayer);
    }

    /*
     * 
     *
     * 
     */
    string EnemyToTarget()
    {
        string enemyName;

        if (this.gameObject.tag == "Player")
            enemyName = "Player2";
        else
            enemyName = "Player";
       // print("Enemy Name is:" + enemyName);
        return enemyName;
    }
}
