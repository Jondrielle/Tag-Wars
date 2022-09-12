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

        if (checkCondition)                                   
        {
            float step = 5.0f * Time.fixedDeltaTime;

            transform.position = Vector2.MoveTowards(dooplePosition, targetPosition, step);
            dooplePosition = transform.position;
        }
        else
        {
            Destroy(this.gameObject);
            movement.StartCoroutine(movement.StunPlayer());
        }
           
    }

    // Doopleganger pursues enemy
    public void PursueEnemy(Vector2 startLocation, Vector2 destination, PlayerManager targetPlayer)
    {
        dooplePosition = startLocation;
        targetPosition = destination;
        playerManager = targetPlayer;
    }

}
