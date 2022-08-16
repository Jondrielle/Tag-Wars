using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabMovement : MonoBehaviour
{
    public GameObject doopleganger;
    public Vector2 dooplePosition;
    public Vector2 targetPosition;
    public PlayerManager playerManager;
    public PlayerMovement movement;

    private Vector2 distanceFromTarget;
   
    void Awake()
    {
        movement = playerManager.GetComponent<PlayerMovement>();
    }
    
    void Start()
    {
        dooplePosition = doopleganger.transform.position;
        movement = playerManager.GetComponent<PlayerMovement>();
    }

    // Checks distance between doopleGanger and target
    void FixedUpdate()
    {
        bool checkCondition = Vector2.Distance(dooplePosition, targetPosition) > .1f;
        print("Check condition is: " + checkCondition);
        // check distance is in a certain threshold < .1
        // check if values are not same not the refrences
        if (checkCondition)                                   
        {
            //print(dooplePosition);
            //print(targetPosition);
            float step = 5.0f * Time.fixedDeltaTime;
            //print("Before transform: " + transform.position);
            transform.position = Vector2.MoveTowards(dooplePosition, targetPosition, step);
            dooplePosition = transform.position;
            //print("After transform: " + transform.position);
            //print("New transform" + transform.position);
        }
        else
        {
            Destroy(this.gameObject);
            //print(playerManager.gameObject.tag);
            //print(movement.gameObject.tag);
            movement.StartCoroutine(movement.StunPlayer());
            //PlayerMovement playerMovement = playerManager.GetComponent<PlayerMovement>();
            //playerMovement.StartCoroutine(playerMovement.StunPlayer());
            //print("Enemy was stunned");
            //print("Destroyed doople");
        }
           
    }

    // Doopleganger pursues enemy
    public void PursueEnemy(Vector2 startLocation, Vector2 destination, PlayerManager targetPlayer)
    {
        dooplePosition = startLocation;
        targetPosition = destination;
        playerManager = targetPlayer;
        //Debug.Log("Enemy is:" + targetPlayer);
    }

}
